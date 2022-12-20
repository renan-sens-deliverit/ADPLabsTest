using ADPLabs_DeliverIT_Test.API.EF.Interfaces;
using ADPLabs_DeliverIT_Test.API.Model;
using ADPLabs_DeliverIT_Test.API.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http;

namespace ADPLabs_DeliverIT_Test.API.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class CalcTaskController : ControllerBase
    {
        readonly ICalcTaskRepository _calcTaskRepository;
        readonly IRequestPostCalc _requestPostCalcRepository;

        private readonly ILogger<CalcTaskController> _logger;

        public CalcTaskController(ILogger<CalcTaskController> logger, ICalcTaskRepository calcTaskRepository, IRequestPostCalc requestPostCalcRepository)
        {
            _logger = logger;
            _requestPostCalcRepository = requestPostCalcRepository;
            _calcTaskRepository = calcTaskRepository;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet(Name = "GetCalcTask")]
        public ResponseCalcTaskResult? Get()
        {
            _logger.LogInformation("Service GetCalcTask start at: " + DateTime.Now.ToString());

            string error = String.Empty;
            ResponseCalcTaskResult? retCalc = null;
            try
            {
                RequestPostCalc? requestPost = null;
                CalcTask? responseConverted = null;
                Business? bu = null;

                //Retry for 3 times if any error
                int cont = 1;
                while (responseConverted == null && cont <= 3)
                {

                    System.Threading.Thread.Sleep(3000);

                    cont++;
                    responseConverted = Service.ServiceAgent.GetTask(out error);
                }

                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }


                if (responseConverted != null)
                {
                    _logger.LogInformation($"Success on GetCalcTask");

                    //Add object in memory
                    _calcTaskRepository.Save(responseConverted);

                    bu = new Business();
                    requestPost = bu.MakeCalcAndResult(responseConverted);

                    //Add object in memory
                    _requestPostCalcRepository.Save(requestPost);

                    _logger.LogInformation($"Make result for ID: {requestPost.id} Operation: {responseConverted.Operation} Values: ({responseConverted.Left},{responseConverted.Right}) Result: {requestPost.result} ");

                    if (!Service.ServiceAgent.PostTask(requestPost, out error))
                    {
                        throw new Exception(error);
                    }

                    retCalc = new ResponseCalcTaskResult { ID = responseConverted.ID, Operation = responseConverted.Operation, Left = responseConverted.Left, Right = responseConverted.Right, Result = requestPost.result };

                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                _logger.LogError($"ErrorMessage:{ex.Message} - StackTrace:{ex.StackTrace} - Success:{false}");
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred, please try again or contact the administrator."),
                    ReasonPhrase = "Critical Exception"
                });
            }

            _logger.LogInformation("Service GetCalcTask Finish at: " + DateTime.Now.ToString());
            return retCalc;
        }



    }
}