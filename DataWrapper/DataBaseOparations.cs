using Microsoft.EntityFrameworkCore;
using OpenX.Data;
using OpenX.Models;

namespace OpenX.DataWrapper
{
    public class DataBaseOparations (AppDbContext dbContext)
    {
        private readonly AppDbContext context = dbContext;
         
        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await context.Users.AnyAsync(u => u.UserName == userName).ConfigureAwait(false); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DataBaseOparations: UserExists(): {ex.Message}, {ex.StackTrace}");
                throw; 
            }
        }

        public async Task CreateUser(UserDetails user)
        {
            try
            {
                context.Users.Add(user);
                await context.SaveChangesAsync().ConfigureAwait(false); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DataBaseOparations: CreateUser(): {ex.Message}, {ex.StackTrace}");
            }
        } 

        public async Task<UserDetails?> GetUserByUsername(string userName)
        {
            try
            {
                return await context.Users.FirstOrDefaultAsync(u => u.UserName == userName).ConfigureAwait(false); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DataBaseOparations: GetUserByUsername(): {ex.Message}, {ex.StackTrace}");
                throw;
            }
        }

    }
}
