using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Time.Interfaces;
using Time.Models;

namespace Time.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class HoursController : ControllerBase
    {

        private readonly ILogger<HoursController> _logger;
        private readonly IHoursService _hoursService;

        public HoursController(ILogger<HoursController> logger, IHoursService hours)
        {
            _logger = logger;
            _hoursService = hours;
        }

        [EnableCors]
        [HttpGet("name/{first_name}/{last_name}")]
        public IActionResult Get(string first_name, string last_name)
        {
            return Ok(_hoursService.getEmployee(first_name, last_name));
        }

        [EnableCors]
        [HttpPost("name")]
        public IActionResult Post([FromBody] EmployeeHours employeeHours)
        {
            var response = _hoursService.save(employeeHours);
            return Ok(response);
        }

        [EnableCors]
        [HttpPut("name/{first_name}/{last_name}/{hours}/{epoch}")]
        public IActionResult Put(string first_name, string last_name, int hours, int epoch)
        {
            return Ok(_hoursService.addTime(first_name, last_name, hours, epoch));
        }
    }
}
