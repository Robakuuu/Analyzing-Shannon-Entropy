using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static MudBlazor.CategoryTypes;

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
        public double MutualConditionaInformation2(string seq1, string seq2, string seq3)
        {
            var alphabetX = seq1.Distinct().ToList();
            var alphabetY = seq2.Distinct().ToList();
            var alphabetZ = seq3.Distinct().ToList();

            ConcurrentDictionary<char, double> probabiltyForZ = new ConcurrentDictionary<char, double>();
            foreach (var sign in alphabetZ)
            {
                double count = 0;
                foreach (var x in seq3)
                {
                    if (x == sign) count++;
                }
           
                if (count != 0)
                {
                    probabiltyForZ.TryAdd(sign, count / seq3.Length);
                }
            }
           
            ConcurrentDictionary<PairedEvent, double> probabiltyForXwithZ = new ConcurrentDictionary<PairedEvent, double>();
            foreach (var firstSign in alphabetX)
            {
                double count = 0;
                foreach (var secondSign in alphabetZ)
                {
                    for (int index = 0; index < seq1.Length; index++)
                    {
                        if (seq1[index] == firstSign && seq3[index]==secondSign) count++;
                    }
           
                    if (count != 0)
                    {
                        probabiltyForXwithZ.TryAdd(new PairedEvent() {FirstSign = firstSign, SecondSign = secondSign},
                            count / seq1.Length);
                    }
                }
            }
            ConcurrentDictionary<PairedEvent, double> probabiltyForYwithZ = new ConcurrentDictionary<PairedEvent, double>();
            foreach (var firstSign in alphabetY)
            {
                double count = 0;
                foreach (var secondSign in alphabetZ)
                {
                    for (int index = 0; index < seq1.Length; index++)
                    {
                        if (seq1[index] == firstSign && seq3[index]==secondSign) count++;
                    }
           
                    if (count != 0)
                    {
                        probabiltyForYwithZ.TryAdd(new PairedEvent() {FirstSign = firstSign, SecondSign = secondSign},
                            count / seq1.Length);
                    }
                }
            }
            ConcurrentDictionary<TripledEvent, double> probabiltyForXwithYwithZ = new ConcurrentDictionary<TripledEvent, double>();
            foreach (var firstSign in alphabetX)
            {

                foreach (var secondSign in alphabetY)
                {
                    foreach (var thirdSign in alphabetZ)
                    {
                        double count = 0;
                        for (int index = 0; index < seq1.Length; index++)
                        {

                            if (seq1[index] == firstSign && seq2[index] == secondSign && seq3[index] == thirdSign)
                            {
                                count++;
                            }
                        }

                        if (count != 0)
                        {
                            probabiltyForXwithYwithZ.TryAdd(
                                new TripledEvent()
                                    { FirstSign = firstSign, SecondSign = secondSign, ThirdSign = thirdSign },
                                count / seq1.Length);
                        }
                        
                    }
                }
            }

            double HXYZ = 0.0;
            double HXZ = 0.0;
            double HYZ = 0.0;
            double HZ = 0.0;


           // ConcurrentDictionary<char, double> tmpZ = new ConcurrentDictionary<char, double>();
           // ConcurrentDictionary<PairedEvent, double> tmpXwithZ = new ConcurrentDictionary<PairedEvent, double>();
           // ConcurrentDictionary<PairedEvent, double> tmpYwithZ = new ConcurrentDictionary<PairedEvent, double>();

            foreach (var triplets in probabiltyForXwithYwithZ)
            {
                var tmp = Math.Truncate(triplets.Value * 1000)/ 1000;
                HXYZ-=tmp * Math.Log(tmp, 2);
              //  tmpZ.TryAdd(triplets.Key.ThirdSign, probabiltyForZ[triplets.Key.ThirdSign]);
              //  tmpXwithZ.TryAdd(new PairedEvent() {FirstSign = triplets.Key.FirstSign,SecondSign = triplets.Key.ThirdSign }, probabiltyForXwithZ[new PairedEvent() { FirstSign = triplets.Key.FirstSign, SecondSign = triplets.Key.ThirdSign }]);
              //  tmpYwithZ.TryAdd(new PairedEvent() { FirstSign = triplets.Key.SecondSign,SecondSign = triplets.Key.ThirdSign}, probabiltyForXwithZ[new PairedEvent() { FirstSign = triplets.Key.SecondSign, SecondSign = triplets.Key.ThirdSign }]);
            }
            foreach (var pairs in probabiltyForXwithZ)
            {
                var tmp = Math.Truncate(pairs.Value * 1000)/ 1000;
                HXZ-=tmp * Math.Log(tmp, 2);
            }
            foreach (var pairs in probabiltyForYwithZ)
            {
                var tmp = Math.Truncate(pairs.Value * 1000)/ 1000;
                HYZ-=tmp * Math.Log(tmp, 2);
            }
            foreach (var unit in probabiltyForZ)
            {
                var tmp = Math.Truncate(unit.Value * 1000)/ 1000;
                HZ-=tmp * Math.Log(tmp, 2);
            }

            double result = 0.0;
            result = HXZ + HYZ - HXYZ - HZ;

            return result;

        }


    }
