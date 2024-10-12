using GoogleAuthticationExample.Repositories.Interfance;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GoogleAuthticationExample.Repositories.Implementaion
{
    public class LoginGoogleService : ILoginGoogleService
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public LoginGoogleService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        public async Task<bool> LoginExtnal(string email)
        {
            bool result = false;
            var userExists = await userManager.FindByEmailAsync(email);
            if (userExists != null)
             { 
               
                await signInManager.SignInAsync(userExists,false);
               
                result = true;
             }
            return result;
        }

        public async Task<bool> RegisterExtnal(string email)
        {
            bool result = false;
            var userExists = await userManager.FindByEmailAsync(email);
            if (userExists == null)
            {
                IdentityUser applicationUser = new IdentityUser
                {
                    SecurityStamp = Guid.NewGuid().ToString(),

                    Email = email,
                    UserName = email,
                    EmailConfirmed = true,
                    

                };
                var userCreation = await userManager.CreateAsync(applicationUser);
                if (userCreation.Succeeded)
                {
                    
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
               
            }
            return result;
        }
    }
}
