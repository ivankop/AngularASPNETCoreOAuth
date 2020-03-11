using AuthServer.Infrastructure.Data.Identity;
using RestSharp;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Controllers
{
    public class ADUserManager
    {
        public ADUserManager()
        {

        }

        public AppUser CheckPasswordAsync(string username, string password)
        {
            try
            {
                bool valid = false;
                using (var context = new PrincipalContext(ContextType.Domain, "MYDOMAIN"))
                {
                    valid = context.ValidateCredentials(username, password);
                    if (valid)
                    {

                        UserPrincipal user = new UserPrincipal(context);
                        user.UserPrincipalName = username;

                        using (var searcher = new PrincipalSearcher(user))
                        {
                            var searchResults = searcher.FindAll();
                            if (searchResults.Count() > 0)
                            {
                                UserPrincipal result = (UserPrincipal)searchResults.FirstOrDefault();
                                AppUser appUser = new AppUser();
                                appUser.UserName = username;
                                appUser.Id = result.Guid.ToString();
                                appUser.Name = username;
                                appUser.Email = result.EmailAddress;

                                return appUser;
                            }
                        }                        
                    }
                    return null;
                }
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
