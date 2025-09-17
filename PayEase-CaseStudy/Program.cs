using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PayEase_CaseStudy.Authentication;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.Repository;
using PayEase_CaseStudy.Services;

var builder = WebApplication.CreateBuilder(args);

// -------------------- CONNECTION STRINGS --------------------
var connectionString = builder.Configuration.GetConnectionString("defaultconnection");

// -------------------- DBCONTEXTS --------------------
// Identity DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// App DbContext
builder.Services.AddDbContext<PayDbContext>(options =>
    options.UseSqlServer(connectionString));



// -------------------- IDENTITY --------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// -------------------- JWT AUTHENTICATION --------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "please Enter Token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
});

// -------------------- REPOSITORIES --------------------
builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddScoped<IEmployee, EmployeeRepo>();
builder.Services.AddScoped<IDepartment, DepartmentRepo>();
builder.Services.AddScoped<IPayroll, PayrollRepo>();
builder.Services.AddScoped<IPayrollDetail, PayrollDetailRepo>();
builder.Services.AddScoped<IAuditLog, AuditLogRepo>();
builder.Services.AddScoped<ICompensation, CompensationRepo>();
builder.Services.AddScoped<ILeave, LeaveRepo>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


builder.Services.AddDbContext<PayDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultconnection")));



// -------------------- CONTROLLERS & SWAGGER --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// -------------------- MIDDLEWARE --------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Order matters: Authentication BEFORE Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
