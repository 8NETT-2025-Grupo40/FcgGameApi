using Fcg.Game.Domain.Exceptions;
using Fcg.Game.Domain.ValueObjects;

namespace UnitTests.Core.Fcg.Game.Domain.Tests.Entities;

public class DescriptionTests
{
	[Fact]
	public void OnDescriptionCreation_WhenValueIsValid_ShouldCreateInstance()
	{
		// Arrange
		const string validTitle = "Sample Valid Description";

		// Act
		var title = new Description(validTitle);

		// Assert
		Assert.Equal(validTitle, title.Value);
	}

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData(null)]
	public void OnDescriptionCreation_WhenValueIsNullOrWhitespace_ShouldThrowExceptionWithCorrectMessage(string invalidDescription)
	{
		// Arrange
		const string expectedExceptionMessage = "The description must not be empty";

		// Act
		void createDescription() { var description = new Description(invalidDescription); }

		// Assert
		var exceptionResult = Assert.Throws<DomainException>(createDescription);
		Assert.Equal(expectedExceptionMessage, exceptionResult.Message);
	}

	[Theory]
	[InlineData("invalid")]
	[InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Lorem ipsum. Lorem ipsum. Lorem ipsum. Lorem ipsum.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Lorem ipsum. Lorem ipsum. Lorem ipsum. Lorem ipsum.")]
	public void OnDescriptionCreation_WhenValueDoesNotHaveExpectedLength_ShouldThrowExceptionWithCorrectMessage(string invalidTitle)
	{
		// Arrange
		const int minimumLength = 10;
		const int maximumLength = 510;
		var expectedExceptionMessage = $"The description length must be between {minimumLength} and {maximumLength} characters";

		// Act
		var l = invalidTitle.Length;
		void createDescription() { var description = new Description(invalidTitle); }

		// Assert
		var exceptionResult = Assert.Throws<DomainException>(createDescription);
		Assert.Equal(expectedExceptionMessage, exceptionResult.Message);
	}
}
