using Xyaneon.Bioinformatics.FASTA;
using Xyaneon.Bioinformatics.FASTA.IO;
using Standard.Data.StringMetrics;

namespace Shannon.Entropy
{
    public class FastaReader
    {
        public FastaReader()
        {
            Sequences = new List<Sequence>();
        }

        public IEnumerable<Sequence> Sequences { get; set; }

        public void ReadFile(string path)
        {
            Sequences = SequenceFileReader.ReadMultipleFromFile(path);
        }

        public void Test(string firstseq, string secondseq)
        {
            NeedlemanWunch nw= new NeedlemanWunch();
            nw.GetSimilarityExplained(firstseq, secondseq);
        }
    }
}
