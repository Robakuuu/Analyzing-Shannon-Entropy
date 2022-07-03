using System;
using System.Collections.Generic;
using System.Linq;

namespace ShannonEntropyCal
{

    public class MutualInformationCal
    {
        public double CalculateMI(string seq1, string seq2)
        {
            List<string> pairs = new List<string>();
            List<double> results = new List<double>();
            if (seq1.Length == seq2.Length)
            {
                for (int index = 0; index<seq1.Length; index++)
                {
                    string pair = seq1[index].ToString() + seq2[index].ToString();
                    string invertPair = seq2[index].ToString() + seq1[index].ToString();
                    if (!pairs.Contains(pair) && !pairs.Contains(invertPair))
                    {
                        
                       double XfrompXY = 0.0;
                       if ((double)seq1.Count(c => c == seq1[index]) > (double)seq2.Count(c => c == seq1[index]))
                       {
                           XfrompXY = ((double)seq1.Count(c => c == seq1[index]));
                        }
                       else
                       {
                           XfrompXY = ((double)seq2.Count(c => c == seq1[index]));
                        }

                       double countMatches = 0;
                       for (int indexForPairSearch = 0; indexForPairSearch < seq2.Length; indexForPairSearch++)
                       {
                            if(seq1[indexForPairSearch].ToString()+seq2[indexForPairSearch].ToString() == pair)
                                countMatches++;
                       }

                       if (countMatches != XfrompXY)
                       {
                           double pXY = Math.Truncate(countMatches / XfrompXY * 1000) / 1000;

                           double pX = Math.Truncate((double)seq1.Count(c => c == seq1[index]) / seq1.Length * 1000) /
                                       1000;
                           double pY = Math.Truncate((double)seq2.Count(c => c == seq2[index]) / seq2.Length * 1000) /
                                       1000;

                           double result = pXY * Math.Log((pXY / (pX * pY)), 2);
                           results.Add(result);
                       }
                       else
                       {
                           results.Add(0.0);
                       }
                       pairs.Add(pair);
                    }
                    //0.6931
                    //

                }
            }

            return results.Sum();

        }
    }
    public class EntropyCal
    {
        /// <summary>
        /// Gets the entropy value of a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public double EntropyValue(string message)
        {
            Dictionary<char, int> K = message.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            double EntropyValue = 0;
            foreach (var character in K)
            {
                double PR = character.Value / (double)message.Length;
                EntropyValue -= PR * Math.Log(PR, 2);
            }
            return EntropyValue;
        }

        /// <summary>
        /// Calculates the Entropy Bit value of a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public double EntropyBits(string message)
        {
            Dictionary<char, int> K = message.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            double EntropyValue = 0;
            foreach (var character in K)
            {
                double PR = character.Value / (double)message.Length;
                EntropyValue -= PR * Math.Log(PR, 2);
            }
            return Math.Ceiling(EntropyValue) * message.Length;
        }
    }
}