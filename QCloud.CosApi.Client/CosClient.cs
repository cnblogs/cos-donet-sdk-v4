using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QCloud.CosApi.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QCloud.CosApi.Client
{
    public class CosClient
    {
        private CosClientOptions _cosClientOptions;
        private const string BASE_ADDRESS = "http://sh.file.myqcloud.com/files/v2";
        private const int SIGN_EXPIRED_TIME = 180;
        private readonly static HttpClient _httpClient = new HttpClient();
        private readonly ILogger _logger;

        public CosClient(IOptions<CosClientOptions> cosClientOptions,
            ILoggerFactory loggerFactory)
        {
            _cosClientOptions = cosClientOptions.Value;
            _logger = loggerFactory.CreateLogger<CosClient>();
        }

        public async Task<bool> UploadFileAsync(string bucketName, string remotePath, Stream uploadStream)
        {
            var encodedRemotePath = HttpUtility.UrlPathEncode(remotePath.TrimStart('/'));
            var path = $"/{_cosClientOptions.AppId}/{bucketName}/{encodedRemotePath}";
            var url = $"{BASE_ADDRESS}{path}";
            var signature = GenerateSignature(bucketName);
            var fileName = Path.GetFileName(remotePath);

            _logger.LogInformation($"POST to {url}");

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Authorization", signature);

            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            var data = new MultipartFormDataContent(boundary);
            data.Add(new ByteArrayContent(Encoding.UTF8.GetBytes("upload")), "\"op\"");

            var streamContent = new StreamContent(uploadStream);
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"fileContent\"",
                FileName = "\"" + fileName + "\""
            };
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            data.Add(streamContent);

            data.Headers.Remove("Content-Type");
            data.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);
            request.Content = data;
            var response = await _httpClient.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            var result = new { Code = 0, Message = "" };
            try
            {
                result = JsonConvert.DeserializeAnonymousType(json, result);
            }
            catch
            {
                _logger.LogError(0, json);
                return false;
            }

            if (result.Code != 0)
            {
                _logger.LogError(json);
                return false;
            }
            else
            {
                _logger.LogInformation(json);
                return true;
            }
        }

        //Task<bool> DeleteFileAsync(string virtualPath)
        //{

        //}

        private string GenerateSignature(string bucketName)
        {
            return Sign.Signature(_cosClientOptions.AppId,
                _cosClientOptions.SecretId,
                _cosClientOptions.SecretKey,
                getExpiredTime(),
                bucketName);
        }

        private long getExpiredTime()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds() + SIGN_EXPIRED_TIME;
        }
    }
}
