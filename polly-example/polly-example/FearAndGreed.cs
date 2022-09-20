#pragma warning disable SA1600

using System.Text.Json.Serialization;

namespace PollyExample
{
    internal class FearAndGreed
    {
        [JsonPropertyName("fear_and_greed")]
        public FearAndGreedData FearGreedData { get; set; } = default!;
    }
}