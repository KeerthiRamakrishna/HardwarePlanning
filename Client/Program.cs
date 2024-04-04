using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Osporting.Client;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<Osporting.Client.OSPortDBService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient("Osporting.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Osporting.Server"));
builder.Services.AddScoped<Osporting.Client.SecurityService>();
builder.Services.AddScoped<AuthenticationStateProvider, Osporting.Client.ApplicationAuthenticationStateProvider>();
var host = builder.Build();
await host.RunAsync();