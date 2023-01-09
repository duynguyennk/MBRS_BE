using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Repositories.Repository;
using MBRS_API.Services.IService;
using MBRS_API.Services.Service;
using MBRS_API.SignalR;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Repositories.IRepository;
using MBRS_API_DEMO.Repositories.Repository;
using MBRS_API_DEMO.Services.IService;
using MBRS_API_DEMO.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SqlServerDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
});
builder.Services.AddControllers();
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IManageEmployeeAccountService, ManageEmployeeAccountService>();
builder.Services.AddTransient<IManageCustomerAccountService, ManageCustomerAccountService>();
builder.Services.AddTransient<IManageRoomService, ManageRoomService>();
builder.Services.AddTransient<IManageBikeService, ManageBikeService>();
builder.Services.AddTransient<IManageFoodService, ManageFoodService>();
builder.Services.AddTransient<IManageCarAirportService, ManageCarAirportService>();
builder.Services.AddTransient<IManageFloorService, ManageFloorService>();
builder.Services.AddTransient<IManageTypeRoomService, ManageTypeRoomService>();
builder.Services.AddTransient<IManageTypeBikeService, ManageTypeBikeService>();
builder.Services.AddTransient<IManageTypeFoodService, ManageTypeFoodService>();
builder.Services.AddTransient<IManageTypeCarAirportService, ManageTypeCarAirportService>();
builder.Services.AddTransient<IOrderRoomService, OrderRoomService>();
builder.Services.AddTransient<IManageOrderRoomService, ManageOrderRoomService>();
builder.Services.AddTransient<IManageOrderBikeService, ManageOrderBikeService>();
builder.Services.AddTransient<IOrderBikeService, OrderBikeService>();
builder.Services.AddTransient<IOrderFoodService, OrderFoodService>();
builder.Services.AddTransient<IViewStatusRoomService, ViewStatusRoomService>();
builder.Services.AddTransient<ICustomerAccountService, CustomerAccountService>();
builder.Services.AddTransient<IViewOrderForCustomerService, ViewOrderForCustomerService>();
builder.Services.AddTransient<IManageFeedbackOrderCustomerService, ManageFeedbackOrderCustomerService>();
builder.Services.AddTransient<IFilterServiceCustomerService, FilterServiceCustomerService>();
builder.Services.AddTransient<IManageOrderFoodService, ManageOrderFoodService>();
builder.Services.AddTransient<ISendNotificationService, SendNotificationService>();
builder.Services.AddTransient<IManageActivityEmployeeService, ManageActivityEmployeeService>();


builder.Services.AddTransient<ILoginRepository, LoginRepository>();
builder.Services.AddTransient<IManageEmployeeAccountRepository, ManageEmployeeAccountRepository>();
builder.Services.AddTransient<IManageCustomerAccountRepository, ManageCustomerAccountRepository>();
builder.Services.AddTransient<IManageRoomRepository, ManageRoomRepository>();
builder.Services.AddTransient<IManageBikeRepository, ManageBikeRepository>();
builder.Services.AddTransient<IManageFoodRepository, ManageFoodRepository>();
builder.Services.AddTransient<IManageCarAirportRepository, ManageCarAirportRepository>();
builder.Services.AddTransient<IManageFloorRepository, ManageFloorRepository>();
builder.Services.AddTransient<IManageTypeRoomRepository, ManageTypeRoomRepository>();
builder.Services.AddTransient<IManageTypeBikeRepository, ManageTypeBikeRepository>();
builder.Services.AddTransient<IManageTypeFoodRepository, ManageTypeFoodRepository>();
builder.Services.AddTransient<IManageTypeCarAirportRepository, ManageTypeCarAirportRepository>();
builder.Services.AddTransient<IOrderRoomRepository, OrderRoomRepository>();
builder.Services.AddTransient<IManageOrderRoomRepository, ManageOrderRoomRepository>();
builder.Services.AddTransient<IManageOrderBikeRepository, ManageOrderBikeRepository>();
builder.Services.AddTransient<IOrderBikeRepository, OrderBikeRepository>();
builder.Services.AddTransient<IOrderFoodRepository, OrderFoodRepository>();
builder.Services.AddTransient<IViewStatusRoomRepository, ViewStatusRoomRepository>();
builder.Services.AddTransient<ICustomerAccountRepository, CustomerAccountRepository>();
builder.Services.AddTransient<IViewOrderForCustomerRepository, ViewOrderForCustomerRepository>();
builder.Services.AddTransient<IManageFeedbackOrderCustomerRepository, ManageFeedbackOrderCustomerRepository>();
builder.Services.AddTransient<IFilterServiceCustomerRepository, FilterServiceCustomerRepository>();
builder.Services.AddTransient<IManageOrderFoodRepository, ManageOrderFoodRepository>();
builder.Services.AddTransient<ISendNotificationRepository, SendNotificationRepository>();
builder.Services.AddTransient<IManageActivityEmployeeRepository, ManageActivityEmployeeRepository>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<TimerManager>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MBRS_API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
});
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200", "https://localhost:7076", "http://sandbox.vnpayment.vn", "https://localhost:44358").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                      });
});
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
builder.Services.AddFluentEmail(builder.Configuration["MailConfig:DefaultToMailAddress"]).AddSmtpSender(builder.Configuration["MailConfig:Host"], int.Parse(builder.Configuration["MailConfig:Port"]), builder.Configuration["MailConfig:UserName"], builder.Configuration["MailConfig:Password"]);
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});
app.MapHub<ChatHub>("/sendNotification");

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
