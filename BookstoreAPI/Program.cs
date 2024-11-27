using BookstoreAPI.Configuration;
using BookstoreAPI.Repositories;
using BookstoreAPI.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings)));

builder.Services.AddSingleton<IMongoDbSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IMongoDbSettings>();
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

