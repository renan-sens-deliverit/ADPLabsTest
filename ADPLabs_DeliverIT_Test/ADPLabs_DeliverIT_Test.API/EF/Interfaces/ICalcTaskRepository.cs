using ADPLabs_DeliverIT_Test.API.Model;

namespace ADPLabs_DeliverIT_Test.API.EF.Interfaces
{
    public interface ICalcTaskRepository
    {
        public List<CalcTask> GetCalcTasks();

        public void Save(CalcTask calc);
    }
}
