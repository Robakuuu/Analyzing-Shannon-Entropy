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
    public double CalculateConditionalEntropy(string seq1, string seq2) //most right way
    {
        if (seq1 != seq2)
        {
            EntropyCal entorpy = new EntropyCal();
            var HXIY = entorpy.ConditionalEntropyValue(seq1, seq2);
            return HXIY;
        }
        return 0.0;
    }
    
}