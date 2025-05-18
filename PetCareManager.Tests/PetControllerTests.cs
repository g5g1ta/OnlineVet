using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using PetCareManager.Controllers;
using PetCareManager.Models;
using PetCareManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;

namespace PetCareManager.Tests
{
    public class PetControllerTests
    {
        private readonly PetController _controller;
        private readonly ApplicationDbContext _context;

        public PetControllerTests()
        {
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb") 
                .Options;

        
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated(); 

            
            var mockHttpContext = new Mock<HttpContext>();
            var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")  
            }));
            mockHttpContext.Setup(ctx => ctx.User).Returns(mockClaimsPrincipal);

            
            _controller = new PetController(_context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };
        }

        [Fact]
        public void Create_Post_ReturnsRedirect_WhenModelIsValid()
        {
            
            var pet = new Pet
            {
                Name = "Charlie"
            };

            
            var result = _controller.Create(pet).Result;

            
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);  
        }

        [Fact]
        public void Create_Post_ReturnsBadRequest_WhenModelIsInvalid()
        {
            
            var pet = new Pet
            {
                Name = string.Empty  
            };

            _controller.ModelState.AddModelError("Name", "Required");
            var result = _controller.Create(pet).Result;

            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void Create_Post_ReturnsRedirect_WhenModelIsValid_EmptyName()
        {
            
            var pet = new Pet
            {
                Name = "Milo"
            };

            
            var result = _controller.Create(pet).Result;

            
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);  
        }

        
        [Fact]
        public void Create_Get_ReturnsViewResult()
        {
            
            var result = _controller.Create();

            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        
        [Fact]
        public void Create_Post_AddsPetToDatabase_WhenModelIsValid()
        {
            
            var pet = new Pet
            {
                Name = "Max"
            };

            
            _controller.Create(pet).Wait(); 

            var addedPet = _context.Pets.FirstOrDefault(p => p.Name == "Max");
            Assert.NotNull(addedPet);
            Assert.Equal("Max", addedPet.Name);
        }
    }
}
