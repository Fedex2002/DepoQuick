using DataAccess.Context;
using DataAccess.Repository;
using Logic;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositories;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("ApplicationDBLocalConnection"),
        providerOptions => providerOptions.EnableRetryOnFailure())
);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<IPersonController,PersonController>();
builder.Services.AddScoped<IPromotionController,PromotionController>();
builder.Services.AddScoped<IStorageUnitController,StorageUnitController>();
builder.Services.AddScoped<IDateRangeController,StorageUnitController>();
builder.Services.AddScoped<IBookingController,BookingController>();


builder.Services.AddScoped<PersonsRepository>();
builder.Services.AddScoped<PromotionsRepository>();
builder.Services.AddScoped<StorageUnitsRepository>();
builder.Services.AddScoped<BookingsRepository>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();