using Event___Task_Scheduler_for_Remote_Teams.Models;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> AddCommentAsync(Comment comment);
        Task<IEnumerable<Comment>> GetCommentsByEntityAsync(Guid entityId, string entityType);
        Task<bool> DeleteCommentAsync(Guid commentId);
    }
}
