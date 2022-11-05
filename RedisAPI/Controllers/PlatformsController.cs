using Microsoft.AspNetCore.Mvc;
using RedisAPI.Data;
using RedisAPI.Models;

namespace RedisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;

        public PlatformsController(IPlatformRepo platformRepo)
        {
            _platformRepo = platformRepo;
        }

        [HttpGet("{id}", Name = "GetPlatform")]  
        public ActionResult<Platform> GetPlatform(string id)
        {
            var platform = _platformRepo.GetPlatformById(id);

            if (platform is null)
            {
                return NotFound($"Platform with id: '{id}' does not exist.");
            }

            return Ok(platform);
        }

        [HttpGet]
        public ActionResult GetPlatforms()
        {
            var platforms = _platformRepo.GetAllPlatforms();                      

            return Ok(platforms);
        }

        [HttpPost]
        public ActionResult<Platform> CreatePlatform(Platform platform)
        {
            _platformRepo.CreatePlatform(platform);

            return CreatedAtRoute(nameof(GetPlatform), new { Id = platform.Id}, platform);              
        }
    }
}
