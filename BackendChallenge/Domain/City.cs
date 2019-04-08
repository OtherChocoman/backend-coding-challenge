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
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.AsciiName = asciiName ?? throw new ArgumentNullException(nameof(name));
            this.Country = country ?? throw new ArgumentNullException(nameof(name));
            this.Region = region ?? throw new ArgumentNullException(nameof(name));
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
    }
}
