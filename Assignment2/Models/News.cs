namespace Assignment2.Models
{
    public class News
    {
        public int NewsId { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public SportClubs SportClubs { get; set; }
    }
    public enum SportClubs
    {
        Alpha,
        Beta,
        Omega
    }
}
