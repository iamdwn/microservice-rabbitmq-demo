using BusinessObject.SharedModel.Enums;
using BusinessObject.SharedModels.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ponds.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PondsController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly ILogger<PondsController> _logger;

        public PondsController(IBus bus, ILogger<PondsController> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        private static List<Pond> _ponds = new List<Pond>
        {
            new Pond
            {
                Id = Guid.NewGuid(),
                PondName = "Pond A",
                Volume = 1000m,
                Depth = 2m,
                DrainCount = 1,
                SkimmerCount = 1,
                PumpCapacity = 200m,
                ImgUrl = "http://example.com/image1.jpg",
                Note = "Note for Pond A",
                Description = "Description for Pond A",
                Status = PondStatus.Active,
                IsQualified = true,
                UserId = Guid.NewGuid() // Simulate a user id
            },
            new Pond
            {
                Id = Guid.NewGuid(),
                PondName = "Pond B",
                Volume = 2000m,
                Depth = 3m,
                DrainCount = 2,
                SkimmerCount = 2,
                PumpCapacity = 400m,
                ImgUrl = "http://example.com/image2.jpg",
                Note = "Note for Pond B",
                Description = "Description for Pond B",
                Status = PondStatus.Inactive,
                IsQualified = false,
                UserId = Guid.NewGuid() // Simulate a user id
            }
        };

        // GET: api/<PondsController>
        [HttpGet]
        public ActionResult<IEnumerable<Pond>> Get()
        {
            return _ponds;
        }

        // GET api/<PondsController>/5
        [HttpGet("{id}")]
        public ActionResult<Pond> Get(Guid id)
        {
            var pond = _ponds.FirstOrDefault(p => p.Id == id);
            if (pond == null)
            {
                return NotFound();
            }
            return pond;
        }

        // POST api/<PondsController>
        [HttpPost]
        public async Task<ActionResult<Pond>> Post([FromBody] Pond newPond)
        {
            newPond.Id = Guid.NewGuid();
            _ponds.Add(newPond);
            #region Business rule process anh/or call other API Service

            #endregion

            #region Publish data to Queue on RabbitMQ

            Uri uri = new Uri("rabbitmq://localhost/pondQueue");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(newPond);

            #endregion
            string messageLog = string.Format("PUBLISH data to RabbitMQ.pondQueue at " + $"{DateTime.Now}, with pond: pondId = {newPond.Id}");
            _logger.LogInformation(messageLog);
            return Ok(messageLog);
        }

        // PUT api/<PondsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Pond updatedPond)
        {
            var pond = _ponds.FirstOrDefault(p => p.Id == id);
            if (pond == null)
            {
                return NotFound();
            }

            pond.PondName = updatedPond.PondName;
            pond.Volume = updatedPond.Volume;
            pond.Depth = updatedPond.Depth;
            pond.DrainCount = updatedPond.DrainCount;
            pond.SkimmerCount = updatedPond.SkimmerCount;
            pond.PumpCapacity = updatedPond.PumpCapacity;
            pond.ImgUrl = updatedPond.ImgUrl;
            pond.Note = updatedPond.Note;
            pond.Description = updatedPond.Description;
            pond.Status = updatedPond.Status;
            pond.IsQualified = updatedPond.IsQualified;
            pond.UserId = updatedPond.UserId;

            return NoContent();
        }

        // DELETE api/<PondsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var pond = _ponds.FirstOrDefault(p => p.Id == id);
            if (pond == null)
            {
                return NotFound();
            }

            _ponds.Remove(pond);
            return NoContent();
        }
    }
}
