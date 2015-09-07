using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sunshine.BizInterface
{
    public class SunshineRandom : Random
    {
        private static readonly RNGCryptoServiceProvider _global = new RNGCryptoServiceProvider();
        private Random _local;
        public SunshineRandom()
        {
            byte[] buffer = new byte[4];

            _global.GetBytes(buffer);
            _local = new Random(BitConverter.ToInt32(buffer, 0));
        }

        public override int Next()
        {
            return _local.Next();
        }

        /// <summary>Returns a nonnegative random number less than the specified maximum.</summary>
        /// <param name="maxValue">
        /// The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero. 
        /// </param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to zero, and less than maxValue; 
        /// that is, the range of return values ordinarily includes zero but not maxValue. However, 
        /// if maxValue equals zero, maxValue is returned.
        /// </returns>
        public override int Next(int maxValue)
        {
            return _local.Next(maxValue);
        }

        /// <summary>Returns a random number within a specified range.</summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to minValue and less than maxValue; 
        /// that is, the range of return values includes minValue but not maxValue. 
        /// If minValue equals maxValue, minValue is returned.
        /// </returns>
        public override int Next(int minValue, int maxValue)
        {
            return _local.Next(minValue, maxValue);
        }

        /// <summary>Returns a random integer percentage number within the range (0-100).</summary>
        /// <returns>
        /// A 32-bit signed integer between 0 and 100
        /// </returns>
        public int NextPercentage()
        {
            return _local.Next(101);
        }

        /// <summary>Returns a random number between 0.0 and 1.0.</summary>
        /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
        public override double NextDouble()
        {
            double t = _local.NextDouble();

            while ((t < 0D) && (t >= 1D))
                t = _local.NextDouble();

            return t;
        }

        public double NextDouble(double stdDev, double mean)
        {
            double t = _local.NextDouble();

            while ((t < 0D) && (t >= 1D))
                t = _local.NextDouble();

            t *= stdDev;
            t += mean;

            return t;
        }

        public double NextDouble(double stdDev)
        {
            double t = _local.NextDouble();

            while ((t < 0D) && (t >= 1D))
                t = _local.NextDouble();

            t *= 2D;
            t -= 1D;
            t *= stdDev;

            return t;
        }

        /// <summary>Fills the elements of a specified array of bytes with random numbers.</summary>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        public override void NextBytes(byte[] buffer)
        {
            _local.NextBytes(buffer);
        }
    }
}
