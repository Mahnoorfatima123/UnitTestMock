using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;
using Xunit;

public class UserRepositoryTests
{
    private readonly UserRepository _userRepository;
    private readonly AppDbContext _context;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new AppDbContext(options);
        _userRepository = new UserRepository(_context);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnUsers()
    {
        // Arrange
        _context.Users.Add(new User { Id = 1, Name = "John Doe" });
        _context.Users.Add(new User { Id = 2, Name = "Jane Doe" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _userRepository.GetAllUsersAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser()
    {
        // Arrange
        _context.Users.Add(new User { Id = 1, Name = "Alice" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _userRepository.GetUserByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Alice", result.Name);
    }

    [Fact]
    public async Task AddUserAsync_ShouldAddUser()
    {
        // Arrange
        var user = new User { Id = 3, Name = "Bob" };

        // Act
        await _userRepository.AddUserAsync(user);

        // Assert
        var result = await _context.Users.FindAsync(3);
        Assert.NotNull(result);
        Assert.Equal("Bob", result.Name);
    }
}
