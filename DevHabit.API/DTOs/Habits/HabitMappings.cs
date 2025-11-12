using DevHabit.API.Entities;
using System.Linq.Expressions;

namespace DevHabit.API.DTOs.Habits
{
    public static class HabitMappings
    {
        public static Habit ToEntity(this CreateHabitDto createHabitDto)
        {
            var habit = new Habit
            {
                Id = $"h_{Guid.CreateVersion7()}",
                Name = createHabitDto.Name,
                Description = createHabitDto.Description,
                Type = createHabitDto.Type,
                Frequency = new Frequency
                {
                    Type = createHabitDto.Frequency.Type,
                    TimesPerPeriod = createHabitDto.Frequency.TimesPerPeriod
                },
                Target = new Target
                {
                    Value = createHabitDto.Target.Value,
                    Unit = createHabitDto.Target.Unit
                },
                Status = HabitStatus.None, // Set default or handle as needed
                IsArchived = createHabitDto.IsArchived,
                EndDate = createHabitDto.EndDate,
                Milestone = createHabitDto.Milestone == null ? null : new Milestone
                {
                    Target = createHabitDto.Milestone.Target,
                    Current = createHabitDto.Milestone.Current
                },
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = null
            };

            return habit;
        }

        public static Expression<Func<Habit, HabitDto>> ToDto()
        {
            return h => new HabitDto
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
            };
        }

        public static void UpdateFromDto(this Habit habit, UpdateHabitDto dto)
        {
            // Update basic properties
            habit.Name = dto.Name;
            habit.Description = dto.Description;
            habit.Type = dto.Type;
            habit.EndDate = dto.EndDate;

            // Update frequency (assuming it's immutable, create new instance)
            habit.Frequency = new Frequency
            {
                Type = dto.Frequency.Type,
                TimesPerPeriod = dto.Frequency.TimesPerPeriod
            };

            // Update target
            habit.Target = new Target
            {
                Value = dto.Target.Value,
                Unit = dto.Target.Unit
            };

            habit.UpdatedAtUtc = DateTime.UtcNow;
        }

    }
}
