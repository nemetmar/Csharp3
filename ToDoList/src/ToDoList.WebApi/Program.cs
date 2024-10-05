var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/nazdarSvete", () => "Nazdar světe!");

app.Run();
