using DataAccess.Context;
using Logic;
using Microsoft.EntityFrameworkCore;
using Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
PersonRepositories personRepositories = new PersonRepositories();
PersonController personController = new PersonController(personRepositories);
builder.Services.AddSingleton(personRepositories);
builder.Services.AddSingleton(personController);
SessionLogic sessionLogic = new SessionLogic(personController);
builder.Services.AddSingleton(sessionLogic);
PromotionsRepositories promotionsRepositories = new PromotionsRepositories();
PromotionController promotionController = new PromotionController(promotionsRepositories);
builder.Services.AddSingleton(promotionsRepositories);
builder.Services.AddSingleton(promotionController);
StorageUnitRepositories storageUnitRepositories = new StorageUnitRepositories();
StorageUnitController storageUnitController = new StorageUnitController(storageUnitRepositories);
builder.Services.AddSingleton(storageUnitRepositories);
builder.Services.AddSingleton(storageUnitController);
BookingRepositories bookingRepositories = new BookingRepositories();
BookingController bookingController = new BookingController(bookingRepositories);
builder.Services.AddSingleton(bookingController);
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