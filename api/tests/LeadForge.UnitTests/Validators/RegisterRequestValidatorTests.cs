using FluentAssertions;
using LeadForge.Application;
using LeadForge.Application.Validators;

namespace LeadForge.UnitTests.Validators;

public class RegisterRequestValidatorTests
{
    [Fact]
    public void Should_Return_Error_When_Email_Is_Invalid()
    {
        // Arrange
        var validator = new RegisterRequestValidator();

        var request = new RegisterRequest
        {
            Email = "invalid-email",
            Password = "Password123!"
        };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Return_Error_When_Password_Is_Empty()
    {
        var validator = new RegisterRequestValidator();

        var request = new RegisterRequest
        {
            Email = "test@test.com",
            Password = ""
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Request_Is_Valid()
    {
        var validator = new RegisterRequestValidator();

        var request = new RegisterRequest
        {
            Email = "test@test.com",
            Password = "Password!23"
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
}