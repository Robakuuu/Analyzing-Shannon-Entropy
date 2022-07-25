using FluentAssertions;
using ShannonEntropyCal;

namespace Shannon.Tests
{
    public class CalucaltingTests
    {
        [Fact]
        public void ShannonEntropyBasic()
        {
            EntropyCal sut = new EntropyCal();

            var result= sut.EntropyValue("AAAA");
            result.Should().Be(0.00);

            var result2 = sut.EntropyValue("ATCG");
            result2.Should().Be(2.00);

            var result3 = sut.EntropyValue("AGGT");
            result3.Should().Be(1.50);

            var result4 = sut.EntropyValue("");
            result4.Should().Be(0.00);

            var result5 = sut.EntropyValue("ATTTACAGATTAGAGACA");
            result5.Should().BeGreaterOrEqualTo(1.81634).And.BeLessOrEqualTo(1.81635);

        }

        [Fact]
        public void MutualInformation()
        {

            MutualInformationCal sut = new MutualInformationCal();

            var expectedResult = 0.693;
            var result = sut.CalculateMI("AAAT", "ATTT");

            result.Should().BeLessThan(expectedResult + 0.05).And.BeGreaterThan(expectedResult - 0.05);
            //           print(sklearn.metrics.mutual_info_score(["0.75", "0.25"],["0.25", "0.75"]))
            // A-A  1/3 log2 1/3/(3/4*1/4) = 0.33 log2 0.33 / 0.1875=0.33 log2 1.76= 0.33*0.82=0.2706
            // A-T  2/3 log2 2/3/(0.75 * 0.75) = 0.66 log2 0.66/0.5625= 0.66 log2 1.173=0.66 * 0.23=0.1518
            // T-T   1/3 log2 1/3/(3/4*1/4) = 0.33 log2 0.33 / 0.1875=0.33 log2 1.76= 0.33*0.82=0.2706
            // AAAT
            // ATTT


            var expectedResult2 = 1.039;
      
            var result2 = sut.CalculateMI("AAAATTCCCG", "ATTTTTCCCG");

            result2.Should().BeLessThan(expectedResult2 + 0.05).And.BeGreaterThan(expectedResult2 - 0.05);
            //
            //A-A 0.25 log2 0.25 / (0.4* 0.1) = 0.25 * 2.6438 =  0.66
            //A-T 0.75 log2 0.75 / (0.4 * 0.5) = 0.75 log2 3.75= 0.75 * 1.906 = 1.4295
            //T-T 0.4 log2 0.4 / (0.2*0.5) = 0.4 log2 4 = 0.8
            //
            var expectedResult3 = 0.215;
            var result3 = sut.CalculateMI("AAATA", "ATCGG");

            result3.Should().BeLessThan(expectedResult3 + 0.05).And.BeGreaterThan(expectedResult3 - 0.05);

            
        }
    }
}