﻿using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Handlers;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	[Category("EntryHandler")]
	public partial class EntryHandlerTests : HandlerTestBase<EntryHandler>
	{
		public EntryHandlerTests(HandlerTestFixture fixture) : base(fixture)
		{
		}

		[Fact(DisplayName = "Text Initializes Correctly")]
		public async Task TextInitializesCorrectly()
		{
			var entry = new EntryStub()
			{
				Text = "Test"
			};

			await ValidatePropertyInitValue(entry, () => entry.Text, GetNativeText, entry.Text);
		}

		[Fact(DisplayName = "TextColor Initializes Correctly")]
		public async Task TextColorInitializesCorrectly()
		{
			var entry = new EntryStub()
			{
				Text = "Test",
				TextColor = Color.Yellow
			};

			await ValidatePropertyInitValue(entry, () => entry.TextColor, GetNativeTextColor, entry.TextColor);
		}

		[Theory(DisplayName = "IsPassword Initializes Correctly")]
		[InlineData(true)]
		[InlineData(false)]
		public async Task IsPasswordInitializesCorrectly(bool isPassword)
		{
			var entry = new EntryStub()
			{
				IsPassword = isPassword
			};

			await ValidatePropertyInitValue(entry, () => entry.IsPassword, GetNativeIsPassword, isPassword);
		}

		[Theory(DisplayName = "Is Text Prediction Enabled")]
		[InlineData(true)]
		[InlineData(false)]
		public async Task IsTextPredictionEnabledCorrectly(bool isEnabled)
		{
			var entry = new EntryStub()
			{
				IsTextPredictionEnabled = isEnabled
			};

			await ValidatePropertyInitValue(entry, () => entry.IsTextPredictionEnabled, GetNativeIsTextPredictionEnabled , isEnabled);
		}

		[Theory(DisplayName = "IsPassword Updates Correctly")]
		[InlineData(true, true)]
		[InlineData(true, false)]
		[InlineData(false, true)]
		[InlineData(false, false)]
		public async Task IsPasswordUpdatesCorrectly(bool setValue, bool unsetValue)
		{
			var entry = new EntryStub();

			await ValidatePropertyUpdatesValue(
				entry,
				nameof(IEntry.IsPassword),
				GetNativeIsPassword,
				setValue,
				unsetValue);
		}

		[Theory(DisplayName = "TextColor Updates Correctly")]
		[InlineData(0xFF0000, 0x0000FF)]
		[InlineData(0x0000FF, 0xFF0000)]
		public async Task TextColorUpdatesCorrectly(uint setValue, uint unsetValue)
		{
			var entry = new EntryStub();

			var setColor = Color.FromUint(setValue);
			var unsetColor = Color.FromUint(unsetValue);

			await ValidatePropertyUpdatesValue(
				entry,
				nameof(IEntry.TextColor),
				GetNativeTextColor,
				setColor,
				unsetColor);
		}

		[Theory(DisplayName = "Text Updates Correctly")]
		[InlineData(null, null)]
		[InlineData(null, "Hello")]
		[InlineData("Hello", null)]
		[InlineData("Hello", "Goodbye")]
		public async Task TextUpdatesCorrectly(string setValue, string unsetValue)
		{
			var entry = new EntryStub();

			await ValidatePropertyUpdatesValue(
				entry,
				nameof(IEntry.Text),
				h =>
				{
					var n = GetNativeText(h);
					if (string.IsNullOrEmpty(n))
						n = null; // native platforms may not upport null text
					return n;
				},
				setValue,
				unsetValue);
		}

		[Theory(DisplayName = "IsTextPredictionEnabled Updates Correctly")]
		[InlineData(true, true)]
		[InlineData(true, false)]
		[InlineData(false, true)]
		[InlineData(false, false)]
		public async Task IsTextPredictionEnabledUpdatesCorrectly(bool setValue, bool unsetValue)
		{
			var entry = new EntryStub();

			await ValidatePropertyUpdatesValue(
				entry,
				nameof(IEntry.IsTextPredictionEnabled),
				GetNativeIsTextPredictionEnabled,
				setValue,
				unsetValue);
		}
	}
}
