﻿using Domain.Models;

using Infrastructure.Configurations.ValueObjectsConverter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");

            builder.Property(c => c.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Start)
                .IsRequired();

            builder.Property(c => c.Duration)
                .HasConversion<LessonDurationConverter>()
                .IsRequired();               

            builder.Property(c => c.Type)
                .IsRequired();

            builder
                 .HasOne(l => l.Teacher)
                 .WithMany(u => u.LessonsAsTeacher)
                 .HasForeignKey(l => l.TeacherId);

            builder
                .HasOne(c => c.Vehicle)
                .WithMany(v => v.Lessons);

            builder
                .HasOne(l => l.Student)
                .WithMany(u => u.LessonsAsStudent);                

            builder
                .HasMany(l => l.WaitingList)
                .WithMany(u => u.WaitingList)
                .UsingEntity(j => j.ToTable("LessonUsersPending"));

            builder.Ignore(l => l.End);
        }
    }
}
