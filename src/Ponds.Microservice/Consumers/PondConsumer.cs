using BusinessObject.SharedModels.Models;
using MassTransit;

namespace Ponds.Microservice.Consumers
{
    public class PondConsumer : IConsumer<User>
    {

        private readonly ILogger _logger;

        public PondConsumer(ILogger<PondConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<User> context)
        {
            var data = context.Message;

            if (data != null)
            {
                _logger.LogInformation($"Received message from user post api: create user success");
            }
            else
            {
                _logger.LogInformation($"Received message from user post api: create user fail");
            }
        }
    }
}
