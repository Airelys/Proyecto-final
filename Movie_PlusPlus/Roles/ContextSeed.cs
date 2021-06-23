using Microsoft.AspNetCore.Identity;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_PlusPlus.Roles
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, 
                                                RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Ticket_Agent.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic_User.ToString()));
        }
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, 
                                                RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                FirstName = "Admin First Name",
                LastName = "Admin Last Name",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Admin123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic_User.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Manager.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Ticket_Agent.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }

            }
        }
        public static async Task SeedNormalUserType(ApplicationDbContext _context)
        {
            if (_context.UserType.Count() == 0)
            {
                var _normalUser = new UserType() { Type = "Normal User", Discount = 0 };
                await _context.AddAsync(_normalUser);
                await _context.SaveChangesAsync();
            }
             
        }

        public static async Task SeedMovies(ApplicationDbContext _context)
        {
            var _Movie1 = new Movie()
            {
                Title = "Pelicula 1",
                Actors = "Actores 1",
                Country = "Pais 1",
                Duration = 10,
                KindOfMovie = "Genero 1",
                PropagandisticAndEconomics = true
            };
            if (!_context.Movie.Any(m => m.Title == _Movie1.Title))
            {
                await _context.AddAsync(_Movie1);
                await _context.SaveChangesAsync();
            }
            

            var _Movie2 = new Movie()
            {
                Title = "Pelicula 2",
                Actors = "Actores 2",
                Country = "Pais 2",
                Duration = 100,
                KindOfMovie = "Genero 2",
                PropagandisticAndEconomics = false
            };
            if (!_context.Movie.Any(m => m.Title == _Movie2.Title))
            {
                await _context.AddAsync(_Movie2);
                await _context.SaveChangesAsync();
            }

            var _Movie3 = new Movie()
            {
                Title = "Pelicula 3",
                Actors = "Actores 3",
                Country = "Pais 3",
                Duration = 150,
                KindOfMovie = "Genero 3",
                PropagandisticAndEconomics = false
            };
            if (!_context.Movie.Any(m => m.Title == _Movie3.Title))
            {
                await _context.AddAsync(_Movie3);
                await _context.SaveChangesAsync();
            }

            var _Movie4 = new Movie()
            {
                Title = "Pelicula 4",
                Actors = "Actores 4",
                Country = "Pais 4",
                Duration = 150,
                KindOfMovie = "Genero 4",
                PropagandisticAndEconomics = false
            };
            if(!_context.Movie.Any(m => m.Title == _Movie4.Title))
            {
                await _context.AddAsync(_Movie4);
                await _context.SaveChangesAsync();
            }

            var _Movie5 = new Movie()
            {
                Title = "Pelicula 5",
                Actors = "Actores 5",
                Country = "Pais 5",
                Duration = 150,
                KindOfMovie = "Genero 5",
                PropagandisticAndEconomics = true
            };
            if (!_context.Movie.Any(m => m.Title == _Movie5.Title))
            {
                await _context.AddAsync(_Movie5);
                await _context.SaveChangesAsync();
            }
        }

        public static async Task SeedMovie_Local(ApplicationDbContext _context)
        {
            var _Movie_local1 = new Movie_Local()
            {
                Local_Name = "Sala 1",
                Columns = 10,
                Rows = 10
            };
            if (!_context.Movie_Local.Any(m => m.Local_Name == _Movie_local1.Local_Name)) 
            {
                await _context.AddAsync(_Movie_local1);
                await _context.SaveChangesAsync();
            }
            

            var _Movie_local2 = new Movie_Local()
            {
                Local_Name = "Sala 2",
                Columns = 5,
                Rows = 8
            };
            if (!_context.Movie_Local.Any(m => m.Local_Name == _Movie_local2.Local_Name))
            {
                await _context.AddAsync(_Movie_local2);
                await _context.SaveChangesAsync();
            }

            var _Movie_local3 = new Movie_Local()
            {
                Local_Name = "Sala 3",
                Columns = 5,
                Rows = 10
            };
            if (!_context.Movie_Local.Any(m => m.Local_Name == _Movie_local3.Local_Name))
            {
                await _context.AddAsync(_Movie_local3);
                await _context.SaveChangesAsync();
            }
        }

        public static async Task SeedUsers(UserManager<ApplicationUser> userManager,
                                                RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var manager1 = new ApplicationUser
            {
                UserName = "manager1",
                Email = "manager1@gmail.com",
                FirstName = "Manager1 First Name",
                LastName = "Manager1 Last Name",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != manager1.Id))
            {
                var user = await userManager.FindByEmailAsync(manager1.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(manager1, "Admin123!");
                    await userManager.AddToRoleAsync(manager1, Roles.Manager.ToString());
                }

            }

            var manager2 = new ApplicationUser
            {
                UserName = "manager2",
                Email = "manager2@gmail.com",
                FirstName = "Manager2 First Name",
                LastName = "Manager2 Last Name",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != manager2.Id))
            {
                var user = await userManager.FindByEmailAsync(manager2.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(manager2, "Admin123!");
                    await userManager.AddToRoleAsync(manager2, Roles.Manager.ToString());
                }

            }

            var tickect_agent1 = new ApplicationUser
            {
                UserName = "ticketagent1",
                Email = "ticketagent1@gmail.com",
                FirstName = "ticketagent1 First Name",
                LastName = "ticketagent1 Last Name",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != tickect_agent1.Id))
            {
                var user = await userManager.FindByEmailAsync(tickect_agent1.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(tickect_agent1, "Admin123!");
                    await userManager.AddToRoleAsync(tickect_agent1, Roles.Ticket_Agent.ToString());
                }

            }

            var tickect_agent2 = new ApplicationUser
            {
                UserName = "ticketagent2",
                Email = "ticketagent2@gmail.com",
                FirstName = "ticketagent2 First Name",
                LastName = "ticketagent2 Last Name",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != tickect_agent2.Id))
            {
                var user = await userManager.FindByEmailAsync(tickect_agent2.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(tickect_agent2, "Admin123!");
                    await userManager.AddToRoleAsync(tickect_agent2, Roles.Ticket_Agent.ToString());
                }

            }

            var user1 = new ApplicationUser
            {
                UserName = "user1",
                Email = "user1@gmail.com",
                FirstName = "user1 First Name",
                LastName = "user1 Last Name",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != user1.Id))
            {
                var user = await userManager.FindByEmailAsync(user1.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(user1, "Admin123!");
                    await userManager.AddToRoleAsync(user1, Roles.Basic_User.ToString());
                }

            }

            var user2 = new ApplicationUser
            {
                UserName = "user2",
                Email = "user2@gmail.com",
                FirstName = "user2 First Name",
                LastName = "user2 Last Name",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != user2.Id))
            {
                var user = await userManager.FindByEmailAsync(user2.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(user2, "Admin123!");
                    await userManager.AddToRoleAsync(user2, Roles.Basic_User.ToString());
                }

            }

            var user3 = new ApplicationUser
            {
                UserName = "user3",
                Email = "user3@gmail.com",
                FirstName = "user3 First Name",
                LastName = "user3 Last Name",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != user3.Id))
            {
                var user = await userManager.FindByEmailAsync(user3.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(user3, "Admin123!");
                    await userManager.AddToRoleAsync(user3, Roles.Basic_User.ToString());
                }

            }

            var user4 = new ApplicationUser
            {
                UserName = "user4",
                Email = "user4@gmail.com",
                FirstName = "user4 First Name",
                LastName = "user4 Last Name",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != user4.Id))
            {
                var user = await userManager.FindByEmailAsync(user4.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(user4, "Admin123!");
                    await userManager.AddToRoleAsync(user4, Roles.Basic_User.ToString());
                }

            }

            var user5 = new ApplicationUser
            {
                UserName = "user5",
                Email = "user5@gmail.com",
                FirstName = "user5 First Name",
                LastName = "user5 Last Name",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != user5.Id))
            {
                var user = await userManager.FindByEmailAsync(user5.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(user5, "Admin123!");
                    await userManager.AddToRoleAsync(user5, Roles.Basic_User.ToString());
                }

            }
        }
    }
}
