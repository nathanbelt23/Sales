


namespace Sales.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Sales.Common.Models;
 
    public    class ApiService
    {

        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = "Internet apagado"
                };

            }

        var isReachable = await CrossConnectivity.Current.IsRemoteReachable("www.google.com");

            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No internet"
                };
            }

            var isReachableSite = await CrossConnectivity.Current.IsRemoteReachable("www.google.com");

            if (!isReachableSite)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No internet"
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "No internet"
            };
        }

        


        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller)
        {
            try
            {

                var client = new HttpClient();
                client.BaseAddress = new System.Uri(urlBase);

                var url = $"{prefix}{controller}";//esto es  como usuar String.Format("{0}{1}", prefix, controller);
                var response = await client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {

                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.RequestMessage.ToString()
                    };
                }


                var list = JsonConvert.DeserializeObject<List<T>>(answer);

                return new Response
                {

                    IsSuccess = true,
                    Message = "x",
                    Result = list

                };

            }
            catch (System.Exception e)
            {

               return  new Response{
                   
                    IsSuccess=false,
                    Message= e.Message
                };
            }
        }

    }
}
