namespace LabFive.Models
{
    public class Prediction
    {
        public int PredictionId { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public Question Question { get; set; }
    }
    public enum Question
    {
        Earth,
        Computer
    }
}
