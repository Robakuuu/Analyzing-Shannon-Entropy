using System.Xml.Linq;
using Shannon.Entropy;

namespace Shannon
{
    public class Ncbi
    {
        private string _response = "";
        XDocument _doc = new XDocument();
        public List<Ncbi.INSDSet> Sets = new List<Ncbi.INSDSet>();
        public string URL = "";
        public string FULLNAME = "";
        public Ncbi(string accesion,string fullname)
        {
            FULLNAME = fullname;
            URL = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=nuccore&id=" + accesion + "&rettype=gbc&retmode=xml";
        }

        public async Task Start()
        {
            try
            {
                await Fetch();
                await Parse();
                await Get();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Couldn't fetch data from NCBI");
            }


        }
        public class INSDQualifier
        {
            public string Name;
            public string Value;
        }
        public class INSDInterval
        {
            public string From;
            public string To;
        }
        public class INSDFeatures
        {
            public string INSDFeatureKey;
            public string INSDFeatureLocation;
            public List<Ncbi.INSDInterval> Intervals = new List<Ncbi.INSDInterval>();
            public List<Ncbi.INSDQualifier> Qualifiers = new List<Ncbi.INSDQualifier>();
        }
        public class INSDSet
        {
            public string AccesionID = "";
            public List<Ncbi.INSDFeatures> Features = new List<Ncbi.INSDFeatures>();


        }

        public async Task<string> FetchURL(string url)
        {
            HttpClient httpClient = new HttpClient();
            string response = await httpClient.GetStringAsync(new Uri(url));
            return response;
        }


        public async Task Fetch()
        {
            _response = await FetchURL(URL);
        }
        public async Task Parse()
        {
            _doc = XDocument.Parse(_response);
        }
        public async Task Get()
        {
            var nodes = _doc.Elements();
            Ncbi.INSDFeatures newFeatures = new Ncbi.INSDFeatures();
            Ncbi.INSDInterval newInterval = new Ncbi.INSDInterval();
            Ncbi.INSDQualifier newQualifier = new Ncbi.INSDQualifier();
            Ncbi.INSDSet newSet = new Ncbi.INSDSet();
            foreach (var insdset in nodes)
            {
                foreach (var insdseq in insdset.Elements())
                {
                    foreach (var child in insdseq.Elements())
                    {
                        if (child.Name == "INSDSeq_accession-version")
                        {
                            newSet.AccesionID = child.Value;


                        }
                        if (child.Name == "INSDSeq_feature-table")
                        {
                            foreach (var feauters in child.Elements())
                            {
                                foreach (var feauter in feauters.Elements())
                                {

                                    if (feauter.Name == "INSDFeature_key")
                                    {
                                        newFeatures.INSDFeatureKey = feauter.Value;
                                    }
                                    if (feauter.Name == "INSDFeature_location")
                                    {
                                        newFeatures.INSDFeatureLocation = feauter.Value;
                                    }
                                    if (feauter.Name == "INSDFeature_intervals")
                                    {
                                        foreach (var interval in feauter.Elements())
                                        {
                                            foreach (var childinterval in interval.Elements())
                                            {
                                                if (childinterval.Name == "INSDInterval_from")
                                                {
                                                    newInterval.From = childinterval.Value;
                                                }
                                                if (childinterval.Name == "INSDInterval_to")
                                                {
                                                    newInterval.To = childinterval.Value;
                                                    newFeatures.Intervals.Add(newInterval);
                                                    newInterval = new Ncbi.INSDInterval();
                                                }
                                            }
                                        }

                                    }
                                    if (feauter.Name == "INSDFeature_quals")
                                    {
                                        foreach (var quals in feauter.Elements())
                                        {
                                           
                                            foreach (var childqual in quals.Elements())
                                            {
                                                if (childqual.Name == "INSDQualifier_name")
                                                {
                                                    newQualifier.Name = childqual.Value;
                                                }
                                                if (childqual.Name == "INSDQualifier_value")
                                                {
                                                    newQualifier.Value = childqual.Value;
                                                    newFeatures.Qualifiers.Add(newQualifier);
                                                    newQualifier = new Ncbi.INSDQualifier();
                                                }
                                            }
                                        }
                                    }
                                }
                                newSet.Features.Add(newFeatures);
                                newFeatures = new Ncbi.INSDFeatures();
                            }

                        }


                    }


                }
                Sets.Add(newSet);
            }
        }
    }
}
