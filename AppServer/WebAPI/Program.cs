using FileRepositories;
using RepositoryContracts;
using WebAPI.GlobalExceptionHandler;
using WebAPI.gRPC;
using WebAPI.gRPC.Interfaces;
using WebAPI.gRPC.Services;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
//builder.Services.AddScoped<IProjectRepository, ProjectFileRepository>();
builder.Services.AddScoped<IUserRepository, UserFileRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddSingleton<ProjectGrpcClient>();

var app = builder.Build();

app.MapControllers();   //solves 404 not found error

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
