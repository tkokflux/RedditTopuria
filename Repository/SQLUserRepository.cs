using Microsoft.EntityFrameworkCore;
using Reddit.Models;

namespace Reddit.Repository
{
    public class SQLUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SQLUserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<User>> GetAllAsync(int pageNumber = 1, int pageSize = 1000, string? sortKey = null, bool? isAscending = true, string? searchKey = null)
        {
            var users = dbContext.Users.AsQueryable();


            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                users = users.Where(u =>
                    u.Name.Contains(searchKey) ||
                    u.Email.Contains(searchKey));
            }



            if (!string.IsNullOrWhiteSpace(sortKey))
            {
                users = sortKey.ToLower() switch
                {
                    "numberofposts" => isAscending ?? true
                        ? users.OrderBy(u => u.Posts.Count)
                        : users.OrderByDescending(u => u.Posts.Count),
                    "id" => isAscending ?? true
                        ? users.OrderBy(u => u.Id)
                        : users.OrderByDescending(u => u.Id),
                    _ => users
                };
            }


            var skipResults = (pageNumber - 1) * pageSize;
            return await users.Skip(skipResults).Take(pageSize).ToListAsync();
        }
    }
}
