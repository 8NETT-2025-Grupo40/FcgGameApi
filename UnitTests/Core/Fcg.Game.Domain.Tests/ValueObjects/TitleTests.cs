using Fcg.Game.Domain.Exceptions;
using Fcg.Game.Domain.ValueObjects;

namespace UnitTests.Core.Fcg.Game.Domain.Tests.ValueObjects;

public class TitleTests
{
	[Fact]
	public void OnTitleCreation_WhenValueIsValid_ShouldCreateInstance()
	{
		// Arrange
		const string validTitle = "Sample Valid Title";

		// Act
		var title = new Title(validTitle);

		// Assert
		Assert.Equal(validTitle, title.Value);
	}

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData(null)]
	public void OnTitleCreation_WhenValueIsNullOrWhitespace_ShouldThrowExceptionWithCorrectMessage(string invalidTitle)
	{
		// Arrange
		const string expectedExceptionMessage = "The title must not be empty";

		// Act
		void createTitle() { var title = new Title(invalidTitle); }

		// Assert
		var exceptionResult = Assert.Throws<DomainException>(createTitle);
		Assert.Equal(expectedExceptionMessage, exceptionResult.Message);
	}

	[Theory]
	[InlineData("invalid")]
	[InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Lorem ipsum. Lorem ipsum. Lorem ipsum. Lorem ipsum.")]
	public void OnTitleCreation_WhenValueDoesNotHaveExpectedLength_ShouldThrowExceptionWithCorrectMessage(string invalidTitle)
	{
		// Arrange
		const int minimumLength = 10;
		const int maximumLength = 255;
		var expectedExceptionMessage = $"The title length must be between {minimumLength} and {maximumLength} characters";

		// Act
		var l = invalidTitle.Length;
		void createTitle() { var title = new Title(invalidTitle); }

		// Assert
		var exceptionResult = Assert.Throws<DomainException>(createTitle);
		Assert.Equal(expectedExceptionMessage, exceptionResult.Message);
	}
}
