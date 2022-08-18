using System.Collections.Concurrent;

namespace ShannonEntropyCal;

public class MutualInformationCal
{

    public double CalculateMI(string seq1, string seq2) //most right way
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
    
}