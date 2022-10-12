using bulkybookweb.efcore;
using BulkyBookWeb.EFCore;
using BulkyBookWeb.Models;
using BulkyBookWeb.Security;
using BulkyBookWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<Category>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
{
    Options.Password.RequiredLength = 10;
    Options.Password.RequireNonAlphanumeric = false;
    Options.Password.RequiredUniqueChars = 3;
    //Options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<BulkyContext>();
builder.Services.AddDbContextPool<BulkyContext>(
options => options.UseSqlServer(builder.Configuration.GetConnectionString("BulkyBookDbConnection")));
builder.Services.AddScoped<EFCategoryRepository>(); 
builder.Services.AddScoped<EFCoreUserRepository>();
//Adding authorization,with in authorization, we add policy for claims
builder.Services.AddAuthorization(options=> {
    options.AddPolicy("DeleteRolePolicy",
        policy => policy.RequireClaim("Delete Role"));

    options.AddPolicy("EditRolePolicy",

    // policy=>
    //policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));//here we are doing with handler clases with multiple handler classes

    policy => policy.RequireAssertion(context =>
       context.User.IsInRole("Admin") &&
       context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
       context.User.IsInRole("Super Admin")

           ));


    //allowedcountrypolicy ,will only allowed listed countries in country police,
    //options.AddPolicy("AllowedCountryPolicy",
    //    policy => policy.RequireClaim("country","USA","PAKISTAN","UK"));

    options.AddPolicy("AdminRolePolicy",
       policy => policy.RequireClaim("Admin"));
       //policy => policy.RequireClaim("Admin"));

});
builder.Services.AddSingleton<IAuthorizationHandler,CanEditOnlyOtherAdminRolesAndClaimsHandler>(); // adding handlers
builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();//adding handlers
builder.Services.ConfigureApplicationCookie(options => {
    options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
});
//builder.Services.AddAuthentication()
//               .AddGoogle(options =>
//               {
//                   options.ClientId = "682461296936-hgvhbdnk0a2ct9s38iio4vt7vqjeu9tm.apps.googleusercontent.com";
//                   options.ClientSecret = "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";
//               })
//               .AddFacebook(options =>
//               {
//                   options.AppId = "1173280010120902";
//                   options.AppSecret = "c8e3d478f6fc9987ae3cb524a4ec598c";
//               });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//else
//{
//    //app.UseExceptionHandler("/Error");
//    app.UseStatusCodePagesWithRedirects("/Home/{0}");
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
