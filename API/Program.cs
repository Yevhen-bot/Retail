using API.Mappers;
using Core.Interfaces;
using Data_Access;
using Data_Access.Entities;
using Data_Access.Repos;
using Infrastructure.Auth;
using Infrastructure.Creational;
using Infrastructure.Mappers;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

// TO DO: verify buy endpoint valid adding orders, log out, manage proper adding new worker, unique emails, better validation

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
builder.Services.AddScoped<OrderMapper>();

builder.Services.AddTransient<PasswordService<Owner>>();
builder.Services.AddTransient<PasswordService<Client>>();
builder.Services.AddTransient<PasswordService<Worker>>();

builder.Services.AddScoped<OwnerService>();
builder.Services.AddScoped<BuildingService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<WorkerService>();
builder.Services.AddScoped<CacheService>();

builder.Services.AddMemoryCache(opt =>
{
    opt.SizeLimit = 1024;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
