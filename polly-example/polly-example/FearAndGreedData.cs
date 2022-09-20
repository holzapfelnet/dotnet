#pragma warning disable SA1600

using System.Text.Json.Serialization;

namespace PollyExample
{
    internal class FearAndGreedData
    {
        [JsonPropertyName("score")]
        public decimal Score { get; set; }

        [JsonPropertyName("rating")]
        public string Rating { get; set; } = default!;

        public override string ToString()
        {
            return string.Format("Rating: {0}, score: {1}", Rating, Score);
        }
    }
}