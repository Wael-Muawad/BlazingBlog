using BlazingBlog.Data;
using BlazingBlog.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Services.SeedsService
{
    public class SeedService : ISeedService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            RoleManager<IdentityRole> roleManager)
        {
            _context=context;
            _userManager=userManager;
            _userStore=userStore;
            _roleManager=roleManager;
        }



        public async Task SeedDataAsync()
        {
            //Seed Admin Role
            if (await _roleManager.FindByNameAsync(AdminAccount.Role) is null)
            {
                //Admin role does not exist
                //Lets create it
                var adminRole = new IdentityRole(AdminAccount.Role);
                var result = await _roleManager.CreateAsync(adminRole);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    throw new Exception(string.Join(Environment.NewLine, errors));
                }
            }


            //Seed Admin User
            if (await _userManager.FindByEmailAsync(AdminAccount.Email) is null)
            {
                //Admin user does not exist
                //Lets create it
                var adminUser = new ApplicationUser { Name= AdminAccount.Name, Email= AdminAccount.Email };

                await _userStore.SetUserNameAsync(adminUser, AdminAccount.Email, CancellationToken.None);
                //var emailStore = GetEmailStore();
                //await emailStore.SetEmailAsync(user, AdminAccount.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(adminUser, AdminAccount.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    throw new Exception(string.Join(Environment.NewLine, errors));
                }
            }



            //Seed Categories
            if (!await _context.Categories.AsNoTracking().AnyAsync())
            {
                //Category table is empty
                //Lets create some categories

                await _context.Categories.AddRangeAsync(Category.GetSeedCategories());
                await _context.SaveChangesAsync();
            }
        }
    }


    internal static class AdminAccount
    {
        public const string Name = "Wael Muawad";
        public const string Email = "wael@email.com";
        public const string Role = "Admin";
        public const string Password = "P@ssw0rd";
    }
}
