using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using kafka_csharp__core_api.Model;

namespace kafka_csharp__core_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        [HttpGet]
        [Consumes("application/json")]
        [Route("movies")]
        [ProducesResponseType(typeof(List<Movies>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Movies>> Get()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
