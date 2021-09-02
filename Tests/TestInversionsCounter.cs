using ClassicalAlgorithmsKPI.DataGenerators;
using ClassicalAlgorithmsKPI.Helpers;
using ClassicalAlgorithmsKPI.Sorting;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class TestInversionsCounter
    {
        private const string _initialUrl = "https://courses.prometheus.org.ua/assets/courseware/v1/b562f0c2309e30de66682f4fa0b447ef/c4x/KPI/Algorithms101/asset/data_examples_02.zip";
        private const string _fiveFilmsUrl = "https://courses.prometheus.org.ua/assets/courseware/v1/9a95f1a8992d060eafde59c96919dede/c4x/KPI/Algorithms101/asset/input_1000_5.txt";
        private const string _hundredFilmsUrl = "https://courses.prometheus.org.ua/assets/courseware/v1/c2ab5f7283f9767fb6bad4739237c13c/c4x/KPI/Algorithms101/asset/input_1000_100.txt";

        private static readonly InversionsData InitialTestData = new InversionsData(url: _initialUrl);
        private static readonly IDictionary<string, User[]> InitialTestSamples = InitialTestData.Samples;
        private static readonly IDictionary<string, IDictionary> InitialTestAnswers = InitialTestData.Answers;

        private static readonly InversionsData FiveFilmsTestData = new InversionsData(url: _fiveFilmsUrl, films: 5, users: 1000);
        private static readonly IDictionary<string, User[]> FiveFilmsTestSamples = FiveFilmsTestData.Samples;

        private static readonly InversionsData HundredFilmsTestData = new InversionsData(url: _hundredFilmsUrl, films: 100, users: 1000);
        private static readonly IDictionary<string, User[]> HundredFilmsTestSamples = HundredFilmsTestData.Samples;

        [Test(Author = "Me", Description = "Test for split inversions counting"), Order(8)]
        [TestCase("Users: 5 --> films: 5", 1)]
        [TestCase("Users: 5 --> films: 5", 4)]
        [TestCase("Users: 5 --> films: 10", 2)]
        [TestCase("Users: 5 --> films: 10", 3)]
        [TestCase("Users: 10 --> films: 5", 7)]
        [TestCase("Users: 10 --> films: 5", 3)]
        [TestCase("Users: 50 --> films: 100", 48)]
        [TestCase("Users: 50 --> films: 100", 12)]
        [TestCase("Users: 50 --> films: 100", 18)]
        [TestCase("Users: 50 --> films: 100", 24)]
        [TestCase("Users: 50 --> films: 100", 9)]
        [TestCase("Users: 50 --> films: 100", 35)]
        [TestCase("Users: 50 --> films: 100", 49)]
        [TestCase("Users: 50 --> films: 100", 45)]
        [TestCase("Users: 50 --> films: 100", 50)]
        [TestCase("Users: 50 --> films: 100", 6)]
        [TestCase("Users: 100 --> films: 50", 48)]
        [TestCase("Users: 100 --> films: 50", 15)]
        [TestCase("Users: 100 --> films: 50", 21)]
        [TestCase("Users: 100 --> films: 50", 60)]
        [TestCase("Users: 100 --> films: 50", 58)]
        [TestCase("Users: 100 --> films: 50", 76)]
        [TestCase("Users: 100 --> films: 50", 38)]
        [TestCase("Users: 100 --> films: 50", 25)]
        [TestCase("Users: 100 --> films: 50", 81)]
        [TestCase("Users: 100 --> films: 50", 70)]
        public void TestInitialSequences(string testKey, int userIndex)
        {
            var testSamples = InitialTestSamples[testKey];
            var testAnswers = (Dictionary<int, int>)InitialTestAnswers[testKey][userIndex];
            var targetRate = testSamples.Where(item => item.Number == userIndex)
                                        .Select(item => item.Rate)
                                        .First()
                                        .ToArray();
            var usersRates = testSamples.Where(item => item.Number != userIndex)
                                        .ToDictionary(item => item.Number, item => item.Rate);


            foreach (KeyValuePair<int, int[]> userRate in usersRates)
            {
                var user = userRate.Key;
                var currentUserRate = userRate.Value;
                var preparedUserRate = currentUserRate.Zip(targetRate, (userRateInstance, targetRateInstance) => new { userRateInstance, targetRateInstance })
                                                      .ToDictionary(zipped => zipped.userRateInstance, zipped => zipped.targetRateInstance)
                                                      .OrderBy(pair => pair.Key)
                                                      .Select(pair => pair.Value)
                                                      .ToArray();
                var calculatedUserInversions = MergeSort.SortIntAndCountInversions(preparedUserRate);
                var expectedUserInversions = testAnswers[user];
                Assert.AreEqual(
                    calculatedUserInversions,
                    expectedUserInversions,
                    $"Sample \"{testKey}\"\n" +
                    $"User {userIndex}\n" +
                    $"Expected split inversions count {expectedUserInversions}\n" +
                    $"Actual split inversions count {calculatedUserInversions}"
                );
            }
            
        }

        [Test(Author = "Me", Description = "Test for split inversions counting 1000 / 5"), Order(9)]
        [TestCase("Users: 1000 --> films: 5", 452, 100, 7)]
        [TestCase("Users: 1000 --> films: 5", 863, 29, 0)]
        public void TestFiveFilmsSequences(string testKey, int userIndex, int relativeUserIndex, int expectedUserInversions)
        {
            var testSamples = FiveFilmsTestSamples[testKey];
            var targetRate = testSamples.Where(item => item.Number == userIndex)
                                        .Select(item => item.Rate)
                                        .First()
                                        .ToArray();
            var usersRates = testSamples.Where(item => item.Number != userIndex)
                                        .ToDictionary(item => item.Number, item => item.Rate);
            
            var relativeUserRate = usersRates[relativeUserIndex];
            var preparedUserRate = relativeUserRate.Zip(targetRate, (userRateInstance, targetRateInstance) => new { userRateInstance, targetRateInstance })
                                                   .ToDictionary(zipped => zipped.userRateInstance, zipped => zipped.targetRateInstance)
                                                   .OrderBy(pair => pair.Key)
                                                   .Select(pair => pair.Value)
                                                   .ToArray();
            var calculatedUserInversions = MergeSort.SortIntAndCountInversions(preparedUserRate);
            Assert.AreEqual(
                calculatedUserInversions,
                expectedUserInversions,
                $"Sample \"{testKey}\"\n" +
                $"User {userIndex}\n" +
                $"Expected split inversions count {expectedUserInversions}\n" +
                $"Actual split inversions count {calculatedUserInversions}"
            );
        }

        [Test(Author = "Me", Description = "Test for split inversions counting 1000 / 5"), Order(10)]
        [TestCase("Users: 1000 --> films: 100", 618, 1, 2368)]
        [TestCase("Users: 1000 --> films: 100", 951, 178, 2483)]
        public void TestHundredFilmsSequences(string testKey, int userIndex, int relativeUserIndex, int expectedUserInversions)
        {
            var testSamples = HundredFilmsTestSamples[testKey];
            var targetRate = testSamples.Where(item => item.Number == userIndex)
                                        .Select(item => item.Rate)
                                        .First()
                                        .ToArray();
            var usersRates = testSamples.Where(item => item.Number != userIndex)
                                        .ToDictionary(item => item.Number, item => item.Rate);

            var relativeUserRate = usersRates[relativeUserIndex];
            var preparedUserRate = relativeUserRate.Zip(targetRate, (userRateInstance, targetRateInstance) => new { userRateInstance, targetRateInstance })
                                                   .ToDictionary(zipped => zipped.userRateInstance, zipped => zipped.targetRateInstance)
                                                   .OrderBy(pair => pair.Key)
                                                   .Select(pair => pair.Value)
                                                   .ToArray();
            var calculatedUserInversions = MergeSort.SortIntAndCountInversions(preparedUserRate);
            Assert.AreEqual(
                calculatedUserInversions,
                expectedUserInversions,
                $"Sample \"{testKey}\"\n" +
                $"User {userIndex}\n" +
                $"Expected split inversions count {expectedUserInversions}\n" +
                $"Actual split inversions count {calculatedUserInversions}"
            );
        }
    }
}
