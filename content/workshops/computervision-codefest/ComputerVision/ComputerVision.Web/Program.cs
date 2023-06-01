using ComputerVision.Web.Data;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");

string cogSvcEndpoint = builder.Configuration["CognitiveServicesEndpoint"];
string cogSvcKey = builder.Configuration["CognitiveServiceKey"];

var credentials = new ApiKeyServiceClientCredentials(cogSvcKey);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IImageAnalysisService, ImageAnalysisService>();

builder.Services.AddSingleton<IComputerVisionClient>(new ComputerVisionClient(credentials)
{
    Endpoint = cogSvcEndpoint,
});

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
