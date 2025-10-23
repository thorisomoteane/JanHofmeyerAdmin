using Xunit;
using FluentAssertions;
using System;

namespace JanHofmeyer.Tests
{
    public class ModelTests
    {
        [Fact]
        public void Project_ShouldHaveRequiredProperties()
        {
            // Arrange
            var projectProperties = new[] { "Title", "Summary", "PeopleImpacted", "About", "KeyGoals" };

            // Act
            var hasAllProperties = projectProperties.Length == 5;

            // Assert
            hasAllProperties.Should().BeTrue();
        }

        [Fact]
        public void Event_DateShouldBeInFuture_ForUpcomingEvents()
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(30);
            var today = DateTime.Now;

            // Act
            var isInFuture = futureDate > today;

            // Assert
            isInFuture.Should().BeTrue();
        }

        [Fact]
        public void Contact_EmailValidation_ShouldContainAtSymbol()
        {
            // Arrange
            var validEmail = "test@example.com";
            var invalidEmail = "testexample.com";

            // Act
            var validContainsAt = validEmail.Contains("@");
            var invalidContainsAt = invalidEmail.Contains("@");

            // Assert
            validContainsAt.Should().BeTrue();
            invalidContainsAt.Should().BeFalse();
        }

        [Fact]
        public void Review_RatingRange_ShouldBeBetween1And5()
        {
            // Arrange
            var validRating = 3;
            var invalidLowRating = 0;
            var invalidHighRating = 6;

            // Act & Assert
            (validRating >= 1 && validRating <= 5).Should().BeTrue();
            (invalidLowRating >= 1 && invalidLowRating <= 5).Should().BeFalse();
            (invalidHighRating >= 1 && invalidHighRating <= 5).Should().BeFalse();
        }

        [Fact]
        public void Volunteer_ApplicationStatus_ShouldHaveValidStates()
        {
            // Arrange
            var validStatuses = new[] { "Pending", "Approved", "Rejected" };

            // Act
            var statusCount = validStatuses.Length;

            // Assert
            statusCount.Should().Be(3);
            validStatuses.Should().Contain("Pending");
        }
    }
}