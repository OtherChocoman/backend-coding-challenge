using System;
using System.Collections.Generic;
using System.Linq;

namespace BackendChallenge.Domain
{
    public class City
    {
        private const int LatitudePossibleValues = 180; // From -90 to 90

        private const int LongitudePossibleValues = 360; // From -180 to 180;

        public string Name { get; }

        private string AsciiName { get; }

        private IReadOnlyList<string> AlternateNames { get; }

        private string Country { get; }

        private string Region { get; }

        public float Latitude { get; }

        public float Longitude { get; }

        public string DisplayName => $"{this.Name}, {this.Region}, {this.Country}";

        public City(string name, string asciiName, IReadOnlyList<string> alternateNames, string country, string region, float latitude, float longitude)
        {
            ThrowIfNullOrWhiteSpace(name, nameof(name));
            ThrowIfNullOrWhiteSpace(asciiName, nameof(asciiName));
            ThrowIfNullOrWhiteSpace(country, nameof(country));
            ThrowIfNullOrWhiteSpace(region, nameof(region));
            ThrowIfLatitudeIsInvalid(latitude);
            ThrowIfLongitudeIsInvalid(longitude);

            this.Name = name;
            this.AsciiName = asciiName;
            this.Country = country;
            this.Region = region;
            this.AlternateNames = alternateNames?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>();
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public bool Matches(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return this.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase) ||
                this.AsciiName.Contains(name, StringComparison.InvariantCultureIgnoreCase) ||
                this.AlternateNames.Any(x => x.Contains(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public float CalculateScore(string name, float? latitude = null, float? longitude = null)
        {
            if (!this.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
            {
                return 0;
            }

            var score = (float) name.Length / this.Name.Length;

            if(latitude != null)
            {
                score *= this.CalculateLatitudeScoreFactor(latitude.Value);
            }

            if(longitude != null)
            {
                score *= this.CalculateLongitudeScoreFactor(longitude.Value);
            }

            return score;
        }

        private float CalculateLatitudeScoreFactor(float latitude)
        {
            var difference = (float)Math.Abs((decimal)(this.Latitude - latitude));
            return 1 - difference / LatitudePossibleValues;
        }

        private float CalculateLongitudeScoreFactor(float longitude)
        {
            var difference = (float)Math.Abs((decimal)(this.Longitude - longitude));
            return 1 - difference / LongitudePossibleValues;
        }

        private static void ThrowIfNullOrWhiteSpace(string parameter, string parameterName)
        {
            if(parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            if (parameter == string.Empty)
            {
                throw new ArgumentException($"{parameterName} cannot be empty");
            }
        }

        private static void ThrowIfLatitudeIsInvalid(float latitude)
        {
           if(latitude > 90 || latitude < -90)
           {
                throw new ArgumentException("Latitude must be between -90 and 90");
           }
        }

        private static void ThrowIfLongitudeIsInvalid(float longitude)
        {
            if (longitude > 180 || longitude < -180)
            {
                throw new ArgumentException("Latitude must be between -90 and 90");
            }
        }
    }
}
