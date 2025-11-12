using DevHabit.API.Database;
using DevHabit.API.DTOs.Habits;
using DevHabit.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace DevHabit.API.Controllers
{
    [Route("habits")]
    [ApiController]
    public class HabitsController(ApplicationDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<HabitDto>>> GetHabits(CancellationToken ct)
        {
            var habits = await dbContext.Habits
                        .Select(HabitMappings.ToDto())
                        .ToListAsync(ct);

            return Ok(habits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HabitDto>> GetHabit(string id, CancellationToken ct)
        {
            var habit = await dbContext.Habits
                        .Where(x => x.Id == id)
                        .Select(HabitMappings.ToDto())
                        .FirstOrDefaultAsync(ct);

            return Ok(habit);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHabit(CreateHabitDto createHabitDto, CancellationToken ct)
        {
            // Map CreateHabitDto to Habit entity
            var habit = createHabitDto.ToEntity();

            dbContext.Habits.Add(habit);
            await dbContext.SaveChangesAsync(ct);

            var habitDto = habit;

            // Optionally map Habit to HabitDto for response, or return habit.Id
            return CreatedAtAction(nameof(GetHabit), new { id = habit.Id }, habitDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateHabit(string id, UpdateHabitDto updateHabitDto, CancellationToken ct)
        {
            Habit? habit = await dbContext.Habits.FirstOrDefaultAsync(h => h.Id == id);

            if (habit is null)
            {
                return NotFound();
            }

            habit.UpdateFromDto(updateHabitDto);

            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
