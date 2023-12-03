using BookStore.Admin.Context;
using BookStore.Admin.Entity;
using BookStore.Admin.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace BookStore.Admin.Services
{
    public class AdminRL : IAdmin
    {
        private readonly AdminDbContext adminDbContext;
        private readonly IConfiguration configuration;

        public AdminRL(AdminDbContext adminDbContext, IConfiguration configuration)
        {
            this.adminDbContext = adminDbContext;
            this.configuration = configuration;
        }
        /// <summary>
        /// Registers a new admin in the system.
        /// </summary>
        /// <param name="adminEntity">The admin entity containing registration details.</param>
        /// <returns>The registered admin entity if successful, otherwise null.</returns>
        public AdminEntity AdminRegistration(AdminEntity adminEntity)
        {
            try
            {
                AdminEntity admin = new AdminEntity();
                admin.FullName = adminEntity.FullName;
                admin.Email = adminEntity.Email;
                admin.Password = adminEntity.Password;
                admin.MobileNumber = adminEntity.MobileNumber;
                admin.CreatedDate = adminEntity.CreatedDate;
                admin.UpdatedDate = adminEntity.UpdatedDate;
                adminDbContext.AdminTable.Add(admin);
                int result = adminDbContext.SaveChanges();
                if (result > 0)
                {
                    return admin;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("An unexpected error occurred during registration.", ex);
            }

        }


        /// <summary>
        /// logins the admin into the system
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>The login details  if successful, otherwise null</returns>
        /// <exception cref="Exception"></exception>
        public AdminEntity AdminLogin(string email, string password)
        {
            try
            {
                AdminEntity admin = adminDbContext.AdminTable.FirstOrDefault(a => a.Email == email && a.Password == password);
                if(admin != null)
                {
                    return admin;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("An unexpected error occurred during login.", ex);
            }
        }
    }
}
