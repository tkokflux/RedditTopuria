using Reddit.Models;

namespace Reddit.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(int pageNumber = 1, int pageSize = 1000, string? sortKey = null, bool? isAscending = true, string? searchKey=null);

    }
}
