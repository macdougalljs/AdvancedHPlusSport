using HPlusSport.API.Models;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version"); // this would be remmed out for querystring versioning - then it all works out of the box thanks to the Nuget package
    // we would also add 
    //  options.ApiVersionReader = new QueryStringApiVersionReader("hps-api-version");

});

builder.Services.AddVersionedApiExplorer(options =>  // this section is a work around for the version error in Swagger, hopefully to be addressed in a future update
{
    options.GroupNameFormat = "'v'VVV"; // this is the pattern 
    options.SubstituteApiVersionInUrl = true;

});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ShopContext>(options =>
{
    options.UseInMemoryDatabase("Shop");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();  // this is for API security - redirects http to the https version of the application

app.UseAuthorization();

app.MapControllers();

app.Run();
