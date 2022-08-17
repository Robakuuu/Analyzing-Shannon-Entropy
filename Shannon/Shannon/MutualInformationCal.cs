using System.Collections.Concurrent;

namespace ShannonEntropyCal;

public class MutualInformationCal
{

    public double CalculateMI2(string seq1, string seq2) //most right way
    {
        if (seq1 != seq2)
        {
            EntropyCal entorpy = new EntropyCal();
            var HX = entorpy.EntropyValue(seq1);
            var HXIY = entorpy.ConditionalEntropyValue(seq1, seq2);
            var res = HX - HXIY;
            return res;
        }
        return 0.0;
    }
    public double CalculateAverageMI(string seq1, string seq2) //most right way
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