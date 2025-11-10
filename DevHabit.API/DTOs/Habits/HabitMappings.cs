using DevHabit.API.Entities;

namespace DevHabit.API.DTOs.Habits
{
    public static class HabitMappings
    {
        public static Habit ToEntity(this CreateHabitDto createHabitDto)
        {
            var habit = new Habit
            {
                Id = Guid.NewGuid().ToString(),
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
    }
}
