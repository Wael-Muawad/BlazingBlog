using BlazingBlog.Components;
using BlazingBlog.Components.Account;
using BlazingBlog.Data;
using BlazingBlog.Services.BlogPostsService;
using BlazingBlog.Services.CategoriesService;
using BlazingBlog.Services.SeedsService;
using BlazingBlog.Services.SubscribersService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(op => op.DetailedErrors = true);

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddTransient<ISeedService, SeedService>()
                .AddTransient<ICategoryService, CategoryService>()
                .AddTransient<IBlogPostAdminService, BlogPostAdminService>()
                .AddTransient<IBlogPostService, BlogPostService>()
                .AddTransient<ISubscriberService, SubscriberService>();

//builder.Services.AddAntiforgery();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

//Call the seed data 
await GenerateSeedAsync(app.Services);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();


static async Task GenerateSeedAsync(IServiceProvider services)
{
    var scope = services.CreateScope();
    var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
    await seedService.SeedDataAsync();
}

/*
1-streaming rendring
2-interactive server mode => SSR

11:00 - 26:24
shared comps
fix footer

26:25 - 40:29
Add Entities

40:30 - 1:14:00
Seed Data


1:14:00 - 1:37:10
fix navbar at login and manage claims

1:37:10 - 1:44:35
fix login screen


1:44:35 - 2:13:00
Dashboard
1-admin layout
 

2:13:00 - 2:37:45
Add Category service

2:37:45 - 3:00:28
add quickgrid to manage categories

3:00:28 - 3:34:00
add functionality to manage category


3:34:00 - 3:43:50
add categories to navbar if ShowOnNavbar is true


3:43:50 - 4:11:50
add blogpostAdmin service


4:11:50 - 4:43:25(4:56:40)
add manage blogpost page and add blogpost page


4:56:40 - 5:15:10
add rechtech editor 


5:15:10 - 6:17:30
add upload image functionality and add new blogpost 


6:17:30 - 6:41:40
fix manage blog post table


6:41:40 - 6:57:30
edit blog post and delete image if edit


6:57:30 - 7:19:15
add blogpost service


7:19:15 - 7:43:55
fix shared pages and add data from DB


7:43:55 - 7:55:00
7:55:00 - 8:03:30
fix categoryPost page 

8:03:30 - 8:18:00
work on BlogPostDetails Page


8:18:00 - 8:53:50
add AllCategoryPosts Page with the service


8:53:50 - 9:16:00
work on subscribersPage and service



9:16:00 - 9:29:40
work on manage subscribers page



 */