using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Time.Interfaces;
using Time.Models;

namespace Time.Controllers
{
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

        [EnableCors("AllowAll")]
        [HttpGet("name/{first_name}/{last_name}")]
        public IActionResult Get(string first_name, string last_name)
        {
            var result = _hoursService.getEmployee(first_name, last_name);

            if (result == null)
            {
                return Ok(new EmployeeHoursDTO(first_name, last_name));
            }

            return Ok(result);
        }

        [EnableCors("AllowAll")]
        [HttpPut("name/{first_name}/{last_name}")]
        public IActionResult Put([FromBody] EmployeeHoursDTO employeeData)
        {
            return Ok(_hoursService.addTime(employeeData));
        }
    }
}