public class MutualInformationCal
    {

        public double CalculateMI2(string seq1, string seq2) //most right way
        {
            if (seq1 != seq2)
            {
                EntropyCal entorpy = new EntropyCal();
                var HX = entorpy.EntropyValue(seq1);
                var HY = entorpy.EntropyValue(seq2);
                var HXIY = entorpy.ConditionalEntropyValue(seq1, seq2);
                var HYIX = entorpy.ConditionalEntropyValue(seq2, seq1);
                var tmp1 = HX - HXIY;
                var tmp2 = HY - HYIX;
                var res = (tmp1 + tmp2) /2.0;
                return res;
            }
            return 0.0;
        }
        public double CalculateMI3(string seq1, string seq2)
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
        ///

        public class MatrixUnit
        {
            public char Sign { get; set; }
            public double Value { get; set; }
        }

        public double ConditionalEntropyValue(string message, string message2)
        {
            var alphabetA = message.Distinct().ToList();
            var alphabetB = message2.Distinct().ToList();
            double EntropyValue = 0;
            ConcurrentDictionary<char,List<MatrixUnit>> matrix = new ConcurrentDictionary<char,List<MatrixUnit>>();
            foreach (var columns in alphabetA)
            {
                var list = new List<MatrixUnit>();
                foreach (var rows in alphabetB)
                {
                    list.Add(new MatrixUnit(){Sign=rows});
                }
                matrix.TryAdd(columns, list);
            }

            foreach (var keyA in matrix.Keys)
            {
                foreach (var unit in matrix[keyA])
                {
                    int count = 0;
                    for (int index = 0; index < message.Length; index++)
                    {
                        if (message[index] == keyA && message2[index] == unit.Sign)
                        {
                            count++;
                        }
                    }

                    unit.Value = count / (double) message.Length;

                }
            }

            List<double> sums = new List<double>();
            foreach (var keyA in matrix.Keys)
            {
                double sum = 0.0;
                foreach (var unit in matrix[keyA])
                {
                    sum += unit.Value;
                }
                sums.Add(sum);
            }
            ConcurrentDictionary<char, List<MatrixUnit>> matrixWithSums = new ConcurrentDictionary<char, List<MatrixUnit>>();
            foreach (var keyA in matrix.Keys)
            {
                matrixWithSums.TryAdd(keyA, new List<MatrixUnit>());
                foreach (var unit in matrix[keyA])
                {
                    matrixWithSums[keyA].Add(new MatrixUnit(){Sign = unit.Sign, Value = unit.Value});

                }
            }
          
            int indexForSums = 0;
            foreach (var keyA in matrixWithSums.Keys)
            {
                
                foreach (var unit in matrixWithSums[keyA])
                {
                    unit.Value = unit.Value / sums[indexForSums];
                }

                indexForSums++;
            }

            var HXIY = 0.0;

            List<double> calculations = new List<double>();
            foreach (var keyA in matrixWithSums.Keys)
            {
                foreach (var unit in matrixWithSums[keyA])
                {
                    calculations.Add(unit.Value);
                }
            }

            var result = 0.0;
            int counter = 0;
            foreach (var keyA in matrix.Keys)
            {
                foreach (var unit in matrix[keyA])
                {
                    if (calculations[counter] != 0)
                    {
                        result += unit.Value * Math.Log(calculations[counter], 2);
                        
                    }
                    counter++;
                }
            }
            return result*(-1);
        }
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