using Application.Interfaces;
using Application.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 添加服务到容器
builder.Services.AddControllers();

// 配置SQLite数据库
builder.Services.AddDbContext<FamilyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 注册应用服务
builder.Services.AddScoped<IFamilyService, FamilyService>();
builder.Services.AddScoped<IFamilyMemberService, FamilyMemberService>();

// 配置CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 配置Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "家族族谱管理系统 API",
        Version = "v1",
        Description = "基于DDD架构的家族族谱管理系统API文档"
    });

    // 包含XML注释
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// 自动创建数据库
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FamilyDbContext>();
    dbContext.Database.EnsureCreated();
}

// 配置HTTP请求管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "家族族谱管理系统 API v1");
        c.RoutePrefix = string.Empty; // 设置Swagger UI为根路径
    });
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
