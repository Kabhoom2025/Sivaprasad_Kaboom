using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event___Task_Scheduler_for_Remote_Teams.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] Comment comment)
        {
            if (comment == null) return BadRequest();
            var createdComment = await _commentService.AddCommentAsync(comment);
            return CreatedAtAction(nameof(GetCommentsByEntity), new { entityId = createdComment.LinkedEntityId, entityType = createdComment.EntityType }, createdComment);
        }
        [HttpGet("GetComments/{entityId}/{entityType}")]
        public async Task<IActionResult> GetCommentsByEntity(Guid entityId, string entityType)
        {
            var comments = await _commentService.GetCommentsByEntityAsync(entityId, entityType);
            return Ok(comments);
        }
        [HttpDelete("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var result = await _commentService.DeleteCommentAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
