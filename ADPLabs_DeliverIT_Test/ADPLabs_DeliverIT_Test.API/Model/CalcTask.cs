
namespace ADPLabs_DeliverIT_Test.API.Model
{
    public class CalcTask
    {
        public string? ID { get; set; }

        public string? Operation { get; set; }

        public double Left { get; set; }

        public double Right { get; set; }
        public CalcTask()
        {

        }
    }
}