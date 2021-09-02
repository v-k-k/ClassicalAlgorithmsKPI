using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalAlgorithmsKPI.Helpers
{
    public static class WebClient
    {
        private static HttpClient _client = new HttpClient();

        public static string DownloadZippedSamples(string source, bool semicolumnSeparator = false)
        {
            StringBuilder result = new StringBuilder();
            Task<System.IO.Stream> response = _client.GetAsync(source)
                                                     .Result
                                                     .Content
                                                     .ReadAsStreamAsync();
            using (ZipArchive archive = new ZipArchive(response.Result))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    using (Stream stream = entry.Open())
                    {
                        StreamReader reader = new StreamReader(stream);
                        result.Append(reader.ReadToEnd());
                        if (semicolumnSeparator) result.Append(";");
                    }
                }
            }
            return result.ToString();
        }

        public static string DownloadSamples(string source)
        {
            return _client.GetAsync(source)
                          .Result
                          .Content
                          .ReadAsStringAsync()
                          .Result;
        }
    }
}
