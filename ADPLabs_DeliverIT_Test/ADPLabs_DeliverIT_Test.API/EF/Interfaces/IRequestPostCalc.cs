using ADPLabs_DeliverIT_Test.API.Model;

namespace ADPLabs_DeliverIT_Test.API.EF.Interfaces
{
    public interface IRequestPostCalc
    {
        public List<RequestPostCalc> GetRequestPostCalc();
        public void Save(RequestPostCalc request);
    }
}
