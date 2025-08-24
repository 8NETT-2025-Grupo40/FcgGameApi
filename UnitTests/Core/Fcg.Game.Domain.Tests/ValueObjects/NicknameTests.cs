﻿using Fcg.Game.Domain.Exceptions;
using Fcg.Game.Domain.ValueObjects;

namespace UnitTests.Core.Fcg.Game.Domain.Tests.ValueObjects;

public class NicknameTests
{
	[Fact]
	public void CreateNickname_ShouldTrimAndAssignValue()
	{
		var nickname = new Nickname("  JogadorX  ");
		Assert.Equal("JogadorX", nickname.Value);
	}

	[Theory]
	[InlineData("")]
	[InlineData("  ")]
	[InlineData(null)]
	public void CreateNickname_ButEmpty_ShouldThrow(string input)
	{
		var ex = Assert.Throws<DomainException>(() => new Nickname(input));
		Assert.Equal("Nickname is required.", ex.Message);
	}

	[Theory]
	[InlineData("Jo")]
	[InlineData("EsteNicknameÉMuitoLongoParaSerValido")]
	public void CreateNickname_ButLengthInvalid_ShouldThrow(string input)
	{
		var ex = Assert.Throws<DomainException>(() => new Nickname(input));
		Assert.Equal("Nickname must be between 3 and 20 characters long.", ex.Message);
	}
}