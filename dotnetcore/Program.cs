using Azure.Monitor.OpenTelemetry.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

ConfigureOpenTelemetry(builder);

void ConfigureOpenTelemetry(WebApplicationBuilder builder)
{
    AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);
    builder.Services
        .AddOpenTelemetry()
        .UseAzureMonitor();

    builder.Host.UseSerilog((context, _, config) => config
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.ApplicationInsights(context.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"],
        TelemetryConverter.Traces));

}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();



app.Run();