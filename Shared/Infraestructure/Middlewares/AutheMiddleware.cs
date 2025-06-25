namespace learning_center_back.Shared.Infraestructure.Middlewares;

public class AutheMiddleware
{ 
    private readonly RequestDelegate _next;

    public AutheMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {

    }
    
}