using Ocelot.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.Middleware;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.Provider.Polly;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var routes = "./Routes";
builder.Configuration.AddOcelotWithSwaggerSupport(p => p.Folder = routes);


//builder.Configuration.AddJsonFile("configuration.json", optional: false, reloadOnChange: true);
builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Services.AddAuthentication(opt =>
{

    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    //  opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //  opt.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

}).AddCookie(x =>
{
    x.Cookie.Name = "Bearer";
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"])),
        ClockSkew = TimeSpan.FromDays(1),
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["Bearer"];
            return Task.CompletedTask;
        }
    };




});

/*builder.Services.AddCors();
*/
builder.Services.AddOcelot();




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    /* c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
     c.SwaggerEndpoint("/client/swagger/v1/swagger.json", "Client API");
     c.SwaggerEndpoint("/manager/swagger/v1/swagger.json", "Manager API");*/
});
app.UseSwaggerUI();
app.UseStaticFiles();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

app.UseCors();

await app.UseOcelot();
app.UseRouting();
app.UseHttpsRedirection();


app.Run();
