using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    public class LoggingDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingDelegatingHandler> _logger;
        public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger)
        {
            _logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(@"Sending request to {Url} - 
                    Method {Method} - Version {Version}",
                    request.RequestUri, 
                    request.Method,
                    request.Version);
                var response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation(@"Received a success response from {Url}",
                        response.RequestMessage?.RequestUri);
                }
                else
                {
                    _logger.LogWarning(@"Received a non-success status code {StatusCode}
                          {Url}",
                          (int)response.StatusCode,
                          response.RequestMessage?.RequestUri);
                }
                return response;
            }
            catch(HttpRequestException ex)
                when(ex.InnerException is SocketException { SocketErrorCode: SocketError.ConnectionRefused })
            {
                var hostWithPort = request.RequestUri.IsDefaultPort ? request.RequestUri.DnsSafeHost :
                    $"{request.RequestUri.DnsSafeHost}:{request.RequestUri.Port}";
                _logger.LogCritical(ex, "Unable to connect to {Host} Please check the"
                    + " configuration/settings to to ensure the correct URL for the service"
                    + " has been configured", hostWithPort);
            }
            return new HttpResponseMessage(HttpStatusCode.BadGateway)
            { 
                RequestMessage = request,
            };
        }
    }
}
