using ADPLabs_DeliverIT_Test.API.Model;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ADPLabs_DeliverIT_Test.API.Service
{
    public static class ServiceAgent
    {

        private static readonly string _urlBase = "https://interview.adpeai.com/api/v1";


        public static CalcTask? GetTask(out string error)
        {
            try
            {
                error = string.Empty;

                CalcTask? responseConverted = null;
                var client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(_urlBase);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_urlBase}/get-task"))
                {

                    var response = client.SendAsync(requestMessage).GetAwaiter().GetResult(); ;


                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        error = $"Code: {(int)response.StatusCode} - Message: {response?.Content.ReadAsStringAsync().GetAwaiter().GetResult() ?? "undefined"}";

                        return null;

                    }

                    var responsePayload = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!String.IsNullOrEmpty(responsePayload))
                    {
                        try
                        {
                            responseConverted = JsonConvert.DeserializeObject<CalcTask>(responsePayload);

                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }

                    return responseConverted;

                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }

        }

        public static bool PostTask(RequestPostCalc request, out string error)
        {
            error = string.Empty;

            try
            {
                var client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(_urlBase);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_urlBase}/submit-task"))
                {
                    var jsonContent = JsonConvert.SerializeObject(request);

                    requestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = client.SendAsync(requestMessage).GetAwaiter().GetResult();


                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        error = $"Code: {(int)response.StatusCode} - Message: {response?.Content.ReadAsStringAsync().GetAwaiter().GetResult() ?? "undefined"}";
                        return false;

                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

        }
    }
}
