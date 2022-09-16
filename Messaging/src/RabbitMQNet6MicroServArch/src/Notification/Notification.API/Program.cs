using Common.SeedWork;
using Notification.API;
using Steeltoe.Messaging.RabbitMQ.Config;
using Steeltoe.Messaging.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Services
// Add singleton that will process incoming messages
builder.Services.AddSingleton<ListenerService>();
#endregion

#region SteelToeRabbitMQ Configuration

// Add Steeltoe Rabbit services, use JSON serialization
builder.Services.AddRabbitServices(true);

// Add Steeltoe RabbitAdmin services to get queues declared
builder.Services.AddRabbitAdmin();

// Add Steeltoe RabbitTemplate for sending/receiving
builder.Services.AddRabbitTemplate();

// Add a queue to the message container that the rabbit admin will discover and declare at startup
builder.Services.AddRabbitQueue(new Queue(Queues.ProductAddQueue));

// Tell steeltoe about singleton so it can wire up queues with methods to process queues
builder.Services.AddRabbitListeners<ListenerService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();