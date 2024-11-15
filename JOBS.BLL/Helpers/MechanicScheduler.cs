using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JOBS.BLL.Helpers
{
    public class MechanicScheduler
    {
        public static Mechanic? AssignTaskToLeastBusyMechanic(
            ServiceStationDBContext context,
            Specialisation requiredSpecialisation,
            DateTime plannedStartDate,
            TimeSpan estimatedTaskDuration)
        {
            // Крок 1: Фільтруємо механіків за спеціалізацією
            var filteredMechanics = context.Mechanics.Include(p=>p.Specialisation).ToList()
                .Where(m => m.Specialisation.Id == requiredSpecialisation.Id)
                .ToList();

            // Крок 2: Знаходимо механіка з найменшим навантаженням
            var availableMechanic = filteredMechanics
                .Select(m => new
                {
                    Mechanic = m,
                    NextAvailableTime = GetNextAvailableTime(m, plannedStartDate),
                    TotalWorkload = m.MechanicsTasks
                        .Where(t => t.FinishDate == null || t.FinishDate > plannedStartDate)
                        .Sum(t => (t.FinishDate ?? plannedStartDate).Subtract(t.IssueDate).TotalHours)
                })
                .OrderBy(m => m.NextAvailableTime)  // Найшвидше доступний
                .ThenBy(m => m.TotalWorkload)       // Найменше навантаження
                .FirstOrDefault();

            // Якщо знайдено механіка, повертаємо його
            return availableMechanic?.Mechanic;
        }

        private static DateTime GetNextAvailableTime(Mechanic mechanic, DateTime plannedStartDate)
        {
            // Отримуємо всі завдання механіка, які ще активні після вказаної дати
            var activeTasks = mechanic.MechanicsTasks
                .Where(t => t.FinishDate == null || t.FinishDate > plannedStartDate)
                .OrderBy(t => t.IssueDate)
                .ToList();

            // Якщо немає активних завдань, механік доступний на заплановану дату
            if (!activeTasks.Any())
                return plannedStartDate;

            // Розраховуємо найближчий доступний момент після завершення останнього завдання
            DateTime lastTaskFinish = activeTasks
                .Max(t => t.FinishDate ?? t.IssueDate.AddHours(1)); // Якщо FinishDate немає, припускаємо 1 годину тривалості
            return lastTaskFinish > plannedStartDate ? lastTaskFinish : plannedStartDate;
        }
    }

}
