# Implement resilient applications by using Polly

Modern applications have to communicate with several external systems.
At the same time, these applications should be be resilient against partial failures like timeouts.
To deal with this problem, you can use _Polly_.

```
private readonly IAsyncPolicy<HttpResponseMessage> _policy =
    Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .OrResult(x => x.StatusCode is System.Net.HttpStatusCode.InternalServerError or System.Net.HttpStatusCode.RequestTimeout)
        .WaitAndRetryAsync(Backoff.ExponentialBackoff(new TimeSpan(0, 0, 1), 5));


var response = await _policy.ExecuteAsync(() => _httpClient.GetAsync(_requestUri));
```

# Links

[Polly NuGet package](https://www.nuget.org/packages/Polly)

[Use IHttpClientFactory to implement resilient HTTP requests](https://learn.microsoft.com/de-de/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)

[.NET Microservices: Architecture for Containerized .NET Applications (e-book)](https://dotnet.microsoft.com/en-us/download/e-book/microservices-architecture/pdf)

[Nick Chapsas - The CORRECT way to implement Retries in .NET (YouTube)](https://www.youtube.com/watch?v=nJH0PC2Pubs)