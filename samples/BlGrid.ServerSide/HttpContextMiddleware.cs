using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DnetSpinnerComponent.Infrastructure.Interfaces;

namespace BlGrid.ServerSide
{
    public class HttpContextMiddleware : DelegatingHandler
    {
        private ISpinnerService _spinnerService;

        private volatile bool _disposed = false;

        public HttpContextMiddleware(ISpinnerService spinnerService)
        {
            _spinnerService = spinnerService;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _spinnerService.UdateCounter(1);

            return base.SendAsync(request, cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                //_spinnerService.UdateCounter(-1);
            }

            base.Dispose(disposing);
        }
    }
}
