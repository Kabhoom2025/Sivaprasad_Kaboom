using Event___Task_Scheduler_for_Remote_Teams.Models;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces
{
    public interface ITaskService
    {
        public Task<List<TaskItem>> GetAllTasksAsync();
        Task<TaskItem> UpdateTaskAsync(TaskItem task);
        Task<TaskItem> GetTaskByIdAsync(Guid id);
        Task<bool> DeleteTaskAsync(Guid id);
        public  Task<TaskItem> CreateTaskAsync(TaskItem task);
        Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(Guid userId);
        Task<IEnumerable<TaskItem>> GetTasksByEventIdAsync(Guid eventId);
        Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(string status);
        Task<IEnumerable<TaskItem>> GetTasksByPriorityAsync(string priority);
        Task<IEnumerable<TaskItem>> GetTasksByDateAsync(DateTime date);
        Task<IEnumerable<TaskItem>> SearchTasksAsync(string searchTerm);
        Task<IEnumerable<TaskItem>> FilterTasksAsync(string status, string priority, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<TaskItem>> SortTasksAsync(string sortBy, string sortOrder);

        Task<byte[]> ExportTasksAsync(string format);
        Task<bool> ImportTasksAsync(IFormFile file);

        Task<IEnumerable<object>> GetTaskRemindersAsync();
        Task<IEnumerable<object>> GetTaskNotificationsAsync();

        // Replace int taskId with Guid taskId below
        Task<IEnumerable<object>> GetTaskHistoryAsync(Guid taskId);
        Task<IEnumerable<object>> GetTaskDependenciesAsync(Guid taskId);
        Task<IEnumerable<object>> GetTaskCollaborationAsync(Guid taskId);
        Task<object> GetTaskProgressAsync(Guid taskId);
        Task<object> GetTaskAnalyticsAsync(Guid taskId);
        Task<object> GetTaskReportAsync(Guid taskId);
        Task<object> GetTaskStatusUpdateAsync(Guid taskId);
        Task<IEnumerable<object>> GetTaskCommentsAsync(Guid taskId);
        Task<IEnumerable<object>> GetTaskAttachmentsAsync(Guid taskId);
        Task<IEnumerable<object>> GetTaskTagsAsync(Guid taskId);
        Task<object> GetTaskRecurrenceAsync(Guid taskId);

        // Time Tracking & Management
        Task<object> GetTaskTimeTrackingAsync(Guid taskId);
        Task<object> GetTaskTimeEstimationAsync(Guid taskId);
        Task<object> GetTaskTimeLoggingAsync(Guid taskId);
        Task<object> GetTaskTimeReportingAsync(Guid taskId);

        // Time Off
        Task<object> GetTaskTimeOffAsync(Guid taskId);
        Task<IEnumerable<object>> GetTaskTimeOffRequestsAsync(Guid taskId);
        Task<object> GetTaskTimeOffApprovalAsync(Guid taskId);
        Task<IEnumerable<object>> GetTaskTimeOffHistoryAsync(Guid taskId);
        Task<object> GetTaskTimeOffBalanceAsync(Guid taskId);
        Task<object> GetTaskTimeOffPolicyAsync(Guid taskId);
        Task<IEnumerable<object>> GetTaskTimeOffRequestsHistoryAsync(Guid taskId);
        Task<IEnumerable<object>> GetTaskTimeOffRequestsApprovalAsync(Guid taskId);
    }
}
