using Bio;
using Bio.Algorithms.Alignment;
using Bio.Extensions;

namespace Shannon
{
    public record AlignResult
    {
        public string FirstSequence;
        public string SecondSequence;
    }
    public class AlignerNW
    {
        public AlignResult AlignSequencesNW(string seq1, string seq2)
        {
            var alt = new Bio.Algorithms.Alignment.NucmerPairwiseAligner();

            var referenceSeqs = new List<ISequence>
            {
                new Bio.Sequence(Alphabets.DNA, seq1){ID="R1"},
                new Bio.Sequence(Alphabets.DNA, seq2){ID="Q1"}
            };

            AlignResult resultOfAlignment = new AlignResult();
            NeedlemanWunschAligner nw = new NeedlemanWunschAligner();
            var result = nw.AlignSimple(referenceSeqs);

            if (result.Count > 0)
            {
                var check1 = result[0].AlignedSequences;
                if (check1.Count > 0)
                {
                    var sequences = check1[0];
                    var resForFirst = sequences.Sequences.First();
                    var resForSecond = sequences.Sequences.Last();

                    resultOfAlignment.FirstSequence= resForFirst.GetComplementedSequence().ConvertToString();
                    resultOfAlignment.SecondSequence= resForSecond.GetComplementedSequence().ConvertToString();
                }
            }

            return resultOfAlignment;
        }
    }
}
