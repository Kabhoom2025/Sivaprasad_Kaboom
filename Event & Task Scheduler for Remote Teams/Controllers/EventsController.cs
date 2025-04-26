using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event___Task_Scheduler_for_Remote_Teams.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [HttpGet("GetAllEvents")]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }
        [HttpGet("GetEvent/{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return Ok(eventItem);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateEvent")]
        public async Task<IActionResult> CreateEvent([FromBody] EventItem eventItem)
        {
            if (eventItem == null)
            {
                return BadRequest();
            }
            var createdEvent = await _eventService.CreateEventAsync(eventItem);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }
        [HttpPut("UpateEvent/{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventItem eventItem)
        {
            if (eventItem == null || id != eventItem.Id)
            {
                return BadRequest();
            }
            var updatedEvent = await _eventService.UpdateEventAsync(eventItem);
            if (updatedEvent == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("DeleteEvent/{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var deleted = await _eventService.DeleteEventAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
