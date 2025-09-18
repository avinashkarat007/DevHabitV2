using DevHabit.API.Database;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.API.Extensions
{
    public static class DatabaseExtensions
    {
        private static readonly Action<ILogger, Exception> _migrationErrorLogger =
            LoggerMessage.Define(
                LogLevel.Information,
                new EventId(0, nameof(ApplyMigrationsAsync)),
                "Database migration applied successfully");


        public static async Task ApplyMigrationsAsync(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            using IServiceScope scope = app.Services.CreateScope();

            using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                await context.Database.MigrateAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _migrationErrorLogger(app.Logger, ex);
                throw;
            }
        }
    }
}
