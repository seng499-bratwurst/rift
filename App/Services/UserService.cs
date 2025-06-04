using Rift.Models;

public class UserService
{
    private readonly ApplicationDbContext _dbContext;

    public UserService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> RegisterUserAsync(User data)
    {
        _dbContext.Users.Add(data);
        await _dbContext.SaveChangesAsync();
        return data;
    }

    public string GatherMessageHistoryAsync(User user)
    {
        throw new NotImplementedException("Gathering message history is not implemented yet.");
    }
}