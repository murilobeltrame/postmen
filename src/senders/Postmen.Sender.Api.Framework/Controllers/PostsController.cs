using Postmen.Sender.Application;
using Postmen.Sender.Application.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace Postmen.Sender.Api.Framework.Controllers
{
    public class PostsController : ApiController
    {
        private readonly IApplicationService _applicationService;

        public PostsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        // POST api/values
        public async Task<IHttpActionResult> Post([FromBody] PostRequest request)
        {
            await _applicationService.PublishAsync(request, default);
            return Ok();
        }
    }
}
