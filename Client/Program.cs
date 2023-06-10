using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            UdpClient client = new UdpClient();
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Parse("10.7.174.170"), 3040);

            while (true)
            {
                Console.Write("Enter country name (enter 'END' to exit): ");
                string countryName = Console.ReadLine();
                byte[] countryName_bytes = Encoding.Default.GetBytes(countryName);
                client.Send(countryName_bytes, countryName_bytes.Length, clientEndPoint);

                IPEndPoint? serverEndPoint = null;
                byte[] citiesInCountry_bytes = client.Receive(ref serverEndPoint);
                string citiesInCountry_str = Encoding.UTF8.GetString(citiesInCountry_bytes);

                if (string.IsNullOrEmpty(citiesInCountry_str))
                {
                    if (countryName == "END")
                        break;
                    else
                    {
                        Console.WriteLine("There are no cities in this country");
                        continue;
                    }
                }
                Console.WriteLine($"Cities in country: {citiesInCountry_str} : {DateTime.Now.ToShortTimeString()}");
            }
        }
    }
}