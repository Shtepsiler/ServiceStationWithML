using IDENTITY.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDENTITY.DAL.Seeding
{
    public class Seed
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var loger = serviceProvider.GetRequiredService<ILogger<Seed>>();
            try
            {
                // Seed roles if needed
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new Role("Admin"));
                }
                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await roleManager.CreateAsync(new Role("User"));
                }
                if (!await roleManager.RoleExistsAsync("Mechanic"))
                {
                    await roleManager.CreateAsync(new Role("Mechanic"));
                }

                // Seed default user
                var defaultUser1 = await userManager.FindByEmailAsync("broccolicodeman.shopoyisty@gmail.com");
                if (defaultUser1 == null)
                {
                    defaultUser1 = new User
                    {
                        UserName = "broccolicodeman",
                        Email = "broccolicodeman.shopoyisty@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(defaultUser1, ",Hjrjksrjlvfy8"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(defaultUser1, "Admin");
                    }
                }

                var defaultUser3 = await userManager.FindByEmailAsync("ricecodeman.shopoyisty@gmail.com");
                if (defaultUser3 == null)
                {
                    defaultUser3 = new User
                    {
                        UserName = "ricecodeman",
                        Email = "ricecodeman.shopoyisty@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(defaultUser3, ",Hjrjksrjlvfy8"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(defaultUser3, "User");
                    }
                }







                var mechanicuser1 = await userManager.FindByEmailAsync("mechanic1@gmail.com");
                if (mechanicuser1 == null)
                {
                    mechanicuser1 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a1"),
                        UserName = "Air Conditioning Mechanic",
                        Email = "mechanic1@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser1, "amaMechanic@1"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser1, "Mechanic");
                    }
                }
                var mechanicuser2 = await userManager.FindByEmailAsync("mechanic2@gmail.com");
                if (mechanicuser2 == null)
                {
                    mechanicuser2 = new User
                    {
                        Id = Guid.Parse("88c2a122-9e71-4a7a-a52d-9f82a6610d87"),
                        UserName = "Body Repair Mechanic",
                        Email = "mechanic2@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser2, "amaMechanic@2"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser2, "Mechanic");
                    }
                }
                var mechanicuser3 = await userManager.FindByEmailAsync("mechanic3@gmail.com");
                if (mechanicuser3 == null)
                {
                    mechanicuser3 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a3"),
                        UserName = "Brake System Mechanic",
                        Email = "mechanic3@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser3, "amaMechanic@3"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser3, "Mechanic");
                    }
                }
                var mechanicuser4 = await userManager.FindByEmailAsync("mechanic4@gmail.com");
                if (mechanicuser4 == null)
                {
                    mechanicuser4 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a4"),
                        UserName = "Cooling System Mechanic",
                        Email = "mechanic4@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser4, "amaMechanic@4"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser4, "Mechanic");
                    }
                }
                var mechanicuser5 = await userManager.FindByEmailAsync("mechanic5@gmail.com");
                if (mechanicuser5 == null)
                {
                    mechanicuser5 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a5"),
                        UserName = "Diagnostics Mechanic",
                        Email = "mechanic5@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser5, "amaMechanic@5"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser5, "Mechanic");
                    }
                }
                var mechanicuser6 = await userManager.FindByEmailAsync("mechanic6@gmail.com");
                if (mechanicuser6 == null)
                {
                    mechanicuser6 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a6"),
                        UserName = " Electric Vehicle Mechanic",
                        Email = "mechanic6@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser6, "amaMechanic@6"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser6, "Mechanic");
                    }
                }
                var mechanicuser7 = await userManager.FindByEmailAsync("mechanic7@gmail.com");
                if (mechanicuser7 == null)
                {
                    mechanicuser7 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a7"),
                        UserName = "Electrical Mechanic",
                        Email = "mechanic7@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser7, "amaMechanic@7"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser7, "Mechanic");
                    }
                }
                var mechanicuser8 = await userManager.FindByEmailAsync("mechanic8@gmail.com");
                if (mechanicuser8 == null)
                {
                    mechanicuser8 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a8"),
                        UserName = "Engine Mechanic",
                        Email = "mechanic8@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser8, "amaMechanic@8"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser8, "Mechanic");
                    }
                }
                var mechanicuser9 = await userManager.FindByEmailAsync("mechanic9@gmail.com");
                if (mechanicuser9 == null)
                {
                    mechanicuser9 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a9"),
                        UserName = "Exhaust System Mechanic",
                        Email = "mechanic9@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser9, "amaMechanic@9"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser9, "Mechanic");
                    }
                }
                var mechanicuser10 = await userManager.FindByEmailAsync("mechanic10@gmail.com");
                if (mechanicuser10 == null)
                {
                    mechanicuser10 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b996510"),
                        UserName = "Fuel System Mechanic",
                        Email = "mechanic10@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser10, "amaMechanic@10"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser10, "Mechanic");
                    }
                }
                var mechanicuser11 = await userManager.FindByEmailAsync("mechanic11@gmail.com");
                if (mechanicuser11 == null)
                {
                    mechanicuser11 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b996511"),
                        UserName = "Safety Systems Mechanic ABS, ESP",
                        Email = "mechanic11@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser11, "amaMechanic@11"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser11, "Mechanic");
                    }
                }
                var mechanicuser12 = await userManager.FindByEmailAsync("mechanic12@gmail.com");
                if (mechanicuser12 == null)
                {
                    mechanicuser12 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b996512"),
                        UserName = "Steering Mechanic",
                        Email = "mechanic12@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser12, "amaMechanic@12"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser12, "Mechanic");
                    }
                }
                var mechanicuser13 = await userManager.FindByEmailAsync("mechanic13@gmail.com");
                if (mechanicuser13 == null)
                {
                    mechanicuser13 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b996513"),
                        UserName = "Suspension Mechanic",
                        Email = "mechanic13@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser13, "amaMechanic@13"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser13, "Mechanic");
                    }
                }
                var mechanicuser14 = await userManager.FindByEmailAsync("mechanic14@gmail.com");
                if (mechanicuser14 == null)
                {
                    mechanicuser14 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b996514"),
                        UserName = "Tire and Wheel Mechanic",
                        Email = "mechanic14@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser14, "amaMechanic@14"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser14, "Mechanic");
                    }
                }
                var mechanicuser15 = await userManager.FindByEmailAsync("mechanic15@gmail.com");
                if (mechanicuser15 == null)
                {
                    mechanicuser15 = new User
                    {
                        Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b996515"),
                        UserName = "Transmission Mechanic",
                        Email = "mechanic15@gmail.com",
                        EmailConfirmed = true
                        // Add any additional properties here
                    };
                    var result = await userManager.CreateAsync(mechanicuser15, "amaMechanic@15"); // Change the password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(mechanicuser15, "Mechanic");
                    }
                }
            }
            catch (Exception ex)
            {
                loger.Log(LogLevel.Warning, ex.ToString());
            }
        }
    }
}
