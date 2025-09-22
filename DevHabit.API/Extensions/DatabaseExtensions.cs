using DevHabit.API.Database;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.API.Extensions
{
    public static class DatabaseExtensions
    {
        private static readonly Action<ILogger, Exception?> _logMigrationsApplied =
            LoggerMessage.Define(
                LogLevel.Information,
                new EventId(0, nameof(ApplyMigrationsAsync)),
                "Database migrations applied successfully."
            );

        private static readonly Action<ILogger, Exception?> _logMigrationsError =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(1, "DatabaseMigrationError"),
                "An error occurred while applying database migrations."
            );

        public static async Task ApplyMigrationsAsync(this WebApplication app)
        {

            using IServiceScope scope = app.Services.CreateScope();
            
            using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                await context.Database.MigrateAsync().ConfigureAwait(false);
                _logMigrationsApplied(app.Logger, null);
            }
            catch (Exception ex)
            {
                _logMigrationsError(app.Logger, ex);
                throw;
            }
        }
    }
}
