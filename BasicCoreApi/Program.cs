using BasicCoreApi.Business;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<Program>();
    });

builder.Services.AddSingleton<ICallbackBusiness, CallbackBusiness>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
