using Microsoft.EntityFrameworkCore;
using MyMvcAppFinal.Data;
using MyMvcAppFinal.Services;

var builder = WebApplication.CreateBuilder(args);

//Db
var connection = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<UnitContext>(options => options.UseNpgsql(connection));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IUnitService, UnitService>();

var app = builder.Build();

/*//Json
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);*/

//Db init and update
using var serviceScope = app.Services.CreateScope();
var dbContext = serviceScope.ServiceProvider.GetService<UnitContext>();
dbContext?.Database.Migrate();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Unit}/{action=Index}/{id?}");

app.Run();
