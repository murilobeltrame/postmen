using Postmen.Sender.Application;
using System.Threading.Tasks;
using System.Web.Http;

namespace Postmen.Sender.Api.Framework.Controllers
{
    public class PostsController : ApiController
    {
        // POST api/values
        public async Task<IHttpActionResult> Post([FromBody] PostRequest request)
        {
            return Ok();
        }
    }
}
