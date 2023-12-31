using ProductsApp.Helpers;
using ProductsApp.Services;
using ProductsApp.Services.Contracts;
using ProductsApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri("https://expressproductapi.onrender.com") });
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

if (builder.Environment.IsProduction())
{
    builder.Services.AddSignalR().AddAzureSignalR(options =>
    {
        options.ServerStickyMode = 
            Microsoft.Azure.SignalR.ServerStickyMode.Required;
    });
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

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();