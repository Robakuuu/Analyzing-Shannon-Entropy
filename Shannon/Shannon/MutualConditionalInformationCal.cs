using System.Collections.Concurrent;

namespace ShannonEntropyCal;

public class MutualConditionalInformationCal
{
    public double MutualConditionaInformationNEW(string seq1, string seq2, string seq3)
    {
        //I(X;Y|Z)=H(Z|X) +H(X)+H(Z|Y)+H(Y) -H(Z|X,Y)-H(X,Y)-H(Z)
        //        =I(X;Y) +H(Z|X) +H(Z|Y) - H(Z|XY) -H(Z)
        //        
        //          H(Z|X) +H(X)+H(Z|Y)+H(Y) -H(Z|X,Y)-H(X,Y)-H(Z)  = I(X;Y) +H(Z|X) +H(Z|Y) - H(Z|X,Y) -H(Z)
        //              H(Z|X) +H(X)+H(Z|Y)+H(Y) -H(X,Y)-H(Z) = I(X;Y) +H(Z|X) +H(Z|Y) -H(Z)
        //              H(Z|X) +H(X)+H(Z|Y)+H(Y) -H(X,Y) =I(X;Y) +H(Z|X) +H(Z|Y)
        var alphabetX = seq1.Distinct().ToList();
        var alphabetY = seq2.Distinct().ToList();
        var alphabetZ = seq3.Distinct().ToList();

        var alphabet = alphabetX.Union(alphabetY).Union(alphabetZ).ToList();

        var HZIXY = 0.0;

        foreach (var firstSign in alphabet)
        {
            foreach (var secondSign in alphabet)
            {
                foreach (var thirdSign in alphabet)
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
                        var tmp = 0.0;
                        double howManyTimesXY = 0;
                        for (int index = 0; index < seq1.Length; index++)
                        {

                            if (seq1[index] == firstSign && seq2[index] == secondSign)
                            {
                                howManyTimesXY++;
                            }
                        }

                        if (howManyTimesXY > 0)
                        {
                            tmp = count / howManyTimesXY;
                            HZIXY -= (count / (double)seq1.Length) * Math.Log(tmp, 2);
                        }
                       
                    }

                }
            }
        }
        double HXYZ = 0.0;
        double HXZ = 0.0;
        double HYZ = 0.0;
        double HZ = 0.0;
        double HY = 0.0;
        double HX = 0.0;

        var mi = new MutualInformationCal();
        var entropy = new EntropyCal();

        HZ = Truncate(entropy.EntropyValue(seq3));
        HY =Truncate(entropy.EntropyValue(seq2));
        HX = Truncate(entropy.EntropyValue(seq1));
        HYZ = Truncate(((-1)*mi.CalculateMI(seq2, seq3)) + HY + HZ);
        HXZ =Truncate(((-1)*mi.CalculateMI(seq1, seq3)) + HX + HZ);
        HXYZ = Truncate(HX + entropy.ConditionalEntropyValue(seq2, seq1) + HZIXY);
        double result = 0.0;
        result = Truncate(HXZ) + Truncate(HYZ) - Truncate(HXYZ) - Truncate(HZ);
        var resultTest = Truncate(mi.CalculateMI(seq1, seq2)) + Truncate(entropy.ConditionalEntropyValue(seq3, seq1)) +
                         Truncate(entropy.ConditionalEntropyValue(seq3, seq2))
                         - HZIXY - HZ;

        var retunValue = (result + resultTest) / 2;
        retunValue= Math.Truncate(retunValue * 100)/ 100;
        return Truncate(retunValue);
    }

    public double Truncate(double input)
    {
        var tmp = Math.Truncate(input * 1000)/ 1000;
        return tmp;
    }
    public double MutualConditionaInformation3(string seq1, string seq2, string seq3)
    {
        var alphabetX = seq1.Distinct().ToList();
        var alphabetY = seq2.Distinct().ToList();
        var alphabetZ = seq3.Distinct().ToList();
        ConcurrentDictionary<char, double> probabiltyForX = new ConcurrentDictionary<char, double>();
        foreach (var sign in alphabetX)
        {
            double count = 0;
            foreach (var x in seq1)
            {
                if (x == sign) count++;
            }

            if (count != 0)
            {
                probabiltyForX.TryAdd(sign, count / seq1.Length);
            }
        }
        ConcurrentDictionary<char, double> probabiltyForY = new ConcurrentDictionary<char, double>();
        foreach (var sign in alphabetY)
        {
            double count = 0;
            foreach (var x in seq2)
            {
                if (x == sign) count++;
            }

            if (count != 0)
            {
                probabiltyForY.TryAdd(sign, count / seq2.Length);
            }
        }
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
                    probabiltyForXwithZ.TryAdd(new PairedEvent() { FirstSign = firstSign, SecondSign = secondSign },
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
                    probabiltyForYwithZ.TryAdd(new PairedEvent() { FirstSign = firstSign, SecondSign = secondSign },
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
                            count / (double)seq1.Length);
                    }

                }
            }
        }

        double HXYZ = 0.0;
        double HXZ = 0.0;
        double HYZ = 0.0;
        double HZ = 0.0;
        double HY = 0.0;
        double HX = 0.0;


        // ConcurrentDictionary<char, double> tmpZ = new ConcurrentDictionary<char, double>();
        // ConcurrentDictionary<PairedEvent, double> tmpXwithZ = new ConcurrentDictionary<PairedEvent, double>();
        // ConcurrentDictionary<PairedEvent, double> tmpYwithZ = new ConcurrentDictionary<PairedEvent, double>();

        foreach (var triplets in probabiltyForXwithYwithZ)
        {
            //var tmp = Math.Truncate(triplets.Value * 1000)/ 1000;
            var tmp = triplets.Value;
            HXYZ-=tmp * Math.Log(tmp, 2);
            //  tmpZ.TryAdd(triplets.Key.ThirdSign, probabiltyForZ[triplets.Key.ThirdSign]);
            //  tmpXwithZ.TryAdd(new PairedEvent() {FirstSign = triplets.Key.FirstSign,SecondSign = triplets.Key.ThirdSign }, probabiltyForXwithZ[new PairedEvent() { FirstSign = triplets.Key.FirstSign, SecondSign = triplets.Key.ThirdSign }]);
            //  tmpYwithZ.TryAdd(new PairedEvent() { FirstSign = triplets.Key.SecondSign,SecondSign = triplets.Key.ThirdSign}, probabiltyForXwithZ[new PairedEvent() { FirstSign = triplets.Key.SecondSign, SecondSign = triplets.Key.ThirdSign }]);
        }
    

        var mi = new MutualInformationCal();
        var entropy = new EntropyCal();

        HZ = entropy.EntropyValue(seq3);
        HY = entropy.EntropyValue(seq2);
        HX = entropy.EntropyValue(seq1);
        HYZ = ((-1)*mi.CalculateMI(seq2, seq3)) + HY + HZ;
        HXZ =((-1)*mi.CalculateMI(seq1, seq3)) + HX + HZ;
        
        double result = 0.0;
        result = HXZ + HYZ - HXYZ - HZ;

        return result;

    }
    public double MutualConditionaInformation2(string seq1, string seq2, string seq3)
    {
        var alphabetX = seq1.Distinct().ToList();
        var alphabetY = seq2.Distinct().ToList();
        var alphabetZ = seq3.Distinct().ToList();

        ConcurrentDictionary<char, double> probabiltyForX = new ConcurrentDictionary<char, double>();
        foreach (var sign in alphabetX)
        {
            double count = 0;
            foreach (var x in seq1)
            {
                if (x == sign) count++;
            }

            if (count != 0)
            {
                probabiltyForX.TryAdd(sign, count / seq1.Length);
            }
        }
        ConcurrentDictionary<char, double> probabiltyForY = new ConcurrentDictionary<char, double>();
        foreach (var sign in alphabetY)
        {
            double count = 0;
            foreach (var x in seq2)
            {
                if (x == sign) count++;
            }

            if (count != 0)
            {
                probabiltyForY.TryAdd(sign, count / seq2.Length);
            }
        }
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