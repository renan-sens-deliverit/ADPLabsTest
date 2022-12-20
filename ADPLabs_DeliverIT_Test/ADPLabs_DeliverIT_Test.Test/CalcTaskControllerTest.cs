using ADPLabs_DeliverIT_Test.API.Controllers;
using ADPLabs_DeliverIT_Test.API.EF;
using ADPLabs_DeliverIT_Test.API.EF.Interfaces;
using ADPLabs_DeliverIT_Test.API.Model;
using Microsoft.Extensions.Logging;

namespace ADPLabs_DeliverIT_Test.Test
{
    public class CalcTaskControllerTest
    {
        private CalcTaskRepository _calcTaskRepository;
        private RequestPostCalcRepository _requestPostCalc;
        private ILogger<CalcTaskController> _logger;
        private int _timesForTest;


        [SetUp]
        public void Setup()
        {
            _calcTaskRepository =  new CalcTaskRepository();
            _requestPostCalc = new RequestPostCalcRepository();
            _logger = new Logger<CalcTaskController>(new LoggerFactory());
            //Set quantity of times to call api
            _timesForTest = 10;
        }

        
        [Test]
        public void CalcTaskControllerTest1()
        {
            var controller = new CalcTaskController(_logger, _calcTaskRepository, _requestPostCalc);

            var response = controller.Get();
            //Get list of calc tasks and results generate in EF memory DB
            var calcs = _calcTaskRepository.GetCalcTasks();
            var results = _requestPostCalc.GetRequestPostCalc();

            Assert.True(response != null);

        }


        [Test]
        public void CalcTaskControllerTest1_forMultipleTimes()
        {
            int count = 0;
            while (count <= _timesForTest)
            {
                count++;
                var controller = new CalcTaskController(_logger, _calcTaskRepository, _requestPostCalc);

                var response = controller.Get();
                //Get list of calc tasks and results generate in EF memory DB
                var calcs = _calcTaskRepository.GetCalcTasks();
                var results = _requestPostCalc.GetRequestPostCalc();
              
                Assert.True(response != null);
            }
           

        }
    }
}