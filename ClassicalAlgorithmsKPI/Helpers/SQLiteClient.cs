using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalAlgorithmsKPI.Helpers
{
    public static class SQLiteClient
    {
        private const string DbConnectionString = "Data Source=TestDataDb.db";//initDBSample  TestDataDb

        private static SqliteConnection DbConnection;

        private static Dictionary<int, List<string>> DoQuery(string query, int maxColumnIdx)
        {
            DbConnection = new SqliteConnection(DbConnectionString);
            DbConnection.Open();
            var command = DbConnection.CreateCommand();
            command.CommandText = query;
            var result = new Dictionary<int, List<string>>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int startIdx = 0;
                    var key = reader.GetInt32(startIdx++);
                    var value = new List<string> { reader.GetString(startIdx++) };
                    result.Add(key, value);
                    while (startIdx <= maxColumnIdx)
                        value.Add(reader.GetString(startIdx++));
                }
            }
            DbConnection.Dispose();
            return result;
        }

        public static void GenerateTestDataDb()
        {
            DbConnection = new SqliteConnection(DbConnectionString);
            SQLitePCL.Batteries.Init();
            DbConnection.Open();

            var projectPath = Path.GetFullPath(Assembly.GetExecutingAssembly().Location);
            var sqlPath = Path.GetFullPath(Path.Combine(projectPath, @"..\..\..\..\ClassicalAlgorithmsKPI\SamplesDB\TestDataDb.sql"));

            var command = DbConnection.CreateCommand();
            command.CommandText = File.ReadAllText(sqlPath);
            command.ExecuteReader();

            DbConnection.Dispose();
        }

        public static Dictionary<string, int> GetLettersRate()
        {
            string query = "SELECT * FROM Letters;";
            var queryResult = DoQuery(query: query, maxColumnIdx: 2);
            return queryResult.ToDictionary(i => i.Value[0], i => int.Parse(i.Value[1]));
        }

        public static Tuple<string[], string[]> GetStringsCollections()
        {
            string query = "SELECT * FROM Strings;";
            var queryResult = DoQuery(query: query, maxColumnIdx: 2);
            var baseStrings = queryResult.Select(i => i.Value[0]).ToArray();
            var sortedStrings = queryResult.Select(i => i.Value[1]).ToArray();
            return Tuple.Create(baseStrings, sortedStrings);
        }

        public static string GetResultPassword()
        {
            string query = "SELECT * FROM ResultPassword;";
            var queryResult = DoQuery(query: query, maxColumnIdx: 1);
            return queryResult[1][0];
        }
    }
}
