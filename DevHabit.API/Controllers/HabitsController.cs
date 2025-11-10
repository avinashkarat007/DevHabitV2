using DevHabit.API.Database;
using DevHabit.API.DTOs.Habits;
using DevHabit.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                        .Select(h => new HabitDto
                        {
                            Id = h.Id,
                            Name = h.Name,
                            Description = h.Description,
                            Type = h.Type,
                            Frequency = new FrequencyDto
                            {
                                Type = h.Frequency.Type,
                                TimesPerPeriod = h.Frequency.TimesPerPeriod
                            },
                            Target = new TargetDto
                            {
                                Value = h.Target.Value,
                                Unit = h.Target.Unit
                            },
                            Status = h.Status,
                            IsArchived = h.IsArchived,
                            EndDate = h.EndDate,
                            Milestone = h.Milestone == null ? null : new MilestoneDto
                            {
                                Target = h.Milestone.Target,
                                Current = h.Milestone.Current
                            },
                            CreatedAtUtc = h.CreatedAtUtc,
                            UpdatedAtUtc = h.UpdatedAtUtc
                        })
                        .ToListAsync(ct);

            return Ok(habits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HabitDto>> GetHabit(string id, CancellationToken ct)
        {
            var habit = await dbContext.Habits
                        .Where(x => x.Id == id)
                        .Select(h => new HabitDto
                        {
                            Id = h.Id,
                            Name = h.Name,
                            Description = h.Description,
                            Type = h.Type,
                            Frequency = new FrequencyDto
                            {
                                Type = h.Frequency.Type,
                                TimesPerPeriod = h.Frequency.TimesPerPeriod
                            },
                            Target = new TargetDto
                            {
                                Value = h.Target.Value,
                                Unit = h.Target.Unit
                            },
                            Status = h.Status,
                            IsArchived = h.IsArchived,
                            EndDate = h.EndDate,
                            Milestone = h.Milestone == null ? null : new MilestoneDto
                            {
                                Target = h.Milestone.Target,
                                Current = h.Milestone.Current
                            },
                            CreatedAtUtc = h.CreatedAtUtc,
                            UpdatedAtUtc = h.UpdatedAtUtc
                        })
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

            // Optionally map Habit to HabitDto for response, or return habit.Id
            return CreatedAtAction(nameof(GetHabit), new { id = habit.Id }, habit);
        }
    }
}
