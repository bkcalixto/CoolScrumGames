using Microsoft.Extensions.FileProviders;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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

// ConfigureServices and Configure are still not being use
// the purpose of those will be in the implementations of cookies

void ConfigureServices(IServiceCollection services)
{
    services.AddDistributedMemoryCache(); // Use a distributed cache for a production environment
    services.AddSession(options =>
    {
        options.Cookie.Name = ".YourApp.Session";
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });
    // other service configurations...
}

void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
{
    // other app configurations...

    app.UseSession();

    // other middleware...
}

app.Run();
