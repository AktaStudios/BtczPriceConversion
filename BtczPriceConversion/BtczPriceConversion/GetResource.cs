using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BtczPriceConversion
{
    public static class GetResource
    {
        public static async Task<string> GetJsonResource(string uri)
        {
            HttpClient httpClient = null;
            try
            {
                httpClient = new HttpClient();

                // Set the Authorization header to identify the user.  The access token is not base64-encoded here
                // because it is expected to already be base64-encodedi.
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Set the Accept header to indicate that we want an XML based resource representation.
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(
                        "application/json"
                    ));

                // Send a GET request for the resource and await the response.
                var response = await httpClient.GetAsync(uri);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error on requesting data from Web API.  " + ex.ToString());
                return null;
            }

            finally
            {
                if (httpClient != null)
                    httpClient.Dispose();
            }
        }

        public static async Task<string> GetHtmlSource(string url)
        {
            HttpClient httpClient = null;
            try
            {
                httpClient = new HttpClient();
                
                var response = await httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error getting html source from btcz.me: " + ex.ToString());
                return null;
            }

            finally
            {
                if (httpClient != null)
                    httpClient.Dispose();
            }
        }

    }
}
