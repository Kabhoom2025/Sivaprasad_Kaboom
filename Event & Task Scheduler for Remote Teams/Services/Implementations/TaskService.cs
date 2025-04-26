using Event___Task_Scheduler_for_Remote_Teams.Data;
using Event___Task_Scheduler_for_Remote_Teams.Hubs;
using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Implementations
{
    public class TaskService:ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public TaskService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            task.Id = Guid.NewGuid();
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            // Notify clients about the new task
            await _hubContext.Clients.All.SendAsync("ReceiveTask", task);
            return task;
        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<byte[]> ExportTasksAsync(string format)
        {
            return Task.FromResult(new byte[0]);
        }

        public async Task<IEnumerable<TaskItem>> FilterTasksAsync(string status, string priority, DateTime? startDate, DateTime? endDate)
        {
            return null;
           // return await _context.Tasks
           //.Where(t =>
           //    (!string.IsNullOrEmpty(status) && t.TaskData.ContainsKey("Status") && t.TaskData["Status"].ToString() == status) &&
           //    (!string.IsNullOrEmpty(priority) && t.TaskData.ContainsKey("Priority") && t.TaskData["Priority"].ToString() == priority) &&
           //    (startDate == null || (t.TaskData.ContainsKey("DueDate") && DateTime.TryParse(t.TaskData["DueDate"].ToString(), out var d1) && d1 >= startDate)) &&
           //    (endDate == null || (t.TaskData.ContainsKey("DueDate") && DateTime.TryParse(t.TaskData["DueDate"].ToString(), out var d2) && d2 <= endDate))
           //)
           //.ToListAsync();
        }

        public async Task<List<TaskItem>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public Task<object> GetTaskAnalyticsAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetTaskAttachmentsAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public async Task<TaskItem> GetTaskByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public Task<IEnumerable<object>> GetTaskCollaborationAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetTaskCommentsAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetTaskDependenciesAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetTaskHistoryAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetTaskNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<object> GetTaskProgressAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetTaskRecurrenceAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetTaskRemindersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<object> GetTaskReportAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public  Task<IEnumerable<TaskItem>> GetTasksByDateAsync(DateTime date)
        {
            return  null;
           // return await _context.Tasks
           //.Where(t => t.TaskData.ContainsKey("DueDate") && DateTime.TryParse(t.TaskData["DueDate"].ToString(), out DateTime d) && d.Date == date.Date)
           //.ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByEventIdAsync(Guid eventId)
        {
            return await _context.Tasks
            .Where(t => t.TaskData.ContainsKey("EventId") && t.TaskData["EventId"].ToString() == eventId.ToString())
            .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByPriorityAsync(string priority)
        {
            return await _context.Tasks
            .Where(t => t.TaskData.ContainsKey("Priority") && t.TaskData["Priority"].ToString() == priority)
            .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(string status)
        {
            return await _context.Tasks
           .Where(t => t.TaskData.ContainsKey("Status") && t.TaskData["Status"].ToString() == status)
           .ToListAsync(); 
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(Guid userId)
        {
            return await _context.Tasks
            .Where(t => t.TaskData.ContainsKey("UserId") && t.TaskData["UserId"].ToString() == userId.ToString())
            .ToListAsync();
        }

        public Task<object> GetTaskStatusUpdateAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetTaskTagsAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetTaskTimeEstimationAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetTaskTimeLoggingAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetTaskTimeOffApprovalAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetTaskTimeOffAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetTaskTimeOffBalanceAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetTaskTimeOffHistoryAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetTaskTimeOffPolicyAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetTaskTimeOffRequestsApprovalAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetTaskTimeOffRequestsAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetTaskTimeOffRequestsHistoryAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetTaskTimeReportingAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetTaskTimeTrackingAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ImportTasksAsync(IFormFile file)
        {
            return Task.FromResult(true);
        }

        public async Task<IEnumerable<TaskItem>> SearchTasksAsync(string searchTerm)
        {
            return await _context.Tasks
             .Where(t => t.TaskData.Any(kv => kv.Value != null && kv.Value.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
             .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> SortTasksAsync(string sortBy, string sortOrder)
        {
            var tasks = await _context.Tasks.ToListAsync();

            var sorted = sortOrder.ToLower() == "desc"
                ? tasks.OrderByDescending(t => t.TaskData.ContainsKey(sortBy) ? t.TaskData[sortBy] : null)
                : tasks.OrderBy(t => t.TaskData.ContainsKey(sortBy) ? t.TaskData[sortBy] : null);

            return sorted;
        }

        public async Task<TaskItem> UpdateTaskAsync(TaskItem task)
        {
            var existingTask = await _context.Tasks.FindAsync(task.Id);
            if (existingTask == null) return null;

            existingTask.TaskData = task.TaskData;
            await _context.SaveChangesAsync();
            return existingTask;
        }

    }
}
