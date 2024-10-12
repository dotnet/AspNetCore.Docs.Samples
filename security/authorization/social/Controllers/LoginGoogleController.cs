using Google.Apis.Auth;
using GoogleAuthticationExample.Repositories.Interfance;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace GoogleAuthticationExample.Controllers
{
    public class LoginGoogleController : Controller
    {
        private readonly ILoginGoogleService loginGoogleService;

       
        public LoginGoogleController(ILoginGoogleService loginGoogleService)
        {
            this.loginGoogleService = loginGoogleService;
        } 
        public async Task<IActionResult> Index(string credential)
        {
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(credential);
            if (payload == null)
            {
                return RedirectToAction(nameof(Login));
            }
            var result = await loginGoogleService.RegisterExtnal(payload.Email);
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var clamisUser = await loginGoogleService.LoginExtnal(payload.Email);
                if (!clamisUser)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {

                    return RedirectToAction("Index", "Home");
                }
            }
        }
       
        
    }
}
