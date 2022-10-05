using API;
using API.Middlewares;

using Application.Models.API;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDependecyInjection(builder.Configuration).AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState.Where(x => x.Value?.Errors.Count > 0).SelectMany(x => x.Value!.Errors).Select(x => x.ErrorMessage);
        return new BadRequestObjectResult(new ApiValidationErrorResponse()
        {
            Errors = errors,
        });
    };
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    app.UseExceptionMiddleware();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseSession();
    app.Use(async (context, next) =>
    {
        var token = context.Session.GetString("Token");
        if (!string.IsNullOrEmpty(token))
        {
            context.Request.Headers.Add("Authorization", "Bearer " + token);
        }
        await next();
    });
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}