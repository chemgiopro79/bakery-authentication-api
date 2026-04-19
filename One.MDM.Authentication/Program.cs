using One.MDM.Authentication;
using One.MDM.Authentication.Models;
using One.MDM.Authentication.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages (nếu bạn cần giao diện)
builder.Services.AddRazorPages();

// Controllers + Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký service và DbContext
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddDbContext<BakeryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});
// Build app
var app = builder.Build();

// Luôn bật Swagger (không phụ thuộc môi trường)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "One.MDM.Authentication API V1");
    c.RoutePrefix = string.Empty; // Swagger UI hiển thị tại http://host:port/
});

// Middleware pipeline
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapRazorPages();
app.MapControllers();

// Quan trọng: lắng nghe trên 0.0.0.0 để Docker truy cập được
app.Urls.Add("http://0.0.0.0:5079");
app.Urls.Add("https://0.0.0.0:7208");

app.Run();
