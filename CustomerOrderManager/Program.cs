using CustomerOrderManager.Business.Context;
using CustomerOrderManager.Business.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


/////////////////// Register Dependencies

builder.Services.AddScoped<ICustomerOrderRepository, CustomerOrderRepository>();
builder.Services.AddScoped<ICustomerOrderParameterRepository, CustomerOrderParameterRepository>();

///////////////////


builder.Services.AddDbContext<ICustomerOrderManagerDbContext, CustomerOrderManagerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CustomerOrderManagerDbContext"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer Order Manager", Version = "v1" });
});

var app = builder.Build();

//do db migrations
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    using (var context = serviceScope.ServiceProvider.GetService<CustomerOrderManagerDbContext>())
    {
        context.Database.Migrate();
    }
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ICustomerOrderManagerDbContext>();

    if (!dbContext.Database.CanConnect())
    {
        throw new NotImplementedException("Cannot connect to DB");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

