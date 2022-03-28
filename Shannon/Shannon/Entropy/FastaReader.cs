using Xyaneon.Bioinformatics.FASTA;
using Xyaneon.Bioinformatics.FASTA.IO;


namespace Shannon.Entropy
{
    public class SequenceEntropy
    {
        public List<double> Score { get; set; }

        public SequenceEntropy()
        {
            Score = new List<double>();
        }
    }
    public class FastaReader
    {
        public FastaReader()
        {
            Sequences = new List<Sequence>();
            EntropySequences = new List<SequenceEntropy>();
        }

        public IEnumerable<Sequence> Sequences { get; set; }
        public List<SequenceEntropy> EntropySequences { get; set; }
        public void ReadFile(string path)
        {
            Sequences = SequenceFileReader.ReadMultipleFromFile(path);
        }

    }
}
