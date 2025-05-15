using API.Mappers;
using Core.Interfaces;
using Data_Access;
using Data_Access.Entities;
using Data_Access.Repos;
using Infrastructure.Auth;
using Infrastructure.Creational;
using Infrastructure.Mappers;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddApiAuth(builder.Configuration);
builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped<IUserRepository<Owner>, OwnerRepository>();
builder.Services.AddScoped<IUserRepository<Client>, ClientRepository>();
builder.Services.AddScoped<IUserRepository<Worker>, WorkerRepository>();
builder.Services.AddScoped<IRepository<Order>, OrderRepository>();
builder.Services.AddScoped<IRepository<Building>, BuildingRepository>();

builder.Services.AddSingleton<StoreFactory>();
builder.Services.AddSingleton<WarehouseFactory>();

builder.Services.AddScoped<JwtOptions>();
builder.Services.AddScoped<JwtProvider>();

builder.Services.AddScoped<Mapper>();
builder.Services.AddScoped<ClientMapper>();
builder.Services.AddScoped<WorkerMapper>();
builder.Services.AddScoped<BuildingMapper>();

builder.Services.AddTransient<PasswordService<Owner>>();
builder.Services.AddTransient<PasswordService<Client>>();
builder.Services.AddTransient<PasswordService<Worker>>();

builder.Services.AddScoped<OwnerService>();
builder.Services.AddScoped<BuildingService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
