using ApiBlog.Application;
using ApiBlog.Application.Services;
using ApiBlog.Infrastructure;
using ApiBlog.WebApi.Authentication;
using ApiBlog.WebApi.Extensions;
using ApiBlog.WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilterAttribute>();
});
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserContext, HttpCurrentUserContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
