using IDENTITY.BLL.Services.Interfaces;
using IDENTITY.DAL.Data;
using IDENTITY.DAL.Entities;
using IDENTITY.DAL.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IDENTITY.BLL.Services
{
    public class RoleService : IRoleService
    {


        public RoleManager<Role> _RoleManager;
        public UserManager<User> _UserManager;
        private readonly AppDBContext dbContext;

        public RoleService(RoleManager<Role> roleManager, UserManager<User> userManager, AppDBContext dbContext)
        {
            _RoleManager = roleManager;
            _UserManager = userManager;
            this.dbContext = dbContext;
        }
        public async Task AssignRole(Guid id, string roleName)
        {
            try
            {
                var user = await _UserManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var role = await _RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    throw new EntityNotFoundException($"Role {roleName} not found");
                }

                // Присвоїти роль користувачеві
                await _UserManager.AddToRoleAsync(user, roleName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task UnAssignRole(Guid id, string roleName)
        {
            try
            {
                var user = await _UserManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var role = await _RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    throw new EntityNotFoundException($"Role {roleName} not found");

                }

                // Присвоїти роль користувачеві
                await _UserManager.RemoveFromRoleAsync(user, roleName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {

            try
            {
                return await _RoleManager.Roles.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public async Task CreateRole(string name)
        {
            try
            {
                var role = new Role(name);
                await _RoleManager.CreateAsync(role);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task UpdateRole(string name)
        {
            try
            {
                var role = new Role(name);
                await _RoleManager.CreateAsync(role);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteRole(string name)
        {
            try
            {
                var role = new Role(name);
                await _RoleManager.CreateAsync(role);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<string>> GetRolesForUserAsync(Guid id)
        {

            try
            {


                var user = await _UserManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    throw new EntityNotFoundException($"user with id {id} not found");
                }

                return await _UserManager.GetRolesAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task ReAssignRole(Guid id, string newrole)
        {
            var user = await _UserManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var role = await _RoleManager.FindByNameAsync(newrole);
            if (role == null)
            {
                throw new EntityNotFoundException($"Role {newrole} not found");
            }

            var oldRole = await _UserManager.GetRolesAsync(user);


            await _UserManager.RemoveFromRoleAsync(user, oldRole.First());

            await _UserManager.AddToRoleAsync(user, newrole);
            
            await dbContext.SaveChangesAsync();

        }
    }
}