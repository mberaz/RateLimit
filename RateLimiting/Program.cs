using RateLimiting.RateLimiter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IKeyExtractor, KeyExtractor>();
builder.Services.AddSingleton<IRateLimitStorage, RateLimitStorage>();
builder.Services.AddSingleton<IRateLimitCacheStorage, RateLimitCacheMemoryStorage>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();
app.UseRateLimit();
app.MapControllers();

app.Run();
