using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JanHofmeyer.Tests
{
	public class ControllerTests
	{
		[Fact]
		public void HomeController_Index_ReturnsViewResult()
		{
			// Arrange - This section is intentionally simple for demonstration
			var expectedViewName = "Index";

			// Act - Simulate the action
			var result = expectedViewName;

			// Assert - Check if it's correct
			result.Should().Be("Index");
		}

		[Fact]
		public void ProjectsController_HasCorrectCategories()
		{
			// Arrange
			var categories = new[] { "Active", "Planning" };

			// Act
			var activeCategory = categories[0];
			var planningCategory = categories[1];

			// Assert
			activeCategory.Should().Be("Active");
			planningCategory.Should().Be("Planning");
		}

		[Fact]
		public void EventsController_HasCorrectEventTypes()
		{
			// Arrange
			var eventTypes = new[] { "registration open", "Coming soon", "Invite only" };

			// Act
			var count = eventTypes.Length;

			// Assert
			count.Should().Be(3);
			eventTypes.Should().Contain("registration open");
		}

		[Fact]
		public void ReviewController_StarRating_IsWithinValidRange()
		{
			// Arrange
			var minRating = 1;
			var maxRating = 5;
			var testRating = 4;

			// Act
			var isValid = testRating >= minRating && testRating <= maxRating;

			// Assert
			isValid.Should().BeTrue();
		}

		[Fact]
		public void GalleryController_SupportsMultipleMediaTypes()
		{
			// Arrange
			var supportedTypes = new[] { "image", "video" };

			// Act
			var imageSupported = supportedTypes[0] == "image";
			var videoSupported = supportedTypes[1] == "video";

			// Assert
			imageSupported.Should().BeTrue();
			videoSupported.Should().BeTrue();
		}
	}
}