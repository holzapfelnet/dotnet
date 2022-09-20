#pragma warning disable SA1600

using Polly;
using Polly.Contrib.WaitAndRetry;
using System.IO.Compression;
using System.Text.Json;

namespace PollyExample
{
    internal class FearAndGreedService
    {
        private readonly string _url = "https://production.dataviz.cnn.io/index/fearandgreed/graphdata/";
        private readonly string _requestUri;

        private readonly HttpClient _httpClient = new();

        private readonly IAsyncPolicy<HttpResponseMessage> _policy =
            Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrResult(x => x.StatusCode is System.Net.HttpStatusCode.InternalServerError or System.Net.HttpStatusCode.RequestTimeout)
                .WaitAndRetryAsync(Backoff.ExponentialBackoff(new TimeSpan(0, 0, 1), 5));

        public FearAndGreedService()
        {
            _requestUri = _url + DateTime.Now.ToString("yyyy-MM-dd");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            _httpClient.DefaultRequestHeaders.Add("accept-encoding", "gzip");
            _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36");
        }

        public async Task<FearAndGreedData> GetFearAndGreedData()
        {
            var response = await _policy.ExecuteAsync(() => _httpClient.GetAsync(_requestUri));
            var fearAndGreed = GetFearAndGreed(response);
            return fearAndGreed!.FearGreedData;
        }

        private static FearAndGreed GetFearAndGreed(HttpResponseMessage response)
        {
            var gzip = new GZipStream(response.Content.ReadAsStream(), CompressionMode.Decompress);

            var reader = new StreamReader(gzip);
            var content = reader.ReadToEnd();
            return JsonSerializer.Deserialize<FearAndGreed>(content)!;
        }
    }
}