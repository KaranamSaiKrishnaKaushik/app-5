using Microsoft.AspNetCore.Mvc;
using GherkinHome.Services;

namespace DotnetApp5
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyController : ControllerBase
    {
        private readonly IMyService _service;

        public MyController(IMyService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var entities = _service.GetAll();
            return Ok(entities);
        }
        
        
        [HttpPost("add-numbers")]
        public IActionResult AddNumbers([FromBody] AddNumbersDto dto)
        {
            int result = dto.Number1 + dto.Number2;
            return Ok(new { Sum = result });
        }
    }
}