using Bio;
using Bio.Algorithms.Alignment;
using FluentAssertions;

namespace Shannon.Tests;

public class AlingmentTests
{

    [Fact]
    public void CheckingNeedlemanWunschAligner()
    {
        
        Sequence seq = new Sequence( DnaAlphabet.Instance, "ATTCAAA");
        Sequence seq1 = new Sequence( DnaAlphabet.Instance, "ATT");
        var na = new NeedlemanWunschAligner();

        var result =  na.Align(seq, seq1);

        foreach (var res in result.First().PairwiseAlignedSequences)
        {
            res.SecondSequence.Should().BeEquivalentTo(new Sequence(DnaAlphabet.Instance, "ATT----"));
        }
    }
}