using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nexmo.Api.Test.Unit
{
    internal class MockHttpMessageHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"value\":0.43}"),
            };
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await Task.FromResult(responseMessage);
        }
    }
}
