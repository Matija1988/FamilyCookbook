
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FamilyCookbook;
using FamilyCookbook.Repository;
using FamilyCookbook.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(builder => {
    builder.RegisterModule(new AutofacRepositoryModule());
    builder.RegisterModule(new AutofacServiceModule());
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
