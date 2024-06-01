using DataAccess.Context;
using Logic;
using Microsoft.EntityFrameworkCore;
using Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
PersonRepositories personRepositories = new PersonRepositories();
PersonLogic personLogic = new PersonLogic(personRepositories);
builder.Services.AddSingleton(personRepositories);
builder.Services.AddSingleton(personLogic);
SessionLogic sessionLogic = new SessionLogic(personLogic);
builder.Services.AddSingleton(sessionLogic);
PromotionsRepositories promotionsRepositories = new PromotionsRepositories();
PromotionLogic promotionLogic = new PromotionLogic(promotionsRepositories);
builder.Services.AddSingleton(promotionsRepositories);
builder.Services.AddSingleton(promotionLogic);
StorageUnitRepositories storageUnitRepositories = new StorageUnitRepositories();
StorageUnitLogic storageUnitLogic = new StorageUnitLogic(storageUnitRepositories);
builder.Services.AddSingleton(storageUnitRepositories);
builder.Services.AddSingleton(storageUnitLogic);
BookingRepositories bookingRepositories = new BookingRepositories();
BookingLogic bookingLogic = new BookingLogic(bookingRepositories);
builder.Services.AddSingleton(bookingLogic);
AdministratorLogic administratorLogic = new AdministratorLogic(bookingRepositories);
builder.Services.AddSingleton(administratorLogic);

builder.Services.AddDbContextFactory<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("ApplicationDBLocalConnection"),
        providerOptions => providerOptions.EnableRetryOnFailure())
    );

var app = builder.Build();

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