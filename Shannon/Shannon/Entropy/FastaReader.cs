using System.Collections.ObjectModel;
using Blazorise.Extensions;
using Microsoft.VisualBasic;
using Xyaneon.Bioinformatics.FASTA;
using Xyaneon.Bioinformatics.FASTA.IO;


namespace Shannon.Entropy
{
    public class SequenceEntropy
    {
        public List<double> Score { get; set; }
        public string Name { get; set; }

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
        public void ReadMultipleFromString(string content)
        {

            var t = content.Length;
           // content = content.Replace(" ", "");
            var a = content.Length;
            var tmp = content.Replace("\r","").Split("\n").ToList();

            Sequences = Sequence.ParseMultiple(tmp);
        }

    }
}
