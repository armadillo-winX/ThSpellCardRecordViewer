﻿using System.Text;

namespace ThSpellCardRecordViewer.Score.Th07
{
    internal class Th07ScoreDecoder
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
        The Decrypt(Stream input, Stream output) function
        and
        The Extract(Stream input, Stream output) function are

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
        private static bool Decrypt(Stream input, Stream output)
        {
            int size = (int)input.Length;
            byte[] data = new byte[size];
            _ = input.Read(data, 0, size);

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

            output.Write(data, 0, size);

            return (ushort)checksum == BitConverter.ToUInt16(data, 2);
        }

        private static bool Extract(Stream input, Stream output)
        {
            using BinaryReader reader = new(input, Encoding.UTF8, true);
            using BinaryWriter writer = new(output, Encoding.UTF8, true);
            Th07FileHeader header = new();

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
    }
}