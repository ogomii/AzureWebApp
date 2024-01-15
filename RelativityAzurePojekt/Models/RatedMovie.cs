namespace RelativityAzurePojekt.Models
{
    public class RatedMovie
    {

        public RatedMovie(Movie movie, double averageStars)
        {
            this.movie = movie;
            this.ratingAvg = averageStars;
        }

        public Movie movie { get; set; }
        public double ratingAvg { get; set; }
    }
}
