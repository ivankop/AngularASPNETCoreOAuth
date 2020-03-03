using AuthServer.Infrastructure.Data.Identity;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Controllers
{
    public class DSiUserManager
    {
        public DSiUserManager()
        {

        }

        public async Task<AppUser> CheckPasswordAsync(string username, string password)
        {
            var client = new RestClient("http://testsite1.dsimobile.com/api/v1/Web/user/login");
            var request = new RestRequest();
            var body = $"{{\"username\" : \"{username}\",\"password\": \"{password}\"}}";
            request.AddJsonBody(body);

            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.AddJsonBody(body);

            var response = await client.ExecuteAsync(request);
            var content = response.Content; // raw content as string              
            try
            {
                var myclass = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(content);

                AppUser appUser = new AppUser();
                appUser.UserName = username;
                appUser.Id = myclass.Data.UserId;
                appUser.Name = username;
                appUser.Email = username;

                return appUser;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}

/*
 * 
 *
 *    "Data": {
        "FirstName": "DSI Active Demo",
        "UserName": "TestDevice@dispatchingsolutions.com",
        "Token": "D4CQQYFLOAXRIJ553S6NOVZGEEC3C7DOVXEYCW7QM3U7YSLXZJHWGOAQJD6GYCHV6ZEJPC6KE2ZK5PF7OFQAAVJD7RNGS5BEWCACDVQ",
        "AccountId": 1655,
        "UserId": "9b63598e-019d-11e4-80c8-00155db47804",
        "IsDSiStaff": false,
        "Status": "Good"
    },
    "Message": null,
    "ReturnCode": "Success"
}
 * */
