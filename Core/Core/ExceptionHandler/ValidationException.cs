using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Core.ExceptionHandler;

public static class ValidationExceptionHandler
{
    public static void CustomExceptionHandler(this WebApplication webApplication)
    {
        webApplication.UseExceptionHandler(a =>
        {
            a.Run(async context =>
            {
                context.Response.StatusCode =  StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                
                if (exceptionHandlerPathFeature?.Error is ValidationException validationException)
                {
                    var errors = validationException.Errors
                        .Select(e => new { e.PropertyName, e.ErrorMessage })
                        .ToList();

                    await context.Response.WriteAsJsonAsync(new { errors });
                }
            });
            
        });
    }
}