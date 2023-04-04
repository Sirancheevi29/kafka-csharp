using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using kafka_csharp__core_api.Model;
using kafka_csharp__core_api.Services;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json;

namespace kafka_csharp__core_api.Controllers
{
    [Route("api/kafka/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<Movies>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Movies>> Get()
        {
            try
            {
                KakfaCloud kakfa = new KakfaCloud();
                List<Movies> movies = kakfa.Consumer("movies");

                return Ok(movies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Movies), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Movies> Post(List<Movies> movies)
        {
            try
            {

                KakfaCloud kakfa = new KakfaCloud();
                kakfa.Producer("movies", movies);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
