using AMS.ConHelper;
using AMS.Data;
using AMS.Helpers;
using AMS.Models;
using AMS.Models.CommonViewModel;
using AMS.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Server.IISIntegration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


builder.Services.AddScoped<ApplicationDbContext>();
var _ApplicationInfo = builder.Configuration.GetSection("ApplicationInfo").Get<ApplicationInfo>();
string _GetConnStringName = ControllerExtensions.GetConnectionString(builder.Configuration);
if (_ApplicationInfo.DBConnectionStringName == ConnectionStrings.connMySQL)
{
    builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseMySql(_GetConnStringName, ServerVersion.AutoDetect(_GetConnStringName)));
}
else if(_ApplicationInfo.DBConnectionStringName == ConnectionStrings.connPostgreSQL)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(_GetConnStringName));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_GetConnStringName));
}

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);


//Set Identity Options
var _IServiceScopeFactory = builder.Services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
var _CreateScope = _IServiceScopeFactory.CreateScope();
var _ServiceProvider = _CreateScope.ServiceProvider;
var _context = _ServiceProvider.GetRequiredService<ApplicationDbContext>();
bool IsDBCanConnect = _context.Database.CanConnect();
if (IsDBCanConnect && _context.DefaultIdentityOptions.Count() > 0)
{
    var _DefaultIdentityOptions = _context.DefaultIdentityOptions.Where(x => x.Id == 1).SingleOrDefault();
    AddIdentityOptions.SetOptions(builder.Services, _DefaultIdentityOptions);
}
else
{
    IConfigurationSection _IConfigurationSection = builder.Configuration.GetSection("IdentityDefaultOptions");
    builder.Services.Configure<DefaultIdentityOptions>(_IConfigurationSection);
    var _DefaultIdentityOptions = _IConfigurationSection.Get<DefaultIdentityOptions>();
    AddIdentityOptions.SetOptions(builder.Services, _DefaultIdentityOptions);
}



// Get Super Admin Default options
builder.Services.Configure<SuperAdminDefaultOptions>(builder.Configuration.GetSection("SuperAdminDefaultOptions"));
builder.Services.Configure<ApplicationInfo>(builder.Configuration.GetSection("ApplicationInfo"));

builder.Services.AddTransient<ICommon, Common>();
builder.Services.AddTransient<IAccount, Account>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IRoles, Roles>();
builder.Services.AddTransient<IFunctional, Functional>();
builder.Services.AddTransient<IFunctional, Functional>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Asset M@nagement Sys", Version = "v1" });
    c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
        In = ParameterLocation.Header,
        Name = "Authorization",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    //c.OperationFilter<AuthResponsesOperationFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});






var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AMS v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseSession();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseCors("Open");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var functional = services.GetRequiredService<IFunctional>();

        DbInitializer.Initialize(context, functional).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
