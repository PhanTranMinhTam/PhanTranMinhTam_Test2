using Microsoft.AspNetCore.Mvc;
using PhanTranMinhTam_TestLan2.Models;
using PhanTranMinhTam_TestLan2.Services;

namespace PhanTranMinhTam_TestLan2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleServices _scheduleServices;

        public ScheduleController(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllSchedules()
        {
            IEnumerable<Data.Schedule> schedules = await _scheduleServices.GetAllSchedulesAsync();
            return Ok(schedules);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMusicById(int id)
        {
            Data.Schedule schedule = await _scheduleServices.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return BadRequest();
            }
            return Ok(schedule);
        }
        [HttpPost]
        public async Task<IActionResult> Createschedule([FromForm] ScheduleDTO scheduleDto)
        {
            if (ModelState.IsValid)
            {
                await _scheduleServices.AddOrUpdateScheduleAsync(scheduleDto);

                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            bool result = await _scheduleServices.DeleteScheduleAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
