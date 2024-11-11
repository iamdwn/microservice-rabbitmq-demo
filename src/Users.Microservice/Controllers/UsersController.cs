using BusinessObject.SharedModel.Enums;
using BusinessObject.SharedModels.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Users.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly ILogger<UsersController> _logger;

        private static List<User> _users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Password = "password123",
                Email = "user1@example.com",
                FullName = "User One",
                Status = UserStatus.Active,
                PhoneNumber = "0123456789",
                Role = UserRole.User
            },
            new User
            {
                Id = Guid.NewGuid(),
                Password = "password456",
                Email = "user2@example.com",
                FullName = "User Two",
                Status = UserStatus.Inactive,
                PhoneNumber = "0987654321",
                Role = UserRole.Admin
            }
        };

        public UsersController(IBus bus, ILogger<UsersController> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return _users;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User newUser)
        {
            newUser.Id = Guid.NewGuid();
            _users.Add(newUser);
            #region Business rule process anh/or call other API Service

            #endregion

            #region Publish data to Queue on RabbitMQ

            Uri uri = new Uri("rabbitmq://localhost/userQueue");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(newUser);

            #endregion
            string messageLog = string.Format("PUBLISH data to RabbitMQ.userQueue at " + $"{DateTime.Now}, with user: userId = {newUser.Id}");
            _logger.LogInformation(messageLog);
            return Ok(messageLog);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] User updatedUser)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = updatedUser.Email;
            user.FullName = updatedUser.FullName;
            user.Status = updatedUser.Status;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.Role = updatedUser.Role;

            return NoContent();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _users.Remove(user);
            return NoContent();
        }
    }
}
