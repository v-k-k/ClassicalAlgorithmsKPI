using ClassicalAlgorithmsKPI.Helpers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections;

namespace ClassicalAlgorithmsKPI.DataGenerators
{
    public class InversionsData
    {
        private readonly Regex userPattern = new Regex(@"(?<=користувачів - ).*");
        private readonly Regex usersAnswerPattern = new Regex(@"(?<=користувача )\d+");
        private readonly Regex filmsPattern = new Regex(@"(?<=фільмів - ).*");
        private readonly Regex greedyDigitsPattern = new Regex(@"(\d+\s)+$", RegexOptions.Multiline);
        private readonly string[] splitter = { "############################\n# Кількість", };
        private readonly string[] answerSplitter = { "############################\n# Для користувача №", };
        private readonly string sampleKeyPattern = "Users: {0} --> films: {1}";

        public Dictionary<string, User[]> Samples = new Dictionary<string, User[]>();
        public Dictionary<string, IDictionary> Answers = new Dictionary<string, IDictionary>();

        private User[] GenerateUsersCollection(string[] rawUsersData)
        {
            User[] result = new User[rawUsersData.Length - 1];
            for (int i = 1; i < rawUsersData.Length; i++)
            {
                result[i - 1] = new User(rawUserRateString: rawUsersData[i]);
            }
            return result;
        }

        public InversionsData(string url)
        {
            var unzippedContent = WebClient.DownloadZippedSamples(url);
            var rawDataArray = unzippedContent.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            foreach(var rawData in rawDataArray)
            {
                MatchCollection userMatches = userPattern.Matches(rawData);
                MatchCollection filmsMatches = filmsPattern.Matches(rawData);
                MatchCollection digitsMatches = greedyDigitsPattern.Matches(rawData);
                var userDigits = digitsMatches[0].Value.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                Samples.Add(
                    string.Format(sampleKeyPattern, userMatches[0].Value, filmsMatches[0].Value),
                    GenerateUsersCollection(rawUsersData: userDigits)
                );
                var answers = rawData.Split(answerSplitter, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<int, IDictionary> usersInversions = new Dictionary<int, IDictionary>();
                for (int i = 1; i < answers.Length; i++)
                {
                    var userInAnswer = int.Parse(usersAnswerPattern.Matches(answers[i])[0].Value);
                    if (!usersInversions.ContainsKey(userInAnswer))
                    {
                        var expectedInversions = answers[i].Split(new[] { "############################" }, StringSplitOptions.RemoveEmptyEntries)[1]
                                                           .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                                           .Select(item => item.Split(' '))
                                                           .ToDictionary(userIdx => int.Parse(userIdx[0]), inversions => int.Parse(inversions[1]));
                        usersInversions.Add(userInAnswer, expectedInversions);
                    }
                }
                Answers.Add(
                    string.Format(sampleKeyPattern, userMatches[0].Value, filmsMatches[0].Value),
                    usersInversions
                );
            }
        }

        public InversionsData(string url, int films, int users)
        {
            var content = WebClient.DownloadSamples(source: url);
            var userDigits = content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Samples.Add(
                string.Format(sampleKeyPattern, users, films),
                GenerateUsersCollection(rawUsersData: userDigits)
            );
        }
    }
}
