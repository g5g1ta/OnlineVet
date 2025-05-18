using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using PetCareManager.Controllers;
using PetCareManager.Models;
using PetCareManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace PetCareManager.Tests
{
    public class AppointmentsControllerTests
    {
        private readonly AppointmentsController _controller;
        private readonly ApplicationDbContext _context;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public AppointmentsControllerTests()
        {
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            
            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);

            
            var mockHttpContext = new Mock<HttpContext>();
            var mockClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")  
            }));
            mockHttpContext.Setup(ctx => ctx.User).Returns(mockClaimsPrincipal);

            
            _controller = new AppointmentsController(_mockUserManager.Object, _context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };
        }

        [Fact]
        public async Task Create_Get_ReturnsRedirectResult()
        {
           
            int vetId = 1;
            int petId = 1;
            DateTime appointmentDate = DateTime.Now;

          
            var result = await _controller.Create(vetId, petId, appointmentDate);  

          
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);  
            Assert.Equal("Index", redirectResult.ActionName); 
        }

        [Fact]
        public async Task Create_Post_ReturnsRedirect_WhenModelIsValid()
        {
            
            var appointment = new Appointment
            {
                AppointmentDate = System.DateTime.Now.AddDays(1),  
                PetId = 1,  
                VetId = 1   
            };

            
            var result = await _controller.Create(1, 1, appointment.AppointmentDate); 

            
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);  
        }

        
        [Fact]
        public async Task Create_Post_AddsAppointmentToDatabase_WhenModelIsValid()
        {
            
            var appointment = new Appointment
            {
                AppointmentDate = System.DateTime.Now.AddDays(1),
                PetId = 1,  
                VetId = 1   
            };

            
            await _controller.Create(1, 1, appointment.AppointmentDate);  

            
            var addedAppointment = _context.Appointments.FirstOrDefault(a => a.PetId == 1);
            Assert.NotNull(addedAppointment);  
            Assert.Equal(1, addedAppointment.PetId); 
        }
    }
}
