namespace BackendChallenge.Application
{
    public class Suggestion
    {
        public string Name { get; }

        public float Latitude { get; }

        public float Longitude { get; }
    
        public float Score { get; }

        public Suggestion(string name, float latitude, float longitude, float score)
        {
            this.Name = name;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Score = score;
        }
    }
}
