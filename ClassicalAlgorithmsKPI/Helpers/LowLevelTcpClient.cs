using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ClassicalAlgorithmsKPI.Helpers
{
    public static class LowLevelTcpClient
    {
        private static Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const string BASE_HOST = "courses.prometheus.org.ua";
        private const int SECURE_PORT = 443;

        public static IPAddress ResolveHostNameToIPAddress(string strHostName)
        {
            IPAddress[] retAddr = null;

            try
            {
                retAddr = Dns.GetHostAddresses(strHostName);

                foreach (IPAddress addr in retAddr)
                {
                    if (addr.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return addr;
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            return null;
        }

        public static string GetContent(string host = BASE_HOST, int port = SECURE_PORT, string endPoint = null)
        {
            string[] heads = {
                $"GET /{endPoint} HTTP/1.1",
                $"Host: {host}",
                "User-Agent: BST Algorithm",
                "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
                "Accept-Language: sr,sr-RS;q=0.8,sr-CS;q=0.6,en-US;q=0.4,en;q=0.2",
                "Connection: keep-alive"
            };
            List<byte> bufferList = new List<byte>();
            string response;

            using (client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                client.Connect(new IPEndPoint(ResolveHostNameToIPAddress(host), port));

                using (var stream = new NetworkStream(client))
                {
                    using (var sslStream = new SslStream(stream, false))
                    {
                        sslStream.AuthenticateAsClient(host);
                        using (var streamWriter = new StreamWriter(sslStream))
                        {
                            foreach (string head in heads)
                            {
                                streamWriter.WriteLine(head);
                            }
                            streamWriter.WriteLine();
                            streamWriter.Flush();

                            var lines = ReadHeader(sslStream).ToArray();
                            var contentLengthLine = lines.First(x => x.StartsWith("Content-Length"));
                            var split = contentLengthLine.Split(new string[] { ": ", }, StringSplitOptions.RemoveEmptyEntries);
                            var contentLength = int.Parse(split[1]);

                            var totalBytesRead = 0;
                            int bytesRead;
                            var buffer = new byte[contentLength];
                            do
                            {
                                bytesRead = sslStream.Read(buffer,
                                    totalBytesRead,
                                    contentLength - totalBytesRead);
                                totalBytesRead += bytesRead;                            
                                bufferList.AddRange(buffer);

                            } while (totalBytesRead < contentLength && bytesRead > 0);
                            response = Encoding.UTF8.GetString(bufferList.ToArray());
                        }
                    }
                }

                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            return response; 
        }

        private static bool ValidateServerCertificate(
            object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private static IEnumerable<string> ReadHeader(Stream sslStream)
        {
            // One-byte buffer for reading bytes from the stream
            var buffer = new byte[1];

            // Initialize a four-character string to keep the last four bytes of the message
            var check = new StringBuilder("....");
            int bytes;
            var responseBuilder = new StringBuilder();
            do
            {
                // Read the next byte from the stream and write in into the buffer
                bytes = sslStream.Read(buffer, 0, 1);
                if (bytes == 0)
                {
                    // If nothing was read, break the loop
                    break;
                }

                // Add the received byte to the response builder.
                // We expect the header to be ASCII encoded so it's OK to just cast to char and append
                responseBuilder.Append((char)buffer[0]);

                // Always remove the first char from the string and append the latest received one
                check.Remove(0, 1);
                check.Append((char)buffer[0]);

                // \r\n\r\n marks the end of the message header, so break here
                if (check.ToString() == "\r\n\r\n")
                {
                    break;
                }
            } while (bytes > 0);

            var headerText = responseBuilder.ToString();
            return headerText.Split(new string[] { "\r\n", }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
