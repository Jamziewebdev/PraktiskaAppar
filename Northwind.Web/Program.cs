using Northwind.DataContext.SqlServer;
using Northwind.EntityModels;

#region Konfigurera web server host och tjänster
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddNorthwindContext();
builder.Services.AddRequestDecompression();


var app = builder.Build();
#endregion

#region Konfiguration av HTTP pipeline och routing
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app .Use(async (HttpContext context, Func<Task> next) =>
{
    RouteEndpoint? rep = context.GetEndpoint() as RouteEndpoint;
    if (rep is not null)
    {
        WriteLine($"Endpoint name: {rep.DisplayName}");
        WriteLine($"Endpoint route pattern: {rep.RoutePattern.RawText}");
    }
    if (context.Request.Path == "/bonjour")
    {
        // ifall vi har en request som matchar /bonjour så svarar vi direkt,
        // detta avbryter pipeline, delegate next körs inte
        await context.Response.WriteAsync("Bonjour Monde!");
        return;
    }
    // vi kan modifiera request och response här innan det går vidare i pipeline
    await next();
    // vi kan modifiera response här efter vi har kallat next delegate och innan den skickas till klienten
});
app.UseHttpsRedirection();
app.UseRequestDecompression();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapRazorPages();

app.MapGet("/hello", () => $"Environment is {app.Environment.EnvironmentName}");
#endregion

app.Run();
WriteLine("Detta exekveras efter att webservern har stoppats!");