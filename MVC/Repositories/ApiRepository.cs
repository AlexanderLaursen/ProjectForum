﻿using Common.Enums;
using Common.Models;
using MVC.Models;
using MVC.Repositories.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MVC.Repositories
{
    public class ApiRepository : IApiRepository
    {
        private const string BASE_URL = "https://localhost:7052/api/v1/";

        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiRepository> _logger;
        private HttpClient httpClient;

        public ApiRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public ApiRepository(HttpClient httpClient, ILogger<ApiRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Result<T>> GetAsync<T>(string url, string? bearerToken = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(bearerToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                }

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"");
                    return HandleError<T>(response.StatusCode);
                }

                T? result = await response.Content.ReadFromJsonAsync<T>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result == null)
                {
                    _logger.LogError($"Failed to deserialize API response.");
                    return Result<T>.Failure("Failed to to deserialize API response", ResultStatus.Error);
                }

                return Result<T>.Success(result);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Unkown error occured while fetching from API.");
                return Result<T>.Failure();
            }
        }

        public async Task<ApiResponse<T>> GetAsyncOld<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(BASE_URL + url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return ApiResponse<T>.Success(apiResponse);
            }
            catch (Exception)
            {
                return ApiResponse<T>.Fail();
            }
        }

        public async Task<ApiResponseOld<T>> GetApiResponseAsyncOld<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(BASE_URL + url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponseOld<T>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse == null)
                {
                    return new ApiResponseOld<T>();
                }

                apiResponse.Content ??= new List<T>();
                apiResponse.IsSuccess = true;

                return apiResponse;
            }
            catch (Exception)
            {
                return new ApiResponseOld<T>();
            }
        }

        public async Task<ApiResponse<T>> GetAuthAsync<T>(string url, string bearerToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpResponseMessage response = await _httpClient.GetAsync(BASE_URL + url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return ApiResponse<T>.Success(content: apiResponse);
            }
            catch (Exception)
            {
                return ApiResponse<T>.Fail();
            }
        }

        public async Task<ApiResponseOld<T>> PostApiReponseAsync<T>(string url, object data, string bearerToken)
        {
            try
            {
                string json = JsonSerializer.Serialize(data);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpResponseMessage response = await _httpClient.PostAsync(BASE_URL + url, content);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseBody))
                {
                    return ApiResponseOld<T>.Success();
                }

                var apiResponse = JsonSerializer.Deserialize<ApiResponseOld<T>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse == null)
                {
                    return new ApiResponseOld<T>();
                }

                apiResponse.Content ??= new List<T>();
                apiResponse.IsSuccess = true;

                return apiResponse;
            }
            catch (Exception)
            {
                return new ApiResponseOld<T>();
            }
        }

        public async Task<ApiResponseOld<T>> PutAsync<T>(string url, object data, string bearerToken)
        {
            try
            {
                string json = JsonSerializer.Serialize(data);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpResponseMessage response = await _httpClient.PutAsync(BASE_URL + url, content);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponseOld<T>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse == null)
                {
                    return new ApiResponseOld<T>();
                }

                apiResponse.Content ??= new List<T>();
                apiResponse.IsSuccess = true;

                return apiResponse;
            }
            catch (Exception)
            {
                return new ApiResponseOld<T>();
            }
        }

        public async Task<ApiResponseOld<T>> DeleteAsync<T>(string url, string bearerToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpResponseMessage response = await _httpClient.DeleteAsync(BASE_URL + url);

                response.EnsureSuccessStatusCode();

                ApiResponseOld<T> apiResponse = new ApiResponseOld<T>
                {
                    IsSuccess = true
                };

                return apiResponse;
            }
            catch (Exception)
            {
                return new ApiResponseOld<T>();
            }
        }

        public string StringFactory(string baseString, int page = 0, int pageSize = 0)
        {
            return $"{baseString}?page={page}&pageSize={pageSize}";
        }

        public Result<T> HandleError<T>(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    return Result<T>.NotFound();
                case HttpStatusCode.BadRequest:
                    return Result<T>.InvalidInput();
                case HttpStatusCode.Unauthorized:
                    return Result<T>.Unauthorized();
                default:
                    return Result<T>.Failure();
            }
        }

        public PaginatedResult<T> HandlePaginatedError<T>(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    return PaginatedResult<T>.NotFound();
                case HttpStatusCode.BadRequest:
                    return PaginatedResult<T>.InvalidInput();
                case HttpStatusCode.Unauthorized:
                    return PaginatedResult<T>.Unauthorized();
                default:
                    return PaginatedResult<T>.Failure();
            }
        }
    }
}