using RPayroll.Domain.Entities;
using RPayroll.Infrastructure.Repositories.Interfaces;

namespace RPayroll.Infrastructure.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    public Task<User?> GetByIdAsync(int id)
    {
        var user = InMemoryDataStore.Users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        var user = InMemoryDataStore.Users.FirstOrDefault(u =>
            string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task<List<User>> GetAllAsync()
    {
        return Task.FromResult(InMemoryDataStore.Users.ToList());
    }

    public Task AddAsync(User user)
    {
        EnsureRole(user);
        user.Id = InMemoryDataStore.NextUserId();
        InMemoryDataStore.Users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user)
    {
        var index = InMemoryDataStore.Users.FindIndex(u => u.Id == user.Id);
        if (index >= 0)
        {
            EnsureRole(user);
            InMemoryDataStore.Users[index] = user;
        }
        return Task.CompletedTask;
    }

    private static void EnsureRole(User user)
    {
        if (user.Role == null && user.RoleId == 0)
        {
            return;
        }

        if (user.Role == null)
        {
            user.Role = InMemoryDataStore.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            return;
        }

        if (user.Role.Id == 0)
        {
            var existing = InMemoryDataStore.Roles.FirstOrDefault(r =>
                string.Equals(r.Name, user.Role.Name, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                user.RoleId = existing.Id;
                user.Role = existing;
                return;
            }

            user.Role.Id = InMemoryDataStore.NextRoleId();
            InMemoryDataStore.Roles.Add(user.Role);
            user.RoleId = user.Role.Id;
        }
    }
}
