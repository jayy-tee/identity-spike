using FluentAssertions;
using Identity.Common;
using Identity.Domain.UserAggregate;
using Identity.TestSdk.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Identity.Domain.UnitTests
{
    [TestClass]
    public class UserTests
    {

        [TestMethod]
        public void WhenInstantiated_PropertiesAreSetAsExpected()
        {
            // Arrange & Act
            var testUser = ValidLegacyUser.AsUser();

            // Assert
            testUser.FirstName.Should().Be(ValidLegacyUser.Firstname);
            testUser.LastName.Should().Be(ValidLegacyUser.Lastname);
            testUser.Username.Should().Be(ValidLegacyUser.Username);
            testUser.EmailAddress.Should().Be(ValidLegacyUser.EmailAddress);
            testUser.Source.Should().Be(ValidLegacyUser.Status);
            testUser.UserStatus.Should().Be(ValidLegacyUser.UserSource);
        }

        [TestMethod]
        public void WhenInstantiatedForAnUnkownSource_ExceptionIsThrown()
        {
            // Arrange
            Action action = () => new User(
                   ValidLegacyUser.Firstname, ValidLegacyUser.Lastname, ValidLegacyUser.Username,
                   ValidLegacyUser.EmailAddress, ValidLegacyUser.Status, ValidLegacyUser.Password,
                   UserSource.None);

            // Act & Assert
            action.Should().Throw<Exception>("because we cannot determine a hasing method for an unknown user source");
        }

        [TestMethod]
        public void WhenALegacyPasswordIsSet_ItIsHashedAsWeExpect()
        {
            // Arrange/Act
            var testUser = ValidLegacyUser.AsUser();
            var expectedHash = PasswordHelper.EncodeLegacyPassword(ValidLegacyUser.Password);

            // Assert
            testUser.GetPasswordHash().Should().Be(expectedHash);       
        }

        [TestMethod]
        public void WhenAMembershipPasswordIsSet_ItIsHashedAsWeExpect()
        {
            // Arrange/Act
            var testUser = ValidUser.AsUser();
            var expectedHash = PasswordHelper.EncodeMembershipPassword(ValidUser.Password, testUser.GetPasswordSalt());

            // Assert
            testUser.GetPasswordHash().Should().Be(expectedHash);       
        }

        [TestMethod]
        public void WhenAMembershipPasswordIsChanged_ANewHashAndSaltIsGenerated()
        {
            // Arrange
            const string NewPassword = "theNewPassword";
            var testUser = ValidUser.AsUser();
            var originalSalt = testUser.GetPasswordSalt();

            // Act
            testUser.SetPassword(NewPassword);
            var expectedHash = PasswordHelper.EncodeMembershipPassword(NewPassword, testUser.GetPasswordSalt());

            // Assert
            testUser.GetPasswordHash().Should().Be(expectedHash);           
            testUser.GetPasswordSalt().Should().NotBe(originalSalt, "because, for security reasons, we should re-salt new passwords");
        }

        [TestMethod]
        public void WhenValidatingAPassword_WrongPasswordReturnsFalse()
        {
            // Arrange
            const string WrongPassword = "theWrongPassword"; 
            var testUser = ValidUser.AsUser();

            // Act/Assert
            testUser.ValidatePassword(WrongPassword).Should().BeFalse();
        }

        [TestMethod]
        public void WhenValidatingAPassword_CorrectPasswordReturnsTrue()
        {
            // Arrange
            var testUser = ValidUser.AsUser();

            // Act/Assert
            testUser.ValidatePassword(ValidUser.Password).Should().BeTrue();
        }
    }
}
