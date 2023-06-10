using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;
using CitiesLibrary;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<City>? cities = new List<City>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<City>));

            FileInfo file = new FileInfo(@"C:\Users\maxik\source\repos\NetworkProgrammingCW_ClientServerTask1\Server\cities.xml");
            using (FileStream fs = file.OpenRead())
            {
                cities = serializer.Deserialize(fs) as List<City>;
            }

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("10.7.174.170"), 3040);
            UdpClient server = new UdpClient(endPoint);

            while (true)
            {
                Console.WriteLine("Waiting for the request...");

                IPEndPoint? clientEndPoint = null;
                byte[] countryName_bytes = server.Receive(ref clientEndPoint);
                string countryName = Encoding.UTF8.GetString(countryName_bytes);
                if (countryName == "END")
                {
                    server.Send(Array.Empty<byte>(), 0, clientEndPoint);
                    break;
                }
                Console.WriteLine($"Country name: {countryName}");

                List<string> citiesInCountry = cities.Where(city => city.CountryName == countryName)
                                                     .Select(city => city.CityName)
                                                     .ToList();

                byte[] citiesInCountry_bytes = Encoding.UTF8.GetBytes(string.Join(", ", citiesInCountry));
                server.Send(citiesInCountry_bytes, citiesInCountry_bytes.Length, clientEndPoint);
            }
        }
    }
}