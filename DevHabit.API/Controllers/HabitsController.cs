using DevHabit.API.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.API.Controllers
{
    [Route("habits")]
    [ApiController]
    public class HabitsController(ApplicationDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetHabits()
        {
            var habits = dbContext.Habits.ToListAsync();
            return Ok(habits);
        }
    }
}
