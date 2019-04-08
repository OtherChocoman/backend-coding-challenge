using BackendChallenge.Domain;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackendChallenge.Bootstrap
{
    public static class CityListFactory
    {
        private const char ValueDelimiter = '\t';
        private const char AlternameNamesDeliminter = ',';

        public static IReadOnlyList<City> CreateCityList(string citiesFileLocation, string regionCodesFileLocation)
        {
            var regions = GetRegionCodes(regionCodesFileLocation);

            var cities = new List<City>();
            using (var reader = new StreamReader(citiesFileLocation))
            {
                reader.ReadLine(); // Skip First Line because it is the header
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(ValueDelimiter);

                    var city = new City(
                        values[1],
                        values[2],
                        values[3].Split(AlternameNamesDeliminter).Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
                        values[8],
                        regions[$"{values[8]}.{values[10]}"],
                        float.Parse(values[4]),
                        float.Parse(values[5]));

                    cities.Add(city);
                }
            }

            return cities;
        }

        private static Dictionary<string, string> GetRegionCodes(string regionCodesFileLocation)
        {
            var regions = new Dictionary<string, string>();

            using (var reader = new StreamReader(regionCodesFileLocation))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(ValueDelimiter);

                    regions.Add(values[0], values[1]);
                }
            }

            return regions;
        }
    }
}
