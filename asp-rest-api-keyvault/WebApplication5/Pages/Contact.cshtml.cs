using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace WebApplication5.Pages
{
    public class ContactModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your contact page.";
            String APIToken = GetAccessToken("72f988bf-86f1-41af-91ab-2d7cd011db47", "05c0e4e1-5670-4bb6-9f31-f9a2d2b5394f", "5iZtSpfzQWWL4E9uLqVVnwkOjrZYNIbXSUDt+Z8Ua4M=").Result;
            String results = RestartWebApp(APIToken).Result;
            System.Diagnostics.Debug.WriteLine(results);
            Message = results;
        }
        public static async Task<string> RestartWebApp(string token)
        {
            HttpClient httpClient = new HttpClient();
            Console.WriteLine("Begin RestartWebApp");
            //string URL = "https://management.azure.com/subscriptions/bad7ae36-0b55-466a-b085-cf72954dd5fc" +
            //             "/resourceGroups/LPWinApps/providers/Microsoft.Web/sites/" +
            //             "lpmsi/restart?" +
            //             "api-version=2016-08-01";

            string URL = "https://management.azure.com/subscriptions/bad7ae36-0b55-466a-b085-cf72954dd5fc/resources?$filter=resourceType%20eq%20%27Microsoft.KeyVault/vaults%27&api-version=2019-05-01";
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await httpClient.GetAsync(URL);
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody.ToString();
        }
        public static async Task<string> GetAccessToken(string tenantId, string clientId, string clientKey)
        {
            System.Diagnostics.Debug.WriteLine("Begin GetAccessToken");
            string authContextURL = "https://login.windows.net/" + tenantId;
            var authenticationContext = new AuthenticationContext(authContextURL);
            var credential = new ClientCredential(clientId, clientKey);
            var result = await authenticationContext
                .AcquireTokenAsync("https://management.azure.com/", credential);
            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }
            string token = result.AccessToken;
            return token;
        }
    }
}
