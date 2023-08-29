﻿namespace ThSpellCardRecordViewer.Score
{
    internal class Calculator
    {
        public static string CalcSpellCardGetRate(double get, double challenge)
        {
            if (challenge != 0)
            {
                double rate = get / challenge * 100;
                return $"{rate:F2}%";
            }
            else
            {
                return "-.--%";
            }
        }
    }
}
