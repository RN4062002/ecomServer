using ecomServer.Data;
using ecomServer.Models;
using System;

namespace ecomServer.Services
{
    public class ErrorLogService
    {
        private readonly EcomDbContext db;

        public ErrorLogService(EcomDbContext _db)
        {
            db = _db;
        }

        public async Task LogErrorAsync(Exception ex, HttpContext context)
        {
            var error = new ErrorLog
            {
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace,
                Path = context.Request.Path,
                Method = context.Request.Method,
                CreatedAt = DateTime.Now
            };

            db.ErrorLogs.Add(error);
            await db.SaveChangesAsync();
        }
    }
}
