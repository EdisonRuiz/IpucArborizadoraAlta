using IPUC.AA.Back.BD;
using IPUC.AA.Back.BL.Implementations;
using IPUC.AA.Back.BL.Interfaces;
using IPUC.AA.Back.DataBase;
using Presentation.Data;
using Presentation.Services.Implementations;
using Presentation.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPaymentBL, PaymentBL>();
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<PaymentBD>();
builder.Services.AddScoped<UserDB>();

builder.Services.AddDbContext<DBContext>();

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
