using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATG_Notifier.ViewModels.Utilities
{
    public static class MathUtility
    {
        private static readonly Random random = new Random();

        public static string GetRandomHexNumber(int digits)
        {
            if (digits <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(digits));
            }

            var buffer = new byte[digits / 2];

            random.NextBytes(buffer);

            var result = string.Concat(buffer.Select(x => x.ToString("X2")).ToArray());

            return digits % 2 == 0
                ? result
                : result + random.Next(16).ToString("X");
        }
    }
}
