using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JOBS.DAL.Seeding
{
    public  class Seed
    {
        // Define static IDs for Jobs and Tasks
        private static readonly Guid MechanicId = Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a1");
        private static readonly Guid ModelId1 = Guid.Parse("88C2A122-9E71-4A7A-A52D-9F82A6610D87");
        private static readonly Guid ModelId2 = Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a3");
        private static readonly Guid ModelId3 = Guid.Parse("dc238098-d210-44f3-778e-08dc7b9965a3");

        private static readonly Guid JobId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private static readonly Guid JobId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
        private static readonly Guid JobId3 = Guid.Parse("33333333-3333-3333-3333-333333333333");

        private static readonly Guid TaskId1Job1 = Guid.Parse("44444444-4444-4444-4444-444444444444");
        private static readonly Guid TaskId2Job1 = Guid.Parse("55555555-5555-5555-5555-555555555555");
        private static readonly Guid TaskId3Job1 = Guid.Parse("66666666-6666-6666-6666-666666666666");

        private static readonly Guid TaskId1Job2 = Guid.Parse("77777777-7777-7777-7777-777777777777");
        private static readonly Guid TaskId2Job2 = Guid.Parse("88888888-8888-8888-8888-888888888888");
        private static readonly Guid TaskId3Job2 = Guid.Parse("99999999-9999-9999-9999-999999999999");

        private static readonly Guid TaskId1Job3 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        private static readonly Guid TaskId2Job3 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        private static readonly Guid TaskId3Job3 = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ServiceStationDBContext>();
            var loger = serviceProvider.GetRequiredService<ILogger<Seed>>();

            void addSetc(string spec)
            {
                try
                {
                    var sp = context.Specialisations.Where(p => p.Name == spec).FirstOrDefault();
                    if (sp == null)
                    {
                        Specialisation specialisation = new Specialisation();
                        specialisation.Name = spec;
                        context.Specialisations.Add(specialisation);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    loger.Log(LogLevel.Warning, ex.ToString());
                }
            }
            var mechanicSpecializations = new List<string>
                {
                    "Air Conditioning Mechanic",
                    "Body Repair Mechanic",
                    "Brake System Mechanic",
                    "Cooling System Mechanic",
                    "Diagnostics Mechanic",
                    "Electric Vehicle Mechanic",
                    "Electrical Mechanic",
                    "Engine Mechanic",
                    "Exhaust System Mechanic",
                    "Fuel System Mechanic",
                    "Safety Systems Mechanic (ABS, ESP)",
                    "Steering Mechanic",
                    "Suspension Mechanic",
                    "Tire and Wheel Mechanic",
                    "Transmission Mechanic"
                };


           foreach (string spec in mechanicSpecializations)
                { addSetc(spec); }


            var initialMechanicIds = new List<string>
                {
                    "dc238098-d410-44f3-778e-08dc7b9965a1",
                    "dc238098-d410-44f3-778e-08dc7b9965a2",
                    "dc238098-d410-44f3-778e-08dc7b9965a3",
                    "dc238098-d410-44f3-778e-08dc7b9965a4",
                    "dc238098-d410-44f3-778e-08dc7b9965a5",
                    "dc238098-d410-44f3-778e-08dc7b9965a6",
                    "dc238098-d410-44f3-778e-08dc7b9965a7",
                    "dc238098-d410-44f3-778e-08dc7b9965a8",
                    "dc238098-d410-44f3-778e-08dc7b9965a9",
                    "dc238098-d410-44f3-778e-08dc7b996510",
                    "dc238098-d410-44f3-778e-08dc7b996511",
                    "dc238098-d410-44f3-778e-08dc7b996512",
                    "dc238098-d410-44f3-778e-08dc7b996513",
                    "dc238098-d410-44f3-778e-08dc7b996514",
                    "dc238098-d410-44f3-778e-08dc7b996515"
                };

            void addmech(string id, string spec)
            {
                try
                {
                    var sp = context.Mechanics.Where(p => p.MechanicId == Guid.Parse(id)).FirstOrDefault();
                    if (sp == null)
                    {
                        Specialisation? specialisation = context.Specialisations.Where(p => p.Name == spec).FirstOrDefault();
                        if (specialisation != null)
                        {
                            Mechanic mechanic = new Mechanic();

                            mechanic.MechanicId = Guid.Parse(id);
                            mechanic.Specialisation = specialisation;
                            context.Mechanics.Add(mechanic);
                        }

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    loger.Log(LogLevel.Warning, ex.ToString());
                }
            }
            for (int i = 0; i < initialMechanicIds.Count; i++)
                addmech(initialMechanicIds[i], mechanicSpecializations[i]);











            if (context.Jobs.Any())
            {
                return;   // DB has been seeded
            }
            // Додаємо більше робіт
            var jobs = new List<Job>
    {
        new Job
        {
            Id = JobId1,
            VehicleId = ModelId1,
            IssueDate = DateTime.Now,
            Status = "NewJob",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a1")).First(),
            ClientId = Guid.Parse("A8688413-F6F9-41A2-8CE6-08DCFEFD3F67"),
            Description = "Test issue 1",
            ModelConfidence = 1.0f,
            ModelAproved = true
        },
        new Job
        {
            Id = JobId2,
            VehicleId = ModelId2,
            IssueDate = DateTime.Now,
            Status = "NewJob",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a5")).First(),
            ClientId = Guid.Parse("A8688413-F6F9-41A2-8CE6-08DCFEFD3F67"),
            Description = "Test issue 2",
            ModelConfidence = 0.5f,
            ModelAproved = false
        },
        new Job
        {
            Id = JobId3,
            VehicleId = ModelId3,
            IssueDate = DateTime.Now,
            Status = "NewJob",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a7")).First(),
            ClientId = Guid.Parse("A8688413-F6F9-41A2-8CE6-08DCFEFD3F67"),
            Description = "Test issue 3",
            ModelConfidence = 0.1f,
            ModelAproved = false
        },
        // Додаємо ще кілька робіт
        new Job
        {
            Id = Guid.NewGuid(),
            VehicleId = ModelId1,
            IssueDate = DateTime.Now,
            Status = "NewJob",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a3")).First(),
            ClientId = Guid.Parse("A8688413-F6F9-41A2-8CE6-08DCFEFD3F67"),
            Description = "Test issue 4",
            ModelConfidence = 0.7f,
            ModelAproved = true
        },
        new Job
        {
            Id = Guid.NewGuid(),
            VehicleId = ModelId2,
            IssueDate = DateTime.Now,
            Status = "NewJob",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a9")).First(),
            ClientId = Guid.Parse("A8688413-F6F9-41A2-8CE6-08DCFEFD3F67"),
            Description = "Test issue 5",
            ModelConfidence = 0.8f,
            ModelAproved = true
        },
        new Job
        {
            Id = Guid.NewGuid(),
            VehicleId = ModelId3,
            IssueDate = DateTime.Now,
            Status = "NewJob",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a2")).First(),
            ClientId = Guid.Parse("A8688413-F6F9-41A2-8CE6-08DCFEFD3F67"),
            Description = "Test issue 6",
            ModelConfidence = 0.9f,
            ModelAproved = false
        }
    };

            // Додаємо роботи в контекст
            context.Jobs.AddRange(jobs);
            await context.SaveChangesAsync();

            // Створення списку завдань для кожної роботи
            var tasks = new List<MechanicsTasks>
    {
        new MechanicsTasks
        {
            Id = TaskId1Job1,
            JobId = JobId1,
            Name = "Task 1 for Job 1",
            IssueDate = DateTime.Now,
            Task = "Task description 1",
            Status = "Pending",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a6")).First()
        },
        new MechanicsTasks
        {
            Id = TaskId2Job1,
            JobId = JobId1,
            Name = "Task 2 for Job 1",
            IssueDate = DateTime.Now,
            Task = "Task description 2",
            Status = "Pending",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b996512")).First()
        },
        new MechanicsTasks
        {
            Id = TaskId3Job1,
            JobId = JobId1,
            Name = "Task 3 for Job 1",
            IssueDate = DateTime.Now,
            Task = "Task description 3",
            Status = "Pending",
            Mechanic =context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b996514")).First()
        },
        new MechanicsTasks
        {
            Id = TaskId1Job2,
            JobId = JobId2,
            Name = "Task 1 for Job 2",
            IssueDate = DateTime.Now,
            Task = "Task description 1",
            Status = "Pending",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b996515")).First()
        },
        new MechanicsTasks
        {
            Id = TaskId2Job2,
            JobId = JobId2,
            Name = "Task 2 for Job 2",
            IssueDate = DateTime.Now,
            Task = "Task description 2",
            Status = "Pending",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b996511")).First()
        },
        new MechanicsTasks
        {
            Id = TaskId3Job2,
            JobId = JobId2,
            Name = "Task 3 for Job 2",
            IssueDate = DateTime.Now,
            Task = "Task description 3",
            Status = "Pending",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b996510")).First()
        },
        new MechanicsTasks
        {
            Id = TaskId1Job3,
            JobId = JobId3,
            Name = "Task 1 for Job 3",
            IssueDate = DateTime.Now,
            Task = "Task description 1",
            Status = "Pending",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a8")).First()
        },
        new MechanicsTasks
        {
            Id = TaskId2Job3,
            JobId = JobId3,
            Name = "Task 2 for Job 3",
            IssueDate = DateTime.Now,
            Task = "Task description 2",
            Status = "Pending",
            Mechanic =context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a5")).First()
        },
        new MechanicsTasks
        {
            Id = TaskId3Job3,
            JobId = JobId3,
            Name = "Task 3 for Job 3",
            IssueDate = DateTime.Now,
            Task = "Task description 3",
            Status = "Pending",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a1")).First()
        },
        new MechanicsTasks
        {
            Id = Guid.NewGuid(),
            JobId = JobId1,
            Name = "Task 1 for Job 4",
            IssueDate = DateTime.Now,
            Task = "Task description 4",
            Status = "Pending",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a7")).First()
        },
        new MechanicsTasks
        {
            Id = Guid.NewGuid(),
            JobId = JobId2,
            Name = "Task 1 for Job 5",
            IssueDate = DateTime.Now,
            Task = "Task description 5",
            Status = "Pending",
            Mechanic = context.Mechanics.Include(p=>p.Specialisation).Where(p=>p.MechanicId == Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a1")).First()
        }
    };

            // Додаємо завдання в контекст
            context.MechanicsTasks.AddRange(tasks);
            await context.SaveChangesAsync();

            // Призначаємо завдання до робіт
            context.Jobs.Where(x => x.Id == JobId1).FirstOrDefault().Tasks = context.MechanicsTasks.Where(x => x.JobId == JobId1).ToList();
            context.Jobs.Where(x => x.Id == JobId2).FirstOrDefault().Tasks = context.MechanicsTasks.Where(x => x.JobId == JobId2).ToList();
            context.Jobs.Where(x => x.Id == JobId3).FirstOrDefault().Tasks = context.MechanicsTasks.Where(x => x.JobId == JobId3).ToList();

            // Зберігаємо зміни
            await context.SaveChangesAsync();
        }
    }
}
