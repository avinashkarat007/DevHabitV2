﻿using DevHabit.API.Database.Configurations;
using DevHabit.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.API.Database
{
    public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Habit> Habits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Application);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
