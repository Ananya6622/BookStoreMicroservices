using BookStore.User.Context;
using BookStore.User.Entity;
using BookStore.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BookStore.User.Services
{
    public class UserRL: IUser
    {
        private readonly UserDbContext userDbContext;
        private readonly IConfiguration configuration;
        public UserRL(UserDbContext userDbContext, IConfiguration configuration)
        {
            this.userDbContext = userDbContext;
            this.configuration = configuration;
        }
        /// <summary>
        /// User Registration method to register a new user.
        /// </summary>
        /// <param name="userEntity">The user entity containing registration details.</param>
        /// <returns>The registered user entity if successful, otherwise null.</returns>
        public UserEntity UserRegistration(UserEntity userEntity)
        {
            try
            {
                UserEntity user = new UserEntity();
                user.FullName = userEntity.FullName;
                user.Email = userEntity.Email;
                user.Password = EncryptPass(userEntity.Password);
                user.MobileNumber = userEntity.MobileNumber;
                user.CreatedDate = userEntity.CreatedDate;
                user.UpdatedDate = userEntity.UpdatedDate;
                user.Address = userEntity.Address;
                user.City = userEntity.City;
                user.State = userEntity.State;
                user.PinCode = userEntity.PinCode;
                userDbContext.UserTable.Add(user);
                int result = userDbContext.SaveChanges();
                if (result > 0)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred during user registration.", ex);
            }
        }
        /// <summary>
        /// Encrypts the user's password.
        /// </summary>
        /// <param name="password">The password to be encrypted.</param>
        /// <returns>The encrypted password.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the encryption process.</exception>
        public static string EncryptPass(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="userId">The user's unique identifier.</param>
        /// <returns>The generated JWT token.</returns>
        public string GenerateToken(string Email, int UserId)
        {
            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
               new Claim("Email", Email),
               new Claim("UserId", UserId.ToString())
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        /// <summary>
        /// Authenticates a user and returns a JWT token upon successful login.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The JWT token if login is successful, otherwise null.</returns>
        public string UserLogin(string email, string password)
        {
            try
            {
                var encodedPassword = EncryptPass(password);
                UserEntity user = userDbContext.UserTable.FirstOrDefault(a => a.Email == email && a.Password == encodedPassword);
                if (user != null)
                {
                    var token = this.GenerateToken(user.Email, user.UserId);
                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred during user login.", ex);
            }
        }
        /// <summary>
        /// Retrieves the profile information for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The user's profile information if found, otherwise null.</returns>
        public UserEntity MyProfile(int userId)
        {
            try
            {
                UserEntity user = userDbContext.UserTable.FirstOrDefault(a => a.UserId == userId);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error occurred while retrieving user profile.", ex);
            }
        }
    }
}
