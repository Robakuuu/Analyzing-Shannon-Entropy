using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ShannonEntropyCal
{
    public record PairedEvent
    {
        public Char FirstSign { get; set; }
        public Char SecondSign { get; set; }
    }
    public record TripledEvent
    {
        public Char FirstSign { get; set; }
        public Char SecondSign { get; set; }
        public Char ThirdSign { get; set; }
    }

    public class MutualConditionalInformationCal
    {
        public double MutualConditionaInformation(string seq1, string seq2)
        {
            var alphabetA = seq1.Distinct().ToList();
            var alphabetC = alphabetA;
            var alphabetB = seq2.Distinct().ToList();

            ConcurrentDictionary<char, double> probabiltyForA = new ConcurrentDictionary<char, double>();
            foreach (var sign in alphabetA)
            {
                double count = 0;
                foreach (var x in seq1)
                {
                    if (x == sign) count++;
                }

                probabiltyForA.TryAdd(sign, count / seq1.Length);
            }

            alphabetC = alphabetA;
            ConcurrentDictionary<char, double> probabiltyForC = probabiltyForA;

            ConcurrentDictionary<char, double> probabiltyForB = new ConcurrentDictionary<char, double>();
            foreach (var sign in alphabetB)
            {
                double count = 0;
                foreach (var x in seq2)
                {
                    if (x == sign) count++;
                }

                probabiltyForB.TryAdd(sign, count / seq2.Length);
            }
           
            ConcurrentDictionary<PairedEvent, double> probabiltyForAwithC = new ConcurrentDictionary<PairedEvent, double>();
            foreach (var firstSign in alphabetA)
            {
                double count = 0;
                foreach (var secondSign in alphabetC)
                {
                    for (int index = 0; index < seq1.Length; index++)
                    {
                        if (seq1[index] == firstSign && seq2[index]==secondSign) count++;
                    }
                    probabiltyForAwithC.TryAdd(new PairedEvent() { FirstSign = firstSign, SecondSign = secondSign }, count / seq1.Length);
                }
            }
            ConcurrentDictionary<PairedEvent, double> probabiltyForBwithC = new ConcurrentDictionary<PairedEvent, double>();
            foreach (var firstSign in alphabetB)
            {
                double count = 0;
                foreach (var secondSign in alphabetC)
                {
                    for (int index = 0; index < seq1.Length; index++)
                    {
                        if (seq1[index] == firstSign && seq2[index]==secondSign) count++;
                    }
                    probabiltyForBwithC.TryAdd(new PairedEvent() { FirstSign = firstSign, SecondSign = secondSign }, count / seq1.Length);
                }
            }
            ConcurrentDictionary<TripledEvent, double> probabiltyForAwithBwithC = new ConcurrentDictionary<TripledEvent, double>();
            foreach (var firstSign in alphabetA)
            {
               
                foreach (var secondSign in alphabetB)
                {
                    foreach (var thirdSign in alphabetC)
                    {
                        double count = 0;
                        for (int index = 0; index < seq1.Length; index++)
                        {

                            if (seq1[index] == firstSign && seq2[index] == secondSign &&
                                (seq1[index] == thirdSign || seq2[index] == thirdSign))
                            {
                                count++;
                            }
                        }
                        probabiltyForAwithBwithC.TryAdd(
                            new TripledEvent()
                                { FirstSign = firstSign, SecondSign = secondSign, ThirdSign = thirdSign },
                            count / seq1.Length);
                    }
                }
            }

            double sum = 0.0;
            foreach (var triplets in probabiltyForAwithBwithC)
            {
                double pZ = Math.Truncate(probabiltyForC[triplets.Key.ThirdSign] * 1000) /
                            1000;
                double pXYZ = Math.Truncate(triplets.Value * 1000) / 1000;
                double pXZ = Math.Truncate(probabiltyForAwithC[new PairedEvent(){FirstSign = triplets.Key.FirstSign ,SecondSign = triplets.Key.ThirdSign}] * 1000) /
                            1000;
                double pYZ = Math.Truncate(probabiltyForBwithC[new PairedEvent() { FirstSign = triplets.Key.SecondSign, SecondSign = triplets.Key.ThirdSign }] * 1000) /
                            1000;
                if (pZ != 0 && pXZ != 0 && pYZ != 0 && pXYZ!=0)
                {
                    double result = pXYZ * Math.Log(((pZ*pXYZ) / (pXZ * pYZ)), 2);

                    sum += result;
                }
            }

            return sum;

        }
    }
public class MutualInformationCal
    {
        
        public double CalculateMI2(string seq1, string seq2)
        {
            var alphabetA = seq1.Distinct().ToList();
            var alphabetB = seq2.Distinct().ToList();

            ConcurrentDictionary<char, double> probabiltyForA = new ConcurrentDictionary<char, double>();
            foreach (var sign in alphabetA)
            {
                double count = 0;
                foreach (var x in seq1)
                {
                    if (x == sign) count++;
                }

                probabiltyForA.TryAdd(sign, count / seq1.Length);
            }

            ConcurrentDictionary<char, double> probabiltyForB = new ConcurrentDictionary<char, double>();
            foreach (var sign in alphabetB)
            {
                double count = 0;
                foreach (var x in seq2)
                {
                    if (x == sign) count++;
                }

                probabiltyForB.TryAdd(sign, count / seq2.Length);
            }

            ConcurrentDictionary<PairedEvent, double> probabiltyForAwithB = new ConcurrentDictionary<PairedEvent, double>();
            foreach (var firstSign in alphabetA)
            {
                double count = 0;
                foreach (var secondSign in alphabetB)
                {
                    for (int index = 0; index < seq1.Length; index++)
                    {
                        if(seq1[index] == firstSign && seq2[index]==secondSign) count++;
                    }
                    probabiltyForAwithB.TryAdd(new PairedEvent(){ FirstSign = firstSign,SecondSign = secondSign}, count / seq1.Length);
                }
            }



            double sum = 0.0;

            foreach (var pairs in probabiltyForAwithB)
            {
                double pXY = Math.Truncate(pairs.Value * 1000) / 1000;
                double pX = Math.Truncate(probabiltyForA[pairs.Key.FirstSign] * 1000) /
                            1000;
                double pY = Math.Truncate(probabiltyForB[pairs.Key.SecondSign] * 1000) /
                            1000;
                if (pY != 0 && pX != 0 && pXY != 0)
                {
                    double result = pXY * Math.Log((pXY / (pX * pY)), 2);

                    sum += result;
                }
               
            }

            return sum;

        }

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