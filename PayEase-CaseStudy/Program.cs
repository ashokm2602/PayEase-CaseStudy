using Microsoft.EntityFrameworkCore;
using PayEase_CaseStudy.Models;
using PayEase_CaseStudy.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PayDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Defaultconnection")));
builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddScoped<IRole, RoleRepo>();
builder.Services.AddScoped<IEmployee, EmployeeRepo>();
builder.Services.AddScoped<IDepartment, DepartmentRepo>();
builder.Services.AddScoped<IPayroll, PayrollRepo>();
builder.Services.AddScoped<IPayrollDetail, PayrollDetailRepo>();
builder.Services.AddScoped<IAuditLog, AuditLogRepo>();
builder.Services.AddScoped<ICompensation, CompensationRepo>();
builder.Services.AddScoped<ILeave, LeaveRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
