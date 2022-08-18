using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static MudBlazor.CategoryTypes;

namespace ShannonEntropyCal
{
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
            var alphabetX = message.Distinct().ToList();
            var alphabetY = message2.Distinct().ToList();
            double HXIY = 0;
            var alphabet = alphabetX.Union(alphabetY);

          

            foreach (var firstSign in alphabet)
            {
                foreach (var secondSign in alphabet)
                {
                   
                        double count = 0;
                        for (int index = 0; index < message.Length; index++)
                        {

                            if (message[index] == firstSign && message2[index] == secondSign )
                            {
                                count++;
                            }
                        }

                        if (count != 0)
                        {
                            var tmp = 0.0;
                            double howManyTimesXY = 0;
                            for (int index = 0; index < message2.Length; index++)
                            {

                                if (message2[index] == secondSign)
                                {
                                    howManyTimesXY++;
                                }
                            }

                            if (howManyTimesXY > 0)
                            {
                                tmp = count / howManyTimesXY;
                                HXIY -= (count / (double)message.Length) * Math.Log(tmp, 2);
                            }

                        }

                    
                }
            }

            return HXIY;

        }
        public double ConditionalEntropyValue2(string message, string message2)
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