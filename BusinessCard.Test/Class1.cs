using BusinessCard.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BusinessCard.Test
{
    public class BusinessCardControllerTests
    {
        [Fact]
        public void GetFactsAboutMe()
        {
            // Arrange
            BusinessCardController controller = new BusinessCardController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.Equal("Hello, world!", result?.ViewData["Message"]);
        }

        [Fact]
        public void GetAboutMe()
        {
            // Arrange
            BusinessCardController controller = new BusinessCardController();
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetMyResume()
        {
            // Arrange
            BusinessCardController controller = new BusinessCardController();
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void GetService()
        {
            // Arrange
            BusinessCardController controller = new BusinessCardController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.Equal("Hello, world!", result?.ViewData["Message"]);
        }

        [Fact]
        public void CreateVacancy()
        {
            // Arrange
            BusinessCardController controller = new BusinessCardController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.Equal("Hello, world!", result?.ViewData["Message"]);
        }
    }
}
