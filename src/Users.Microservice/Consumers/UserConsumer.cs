using BusinessObject.SharedModels.Models;
using MassTransit;

namespace Ponds.Microservice.Consumers
{
    public class UserConsumer : IConsumer<Pond>
    {

        private readonly ILogger _logger;

        public UserConsumer(ILogger<UserConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<Pond> context)
        {
            var data = context.Message;

            if (data != null)
            {
                _logger.LogInformation($"Received message from pond post api: create pond success");
            }
            else
            {
                _logger.LogInformation($"Received message from pond post api: create pond fail");
            }
        }
    }
}
