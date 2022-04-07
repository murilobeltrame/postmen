using Microsoft.AspNetCore.Mvc;
using Postmen.Sender.Application;
using Postmen.Sender.Application.Interfaces;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Postmen.Sender.Api.Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IApplicationService _application;

        public PostsController(IApplicationService application)
        {
            _application = application;
        }

        // POST api/<PostsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostRequest request)
        {
            await _application.PublishAsync(request, default);
            return Ok();
        }
    }
}
