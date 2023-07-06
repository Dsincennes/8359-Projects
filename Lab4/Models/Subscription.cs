namespace Lab4.Models
{
    public class Subscription
    {
        public int FanID { get; set; }
        public string SportClubID { get; set; }

        public Fan Fan { get; set; }
        public SportClub SportClub { get; set; }
    }
}
