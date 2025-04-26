using Event___Task_Scheduler_for_Remote_Teams.Data;
using Event___Task_Scheduler_for_Remote_Teams.Hubs;
using Event___Task_Scheduler_for_Remote_Teams.Models;
using Event___Task_Scheduler_for_Remote_Teams.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Event___Task_Scheduler_for_Remote_Teams.Services.Implementations
{
    public class CommentService: ICommentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        public CommentService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            // Notify clients about the new comment
            await _hubContext.Clients.All.SendAsync("ReceiveComment", comment);
            return comment;
        }
        public async Task<bool> DeleteCommentAsync(Guid commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Comment>> GetCommentsByEntityAsync(Guid entityId, string entityType)
        {
            return await _context.Comments
                .Where(c => c.LinkedEntityId == entityId && c.EntityType == entityType)
                .ToListAsync();
        }
    }
   
 }
