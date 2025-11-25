using BookManager.ApiService.Mappings;
using BookManager.Core.Repositories;
using BookManager.Core.Services;
using BookManager.Infrastructure.Persistence;
using BookManager.Infrastructure.Repositories;
using BookManager.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookManagerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BookManagerDb")));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<BookProfile>());

var app = builder.Build();

app.UseExceptionHandler();

app.MapControllers();

// for demonstration purposes I use swagger also in prod
app.UseSwagger();
app.UseSwaggerUI();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.MapDefaultEndpoints();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    // runtime migrations are not good for prod, but
    // this is just tiny demo app with sqlite
    // normally I would generate migrations -> sql script
    // and then run the sql script with docker upon proper database creation in container
    var db = scope.ServiceProvider.GetRequiredService<BookManagerDbContext>();
    db.Database.Migrate();
}

app.Run();
