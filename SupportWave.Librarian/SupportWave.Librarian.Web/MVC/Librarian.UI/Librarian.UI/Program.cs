using Librarian.UI.Services;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
RegisterServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Book/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");

app.Run();
return;

void RegisterServices(WebApplicationBuilder webApplicationBuilder)
{
    var url = webApplicationBuilder.Configuration["ApiSettings:Url"] ??
              throw new Exception("Configuration is not present");
    webApplicationBuilder.Services.AddTransient<IRestClient, RestClient>(_ =>
        new RestClient(url));
    webApplicationBuilder.Services.AddTransient<IBookService, BookService>();
}