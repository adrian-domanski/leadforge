using FluentAssertions;
using LeadForge.Application;
using LeadForge.Application.Interfaces;
using LeadForge.Domain;
using LeadForge.Domain.Enums;
using LeadForge.Domain.Exceptions;
using LeadForge.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace LeadForge.UnitTests.Services;

public class GenerationServiceTests
{
   [Fact]
   public async Task GenerateAsync_Should_Throw_When_User_Has_No_Credits()
   {
      // Arrange

      var options = new DbContextOptionsBuilder<AppDbContext>()
         .UseInMemoryDatabase(Guid.NewGuid().ToString())
         .Options;

      var db = new AppDbContext(options);

      var userId = Guid.NewGuid();

      db.Users.Add(new User
      {
         Id = userId,
         Email = "test@test.com",
         PasswordHash = "fake-hash",
         Credits = 0
      });

      await db.SaveChangesAsync();

      var openAiMock = new Mock<IOpenAiService>();
      var currentUserMock = new Mock<ICurrentUserService>();
      var loggerMock = new Mock<ILogger<GenerationService>>();

      currentUserMock
         .Setup(x => x.GetUserId())
         .Returns(userId);

      var service = new GenerationService(
         db,
         openAiMock.Object,
         currentUserMock.Object,
         loggerMock.Object
      );

      var request = new GeneratePostRequest
      {
         GoalType = GoalType.LeadGeneration,
         InputText = "AI tools"
      };

      // Act
      Func<Task> act = async () =>
         await service.GenerateAsync(request, CancellationToken.None);

      // Assert
      await act.Should().ThrowAsync<InsufficientCreditsException>();


   }

   [Fact]
   public async Task GenerateAsync_Should_Create_Generation_And_Decrease_Credits()
   {
      // Arrange
      var options = new DbContextOptionsBuilder<AppDbContext>()
         .UseInMemoryDatabase(Guid.NewGuid().ToString())
         .Options;

      var db = new AppDbContext(options);

      var userId = Guid.NewGuid();

      db.Users.Add(new User
      {
         Id = userId,
         Email = "test@test.com",
         PasswordHash = "hash",
         Credits = 3
      });

      await db.SaveChangesAsync();

      var openAiMock = new Mock<IOpenAiService>();
      var currentUserMock = new Mock<ICurrentUserService>();
      var loggerMock = new Mock<ILogger<GenerationService>>();

      currentUserMock
         .Setup(x => x.GetUserId())
         .Returns(userId);

      openAiMock
         .Setup(x => x.GenerateLinkedInPost(It.IsAny<GenerateLinkedInPostRequest>(), It.IsAny<CancellationToken>()))
         .ReturnsAsync("AI generated post");

      var service = new GenerationService(
         db,
         openAiMock.Object,
         currentUserMock.Object,
         loggerMock.Object
      );

      var request = new GeneratePostRequest
      {
         GoalType = GoalType.LeadGeneration,
         InputText = "AI tools"
      };

      // Act
      var result = await service.GenerateAsync(request, CancellationToken.None);

      // Assert

      result.OutputText.Should().Be("AI generated post");

      var user = await db.Users.FirstAsync();
      user.Credits.Should().Be(2);

      db.Generations.Count().Should().Be(1);

      openAiMock.Verify(
         x => x.GenerateLinkedInPost(It.IsAny<GenerateLinkedInPostRequest>(), It.IsAny<CancellationToken>()),
         Times.Once
      );
   }

   [Fact]
   public async Task GetByIdAsync_Should_Not_Return_Generation_Of_Other_User()
   {
      var options = new DbContextOptionsBuilder<AppDbContext>()
         .UseInMemoryDatabase(Guid.NewGuid().ToString())
         .Options;

      var db = new AppDbContext(options);

      var ownerId = Guid.NewGuid();
      var otherUserId = Guid.NewGuid();

      db.Generations.Add(new Generation
      {
         Id = Guid.NewGuid(),
         UserId = ownerId,
         GoalType = GoalType.LeadGeneration,
         InputText = "AI",
         OutputText = "Post",
         CreatedAt = DateTime.UtcNow
      });

      await db.SaveChangesAsync();

      var currentUserMock = new Mock<ICurrentUserService>();
      currentUserMock.Setup(x => x.GetUserId()).Returns(otherUserId);

      var service = new GenerationService(
         db,
         new Mock<IOpenAiService>().Object,
         currentUserMock.Object,
         new Mock<ILogger<GenerationService>>().Object
      );

      Func<Task> act = async () =>
         await service.GetByIdAsync(db.Generations.First().Id, CancellationToken.None);

      await act.Should().ThrowAsync<NotFoundException>();
   }

   [Fact]
   public async Task GetByIdAsync_Should_Throw_When_Generation_Not_Found()
   {
      // Arrange
      var options = new DbContextOptionsBuilder<AppDbContext>()
         .UseInMemoryDatabase(Guid.NewGuid().ToString())
         .Options;

      var db = new AppDbContext(options);

      var userId = Guid.NewGuid();

      var currentUserMock = new Mock<ICurrentUserService>();
      currentUserMock
         .Setup(x => x.GetUserId())
         .Returns(userId);

      var openAiMock = new Mock<IOpenAiService>();
      var loggerMock = new Mock<ILogger<GenerationService>>();

      var service = new GenerationService(
         db,
         openAiMock.Object,
         currentUserMock.Object,
         loggerMock.Object
      );

      var generationId = Guid.NewGuid();

      // Act
      Func<Task> act = async () =>
         await service.GetByIdAsync(generationId, CancellationToken.None);

      // Assert
      await act.Should().ThrowAsync<NotFoundException>();
   }
}