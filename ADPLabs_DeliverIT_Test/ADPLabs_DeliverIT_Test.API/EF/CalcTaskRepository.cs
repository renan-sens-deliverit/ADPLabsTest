using ADPLabs_DeliverIT_Test.API.EF.Interfaces;
using ADPLabs_DeliverIT_Test.API.Model;

namespace ADPLabs_DeliverIT_Test.API.EF
{
    public class CalcTaskRepository : ICalcTaskRepository
    {
        public List<CalcTask> GetCalcTasks()
        {
            using (var context = new ApiContext())
            {
                var list = context.CalcTasks
                          .ToList();
                return list;
            }
        }

        public void Save(CalcTask calc)
        {
            using (var context = new ApiContext())
            {

                var newCalc = new CalcTask
                {
                    ID = calc.ID,
                    Left = calc.Left,
                    Right = calc.Right,
                    Operation = calc.Operation

                };
                context.Add(newCalc);
                context.SaveChanges();
            }

        }
    }
}
