using ADPLabs_DeliverIT_Test.API.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ADPLabs_DeliverIT_Test.API.EF;
using System.Text;
using ADPLabs_DeliverIT_Test.API.EF.Interfaces;

namespace ADPLabs_DeliverIT_Test.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalcTaskController : ControllerBase
    {
        readonly ICalcTaskRepository _calcTaskRepository;
        readonly IRequestPostCalc _requestPostCalcRepository;

        private readonly string _urlBase = "https://interview.adpeai.com/api/v1";

        private readonly ILogger<CalcTaskController> _logger;

        public CalcTaskController(ILogger<CalcTaskController> logger, ICalcTaskRepository calcTaskRepository, IRequestPostCalc requestPostCalcRepository)
        {
            _logger = logger;
            _requestPostCalcRepository = requestPostCalcRepository;
            _calcTaskRepository = calcTaskRepository;
        }

        [HttpGet(Name = "GetCalcTask")]
        public ResponseCalcTaskResult? Get()
        {
            _logger.LogInformation("Service GetCalcTask start at: " +  DateTime.Now.ToString());

            string error = String.Empty;
            ResponseCalcTaskResult? retCalc = null;
            try
            {
                RequestPostCalc? requestPost = null;
                CalcTask? responseConverted = null;

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
                         
                    //Make the operation
                    requestPost = new RequestPostCalc();
                    requestPost.id = responseConverted.ID;

                    switch (responseConverted.Operation.ToLower())
                    {
                        case "subtraction":
                            requestPost.result = responseConverted.Left - responseConverted.Right;
                            break;
                        case "multiplication":
                            requestPost.result = responseConverted.Left * responseConverted.Right;
                            break;
                        case "division":
                            requestPost.result = responseConverted.Left / responseConverted.Right;
                            break;
                        case "addition":
                            requestPost.result = responseConverted.Left + responseConverted.Right;
                            break;
                        case "remainder":
                            requestPost.result = responseConverted.Left % responseConverted.Right;
                            break;
                    }

                    //Add object in memory
                    _requestPostCalcRepository.Save(requestPost);

                    _logger.LogInformation($"Make result for ID: {requestPost.id} Operation: {responseConverted.Operation} Values: ({responseConverted.Left},{responseConverted.Right}) Result: {requestPost.result} ");

                    if (!Service.ServiceAgent.PostTask(requestPost, out error))
                    {
                        throw new Exception(error);
                    }

                    _logger.LogInformation("Service GetCalcTask Finish at: " + DateTime.Now.ToString());

                    retCalc =  new ResponseCalcTaskResult { ID = responseConverted.ID, Operation = responseConverted.Operation, Left = responseConverted.Left, Right = responseConverted.Right, Result = requestPost.result };
                
                }
                                                            
            }
            catch (Exception ex)
            {
                error = ex.Message;
                _logger.LogError($"ErrorMessage:{ex.Message} - StackTrace:{ex.StackTrace} - Success:{false}");
            }

            _logger.LogInformation("Service GetCalcTask Finish at: " + DateTime.Now.ToString());
            return retCalc;
        }


      
    }
}