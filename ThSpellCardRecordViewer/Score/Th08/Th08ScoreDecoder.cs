﻿using System.Text;

namespace ThSpellCardRecordViewer.Score.Th08
{
    internal class Th08ScoreDecoder
    {
        public static bool Convert(string scorePath, Stream outputData)
        {
            using FileStream input = new(scorePath, FileMode.Open);
            using MemoryStream memoryStream = new();

            bool decryptResult = Decrypt(input, memoryStream);
            if (decryptResult)
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                return Extract(memoryStream, outputData);
            }
            else
            {
                return false;
            }
        }

        /**
        The Decrypt(Stream input, Stream output) function,
        
        The Extract(Stream input, Stream output) function
        and
        The ThDecrypt(Stream input, Stream output, int size, byte key, byte step, int block, int limit) function
        are

        Copyright (c) 2013, IIHOSHI Yoshinori

        Redistribution and use in source and binary forms, with or without
        modification, are permitted provided that the following conditions are met:

        * Redistributions of source code must retain the above copyright notice, this
          list of conditions and the following disclaimer.

        * Redistributions in binary form must reproduce the above copyright notice,
          this list of conditions and the following disclaimer in the documentation
          and/or other materials provided with the distribution.

        THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
        AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
        IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
        DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
        FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
        DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
        SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
        CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
        OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
        OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
        **/
        public static bool Decrypt(Stream input, Stream output)
        {
            int size = (int)input.Length;
            ThDecrypt(input, output, size, 0x59, 0x79, 0x0100, 0x0C00);

            byte[] data = new byte[size];
            _ = output.Seek(0, SeekOrigin.Begin);
            _ = output.Read(data, 0, size);

            uint checksum = 0;
            byte temp = 0;
            for (int index = 2; index < size; index++)
            {
                temp += data[index - 1];
                temp = (byte)((temp >> 5) | (temp << 3));
                data[index] ^= temp;
                if (index > 3)
                    checksum += data[index];
            }

            _ = output.Seek(0, SeekOrigin.Begin);
            output.Write(data, 0, size);

            return (ushort)checksum == BitConverter.ToUInt16(data, 2);
        }

        public static bool Extract(Stream input, Stream output)
        {
            using BinaryReader reader = new(input, Encoding.UTF8, true);
            using BinaryWriter writer = new(output, Encoding.UTF8, true);
            Th08FileHeader header = new();

            header.ReadFrom(reader);
            if (!header.IsValid)
                return false;
            if (header.Size + header.EncodedBodySize != input.Length)
                return false;

            header.WriteTo(writer);

            Lzss.Decompress(input, output);
            output.Flush();
            output.SetLength(output.Position);

            return output.Position == header.DecodedAllSize;
        }

        private static void ThDecrypt(
            Stream input, Stream output, int size, byte key, byte step, int block, int limit)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size));
            if (block < 0)
                throw new ArgumentOutOfRangeException(nameof(block));
            if (limit < 0)
                throw new ArgumentOutOfRangeException(nameof(limit));

            byte[] inBlock = new byte[block];
            byte[] outBlock = new byte[block];
            int addup;

            addup = size % block;
            if (addup >= block / 4)
                addup = 0;
            addup += size % 2;
            size -= addup;

            while ((size > 0) && (limit > 0))
            {
                if (size < block)
                    block = size;
                if (input.Read(inBlock, 0, block) != block)
                    return;

                int inIndex = 0;
                for (int j = 0; j < 2; ++j)
                {
                    int outIndex = block - j - 1;
                    for (int i = 0; i < (block - j + 1) / 2; ++i)
                    {
                        outBlock[outIndex] = (byte)(inBlock[inIndex] ^ key);
                        inIndex++;
                        outIndex -= 2;
                        key += step;
                    }
                }

                output.Write(outBlock, 0, block);
                limit -= block;
                size -= block;
            }

            size += addup;
            if (size > 0)
            {
                byte[] restbuf = new byte[size];
                if (input.Read(restbuf, 0, size) != size)
                    return;
                output.Write(restbuf, 0, size);
            }
        }
    }
}
