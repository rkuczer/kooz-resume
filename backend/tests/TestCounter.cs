using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Xunit;
using Company.Function;
using Moq;
using Microsoft.Azure.WebJobs;
using System.Collections.Generic;

namespace tests
{
    public class TestCounter
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async Task UpdateCount_Should_Increment_Counter()
        {
            // Arrange
            var initialCount = 5;
            var document = new JObject
            {
                ["id"] = "1",
                ["count"] = initialCount
            };

            var collector = new TestAsyncCollector<JObject>();
            var request = TestFactory.CreateHttpRequest();

            // Act
            var response = await UpdateCountFunction.Run(
                request,
                document,
                collector,
                logger);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Single(collector.Items);
            Assert.Equal(initialCount + 1, (int)collector.Items[0]["count"]);
        }

        [Fact]
        public async Task UpdateCount_Should_Return_Correct_Headers()
        {
            // Arrange
            var document = new JObject
            {
                ["id"] = "1",
                ["count"] = 1
            };

            var collector = new TestAsyncCollector<JObject>();
            var request = TestFactory.CreateHttpRequest();

            // Act
            var response = await UpdateCountFunction.Run(
                request,
                document,
                collector,
                logger);

            // Assert
            Assert.True(response.Headers.Contains("Access-Control-Allow-Origin"));
            Assert.Equal("*", response.Headers.GetValues("Access-Control-Allow-Origin").First());
            Assert.True(response.Headers.Contains("Access-Control-Allow-Methods"));
            Assert.True(response.Headers.Contains("Access-Control-Allow-Headers"));
        }

        [Fact]
        public async Task UpdateCount_Should_Return_JSON_Content()
        {
            // Arrange
            var initialCount = 10;
            var document = new JObject
            {
                ["id"] = "1",
                ["count"] = initialCount
            };

            var collector = new TestAsyncCollector<JObject>();
            var request = TestFactory.CreateHttpRequest();

            // Act
            var response = await UpdateCountFunction.Run(
                request,
                document,
                collector,
                logger);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains($"\"count\": {initialCount + 1}", content);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        }
    }

    // Helper class to mock IAsyncCollector with proper interface implementation
    public class TestAsyncCollector<T> : IAsyncCollector<T>
    {
        public readonly List<T> Items = new List<T>();

        async Task IAsyncCollector<T>.AddAsync(T item, CancellationToken cancellationToken)
        {
            Items.Add(item);
            await Task.CompletedTask;
        }

        async Task IAsyncCollector<T>.FlushAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}