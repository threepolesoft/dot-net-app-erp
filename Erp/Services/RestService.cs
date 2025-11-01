using System.Text;
using static AppBO.Utility.Utility;
using System.Text.Json;

namespace Erp.Services
{
    public class RestService
    {

        public IConfiguration configuration;

        public RestService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<ApiResponse> GET(string url, string Token = "")
        {
            ApiResponse result = new();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration["BaseUrlApi"]);
                // Set headers if necessary
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer YOUR_TOKEN"); // Optional

                if (!string.IsNullOrEmpty(Token))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}"); // Optional
                }


                HttpResponseMessage response = await client.GetAsync(url); // Replace with your endpoint

                string responseData = await response.Content.ReadAsStringAsync();
                // Process the response as needed
                result = JsonSerializer.Deserialize<ApiResponse>(responseData);
                return result; // Return data to the view

                if (response.IsSuccessStatusCode)
                {
                    // Process the response as needed
                    result = JsonSerializer.Deserialize<ApiResponse>(responseData);
                    return result; // Return data to the view

                }
                else
                {
                    // Handle errors
                    // Handle errors
                    return new ApiResponse
                    {
                        Status = false,
                        Message = $"Request failed with status code {response.StatusCode}: {responseData}"
                    };
                }
            }
        }

        public async Task<ApiResponse> GET(string url, dynamic body)
        {
            ApiResponse result = new();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration["BaseUrlApi"]);

                // Set headers if necessary
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer YOUR_TOKEN"); // Optional

                //foreach (var item in header)
                //{
                //    client.DefaultRequestHeaders.Add(item.Key, item.Value); // Optional
                //}

                string jsonData = JsonSerializer.Serialize(body);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content); // Replace with your endpoint

                string responseData = await response.Content.ReadAsStringAsync();
                // Process the response as needed
                result = JsonSerializer.Deserialize<ApiResponse>(responseData);
                return result; // Return data to the view

                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    // Process the response as needed
                    result = JsonSerializer.Deserialize<ApiResponse>(responseData);
                    return result; // Return data to the view
                }
                else
                {
                    // Handle errors
                    return null;
                }
            }
        }

        public async Task<ApiResponse> POST(string url, dynamic body)
        {
            ApiResponse result = new();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration["BaseUrlApi"]);

                // Set headers if necessary
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer YOUR_TOKEN"); // Optional

                //foreach (var item in header)
                //{
                //    client.DefaultRequestHeaders.Add(item.Key, item.Value); // Optional
                //}

                string jsonData = JsonSerializer.Serialize(body);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content); // Replace with your endpoint

                string responseData = await response.Content.ReadAsStringAsync();

                // Process the response as needed
                result = JsonSerializer.Deserialize<ApiResponse>(responseData);
                return result; // Return data to the view

                if (response.IsSuccessStatusCode)
                {

                    // Process the response as needed
                    result = JsonSerializer.Deserialize<ApiResponse>(responseData);
                    return result; // Return data to the view
                }
                else
                {
                    throw new InvalidOperationException($"Status code: {response.StatusCode}, Date: {responseData}");

                }
            }
        }

        public async Task<ApiResponse> POST(string url, dynamic body, string token = "")
        {
            ApiResponse result = new();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration["BaseUrlApi"]);

                // Set headers if necessary
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer YOUR_TOKEN"); // Optional

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}"); // Optional
                }

                string jsonData = JsonSerializer.Serialize(body);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content); // Replace with your endpoint
                string responseData = await response.Content.ReadAsStringAsync();
                // Process the response as needed
                //result = JsonSerializer.Deserialize<ApiResponse>(responseData);
                //return result; // Return data to the view
                if (response.IsSuccessStatusCode)
                {

                    // Process the response as needed
                    result = JsonSerializer.Deserialize<ApiResponse>(responseData);
                    return result; // Return data to the view
                }
                else
                {
                    // Handle errors
                    // Handle errors
                    return new ApiResponse
                    {
                        Status = false,
                        Message = response.ReasonPhrase.ToLower()
                    };
                }
            }
        }
    }
}
