using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace JOBS.BLL.Helpers
{
    public class ApiHttpClient
    {
        private static readonly JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private HttpClient httpClient;

        public ApiHttpClient(HttpClient httpContext)
        {
            httpClient = httpContext;
        }


        public async Task<T> GetAsync<T>(string requestUri, IDictionary<string, string> parameters)
        {
            var uri = BuildUriWithParameters(requestUri, parameters);

            var response = await httpClient.GetAsync(uri);
            var responseBody = await response.Content.ReadAsStringAsync();


            return JsonSerializer.Deserialize<T>(responseBody, options);
        }

        public async Task<T> GetAsync<T>(string requestUri)
        {
            ValidateAndLogUri(requestUri);

            var response = await httpClient.GetAsync(requestUri);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseBody, options);
        }
        public async Task PostAsync<T>(string requestUri, IDictionary<string, string> parameters, T? viewModel)
        {
            ValidateAndLogUri(requestUri);
            var uri = BuildUriWithParameters(requestUri, parameters);

            var response = await httpClient.PostAsJsonAsync(uri, viewModel, options);
            var responseBody = await response.Content.ReadAsStringAsync();
        
        }
        public async Task PostAsync(string requestUri, IDictionary<string, string> parameters)
        {
            ValidateAndLogUri(requestUri);
            var uri = BuildUriWithParameters(requestUri, parameters);

            var response = await httpClient.PostAsJsonAsync(uri, options);
            var responseBody = await response.Content.ReadAsStringAsync();

        }
 

        public async Task<TOut> PostAsync<T, TOut>(string requestUri, T viewModel)
        {
            ValidateAndLogUri(requestUri);

            var response = await httpClient.PostAsJsonAsync(requestUri, viewModel, options);
            var responseBody = await response.Content.ReadAsStringAsync();


            return JsonSerializer.Deserialize<TOut>(responseBody, options);
        }

        public async Task PutAsync<T>(string requestUri, T viewModel)
        {
            ValidateAndLogUri(requestUri);

            var response = await httpClient.PutAsJsonAsync(requestUri, viewModel, options);
            var responseBody = await response.Content.ReadAsStringAsync();
   
        }

        public async Task PutAsync<T>(string requestUri, IDictionary<string, string> parameters, T viewModel)
        {
            var uri = BuildUriWithParameters(requestUri, parameters);
            ValidateAndLogUri(uri);

            var response = await httpClient.PutAsJsonAsync(uri, viewModel, options);
            var responseBody = await response.Content.ReadAsStringAsync();
    
        }

        public async Task DeleteAsync(string requestUri)
        {
            ValidateAndLogUri(requestUri);

            var response = await httpClient.DeleteAsync(requestUri);
            var responseBody = await response.Content.ReadAsStringAsync();
        
        }

        public async Task DeleteAsync(string requestUri, IDictionary<string, string> parameters)
        {
            var uri = BuildUriWithParameters(requestUri, parameters);
            ValidateAndLogUri(uri);

            var response = await httpClient.DeleteAsync(uri);
            var responseBody = await response.Content.ReadAsStringAsync();

        }

        private string BuildUriWithParameters(string requestUri, IDictionary<string, string> parameters)
        {
            var baseUri = new Uri(httpClient.BaseAddress, requestUri);
            var uriBuilder = new UriBuilder(baseUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var param in parameters)
            {
                query[param.Key] = param.Value;
            }

            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }
        private void ValidateAndLogUri(string requestUri)
        {
            if ((requestUri is null) || !Uri.IsWellFormedUriString(requestUri, UriKind.RelativeOrAbsolute))
            {
                throw new UriFormatException($"Invalid URI: The format of the URI '{requestUri}' could not be determined.");
            }
        }
    }
}
