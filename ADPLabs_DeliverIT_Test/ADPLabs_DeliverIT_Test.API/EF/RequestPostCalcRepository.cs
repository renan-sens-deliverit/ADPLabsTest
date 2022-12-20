using ADPLabs_DeliverIT_Test.API.EF.Interfaces;
using ADPLabs_DeliverIT_Test.API.Model;

namespace ADPLabs_DeliverIT_Test.API.EF
{
    public class RequestPostCalcRepository : IRequestPostCalc
    {
        public List<RequestPostCalc> GetRequestPostCalc()
        {
            using (var context = new ApiContext())
            {
                var list = context.RequestPostCalcs
                          .ToList();
                return list;
            }
        }

        public void Save(RequestPostCalc request)
        {
            using (var context = new ApiContext())
            {

                var newRequest = new RequestPostCalc
                {
                    id= request.id, 
                    result = request.result
                };
                context.Add(newRequest);
                context.SaveChanges();
            }
        }
    }
}
