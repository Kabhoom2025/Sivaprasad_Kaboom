using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Event___Task_Scheduler_for_Remote_Teams.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            if (task == null)
            {
                return BadRequest();
            }
            var createdTask = await _taskService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskItem task)
        {
            if (task == null || id != task.Id)
            {
                return BadRequest();
            }
            var updatedTask = await _taskService.UpdateTaskAsync(task);
            if (updatedTask == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTasksByUserId(Guid userId)
        {
            var tasks = await _taskService.GetTasksByUserIdAsync(userId);
            return Ok(tasks);
        }
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetTasksByEventId(Guid eventId)
        {
            var tasks = await _taskService.GetTasksByEventIdAsync(eventId);
            return Ok(tasks);
        }
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetTasksByStatus(string status)
        {
            var tasks = await _taskService.GetTasksByStatusAsync(status);
            return Ok(tasks);
        }
        [HttpGet("priority/{priority}")]
        public async Task<IActionResult> GetTasksByPriority(string priority)
        {
            var tasks = await _taskService.GetTasksByPriorityAsync(priority);
            return Ok(tasks);
        }
        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetTasksByDate(DateTime date)
        {
            var tasks = await _taskService.GetTasksByDateAsync(date);
            return Ok(tasks);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchTasks(string searchTerm)
        {
            var tasks = await _taskService.SearchTasksAsync(searchTerm);
            return Ok(tasks);
        }
        [HttpGet("filter")]
        public async Task<IActionResult> FilterTasks(string status, string priority, DateTime? startDate, DateTime? endDate)
        {
            var tasks = await _taskService.FilterTasksAsync(status, priority, startDate, endDate);
            return Ok(tasks);
        }
        [HttpGet("sort")]
        public async Task<IActionResult> SortTasks(string sortBy, string sortOrder)
        {
            var tasks = await _taskService.SortTasksAsync(sortBy, sortOrder);
            return Ok(tasks);
        }
        [HttpGet("export")]
        public async Task<IActionResult> ExportTasks(string format)
        {
            var result = await _taskService.ExportTasksAsync(format);
            if (result == null)
            {
                return BadRequest("Invalid format");
            }
            return File(result, "application/octet-stream", $"tasks.{format}");
        }
        [HttpGet("import")]
        public async Task<IActionResult> ImportTasks(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }
            var result = await _taskService.ImportTasksAsync(file);
            if (!result)
            {
                return BadRequest("Failed to import tasks");
            }
            return Ok("Tasks imported successfully");
        }
        [HttpGet("reminder")]
        public async Task<IActionResult> GetTaskReminders()
        {
            var reminders = await _taskService.GetTaskRemindersAsync();
            return Ok(reminders);
        }
        [HttpGet("notification")]
        public async Task<IActionResult> GetTaskNotifications()
        {
            var notifications = await _taskService.GetTaskNotificationsAsync();
            return Ok(notifications);
        }
        [HttpGet("history")]
        public async Task<IActionResult> GetTaskHistory(Guid taskId)
        {
            var history = await _taskService.GetTaskHistoryAsync(taskId);
            if (history == null)
            {
                return NotFound();
            }
            return Ok(history);
        }
        [HttpGet("dependencies")]
        public async Task<IActionResult> GetTaskDependencies(Guid taskId)
        {
            var dependencies = await _taskService.GetTaskDependenciesAsync(taskId);
            if (dependencies == null)
            {
                return NotFound();
            }
            return Ok(dependencies);
        }
        [HttpGet("collaboration")]
        public async Task<IActionResult> GetTaskCollaboration(Guid taskId)
        {
            var collaboration = await _taskService.GetTaskCollaborationAsync(taskId);
            if (collaboration == null)
            {
                return NotFound();
            }
            return Ok(collaboration);
        }
        [HttpGet("progress")]
        public async Task<IActionResult> GetTaskProgress(Guid taskId)
        {
            var progress = await _taskService.GetTaskProgressAsync(taskId);
            if (progress == null)
            {
                return NotFound();
            }
            return Ok(progress);
        }
        [HttpGet("analytics")]
        public async Task<IActionResult> GetTaskAnalytics(Guid taskId)
        {
            var analytics = await _taskService.GetTaskAnalyticsAsync(taskId);
            if (analytics == null)
            {
                return NotFound();
            }
            return Ok(analytics);
        }
        [HttpGet("report")]
        public async Task<IActionResult> GetTaskReport(Guid taskId)
        {
            var report = await _taskService.GetTaskReportAsync(taskId);
            if (report == null)
            {
                return NotFound();
            }
            return Ok(report);
        }
        [HttpGet("status-update")]
        public async Task<IActionResult> GetTaskStatusUpdate(Guid taskId)
        {
            var statusUpdate = await _taskService.GetTaskStatusUpdateAsync(taskId);
            if (statusUpdate == null)
            {
                return NotFound();
            }
            return Ok(statusUpdate);
        }
        [HttpGet("comments")]
        public async Task<IActionResult> GetTaskComments(Guid taskId)
        {
            var comments = await _taskService.GetTaskCommentsAsync(taskId);
            if (comments == null)
            {
                return NotFound();
            }
            return Ok(comments);
        }
        [HttpGet("attachments")]
        public async Task<IActionResult> GetTaskAttachments(Guid taskId)
        {
            var attachments = await _taskService.GetTaskAttachmentsAsync(taskId);
            if (attachments == null)
            {
                return NotFound();
            }
            return Ok(attachments);
        }
        [HttpGet("tags")]
        public async Task<IActionResult> GetTaskTags(Guid taskId)
        {
            var tags = await _taskService.GetTaskTagsAsync(taskId);
            if (tags == null)
            {
                return NotFound();
            }
            return Ok(tags);
        }
        [HttpGet("recurrence")]
        public async Task<IActionResult> GetTaskRecurrence(Guid taskId)
        {
            var recurrence = await _taskService.GetTaskRecurrenceAsync(taskId);
            if (recurrence == null)
            {
                return NotFound();
            }
            return Ok(recurrence);
        }
        [HttpGet("time-tracking")]
        public async Task<IActionResult> GetTaskTimeTracking(Guid taskId)
        {
            var timeTracking = await _taskService.GetTaskTimeTrackingAsync(taskId);
            if (timeTracking == null)
            {
                return NotFound();
            }
            return Ok(timeTracking);
        }
        [HttpGet("time-estimation")]
        public async Task<IActionResult> GetTaskTimeEstimation(Guid taskId)
        {
            var timeEstimation = await _taskService.GetTaskTimeEstimationAsync(taskId);
            if (timeEstimation == null)
            {
                return NotFound();
            }
            return Ok(timeEstimation);
        }
        [HttpGet("time-logging")]
        public async Task<IActionResult> GetTaskTimeLogging(Guid taskId)
        {
            var timeLogging = await _taskService.GetTaskTimeLoggingAsync(taskId);
            if (timeLogging == null)
            {
                return NotFound();
            }
            return Ok(timeLogging);
        }
        [HttpGet("time-reporting")]
        public async Task<IActionResult> GetTaskTimeReporting(Guid taskId)
        {
            var timeReporting = await _taskService.GetTaskTimeReportingAsync(taskId);
            if (timeReporting == null)
            {
                return NotFound();
            }
            return Ok(timeReporting);
        }
        [HttpGet("time-off")]
        public async Task<IActionResult> GetTaskTimeOff(Guid taskId)
        {
            var timeOff = await _taskService.GetTaskTimeOffAsync(taskId);
            if (timeOff == null)
            {
                return NotFound();
            }
            return Ok(timeOff);
        }
        [HttpGet("time-off-requests")]
        public async Task<IActionResult> GetTaskTimeOffRequests(Guid taskId)
        {
            var timeOffRequests = await _taskService.GetTaskTimeOffRequestsAsync(taskId);
            if (timeOffRequests == null)
            {
                return NotFound();
            }
            return Ok(timeOffRequests);
        }
        [HttpGet("time-off-approval")]
        public async Task<IActionResult> GetTaskTimeOffApproval(Guid taskId)
        {
            var timeOffApproval = await _taskService.GetTaskTimeOffApprovalAsync(taskId);
            if (timeOffApproval == null)
            {
                return NotFound();
            }
            return Ok(timeOffApproval);
        }
        [HttpGet("time-off-history")]
        public async Task<IActionResult> GetTaskTimeOffHistory(Guid taskId)
        {
            var timeOffHistory = await _taskService.GetTaskTimeOffHistoryAsync(taskId);
            if (timeOffHistory == null)
            {
                return NotFound();
            }
            return Ok(timeOffHistory);
        }
        [HttpGet("time-off-balance")]
        public async Task<IActionResult> GetTaskTimeOffBalance(Guid taskId)
        {
            var timeOffBalance = await _taskService.GetTaskTimeOffBalanceAsync(taskId);
            if (timeOffBalance == null)
            {
                return NotFound();
            }
            return Ok(timeOffBalance);
        }
        [HttpGet("time-off-policy")]
        public async Task<IActionResult> GetTaskTimeOffPolicy(Guid taskId)
        {
            var timeOffPolicy = await _taskService.GetTaskTimeOffPolicyAsync(taskId);
            if (timeOffPolicy == null)
            {
                return NotFound();
            }
            return Ok(timeOffPolicy);
        }
        [HttpGet("time-off-requests-history")]
        public async Task<IActionResult> GetTaskTimeOffRequestsHistory(Guid taskId)
        {
            var timeOffRequestsHistory = await _taskService.GetTaskTimeOffRequestsHistoryAsync(taskId);
            if (timeOffRequestsHistory == null)
            {
                return NotFound();
            }
            return Ok(timeOffRequestsHistory);
        }
        [HttpGet("time-off-requests-approval")]
        public async Task<IActionResult> GetTaskTimeOffRequestsApproval(Guid taskId)
        {
            var timeOffRequestsApproval = await _taskService.GetTaskTimeOffRequestsApprovalAsync(taskId);
            if (timeOffRequestsApproval == null)
            {
                return NotFound();
            }
            return Ok(timeOffRequestsApproval);
        }




    }
}
