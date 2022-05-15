namespace Cloth.Models.ViewModel
{
    public class AnalyticsModel
    {
        //array
        public List<string> First = new List<string>();
        public List<string> Second = new List<string>();
        public List<string> Third = new List<string>();
        
        public IEnumerable<Products> Products { get; set; }

        public List<string> QuantityPoint = new List<string>();
        public List<string> MidlePrice = new List<string>();
        public List<string> TopPrice = new List<string>();

        public double Max { get; set; }
        public double Min { get; set; }

        public double FirstPoint { get; set; }
        public double SecondPoint { get; set; }
        public double ThirdPoint { get; set; }
        public double FourthPoint { get; set; }
    }
}
