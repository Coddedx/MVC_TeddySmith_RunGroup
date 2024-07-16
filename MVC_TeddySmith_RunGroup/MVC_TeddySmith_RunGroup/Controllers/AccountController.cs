using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_TeddySmith_RunGroup.Data;
using MVC_TeddySmith_RunGroup.Models;
using MVC_TeddySmith_RunGroup.ViewModels;

namespace MVC_TeddySmith_RunGroup.Controllers
{
    public class AccountController : Controller
    {
        //maneger will provide extensions 
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context) //we need a constructor and do the dependecy injection
        {
            _context=context;
            _signInManager=signInManager;
            _userManager=userManager;
        }

        public IActionResult Login()
        {
            var result = new LoginViewModel(); //just in case when accidently reloaded the page it will hold the date that you previesly type in
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) //we could use without the view models ...(string password,string EmailAddress like so) but with the view models the code is more neatly 
        {
            if (!ModelState.IsValid) return View(loginViewModel); //check the view model acctually correct(modelstate is a static class)

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress); //go get the user from the database
            if(user != null) //if there is a user 
            {
                await _signInManager.SignOutAsync(); // önceki oturumdan kalma olası cookie bilgilerini temizlemek için arkaplanda SignOutAsync methoduyla çıkış işlemi

                //user is found , check password 
                var passwordCheck = await _userManager.CheckPasswordAsync(user , loginViewModel.Password); //CheckPasswordAsync is returning bool
                if (passwordCheck)
                {
                    //password correct,  sign in 
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");  //action name,controller name
                    }
                }
                else
                {
                    //password is incorrect 
                    TempData["Error"] = "Wrong credentials. Please try again."; //this is a small app so this would be enough for password failier
                    return View(loginViewModel);
                }               
            }
            else
            {
                //user not found
                TempData["Error"] = "Wrong credentials. Please try again.";
            }            
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            var result = new RegisterViewModel(); 
            return View(result);
        }


        // !!!!!!!!!!!!!!!! ŞİFREYİ KÜÇÜK HARF,BÜYÜK HARF,KISALIK GİBİ KONT ETMİYORUZ AMA SQL DE OLUŞUYOR LOGİN APARKEN SORUN OLUYOR!!!!!!!!!!!
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);  //if that was succsessull FindByEmailAsync is gonna return AppUser
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use.";
                return View(registerViewModel);
            }

            var normalizedEmail = _userManager.NormalizeEmail(registerViewModel.EmailAddress);  //emailin hepsini büyük hare çeviriyor. 
            //user yoksa yeni bir user oluşturcak 
            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,  //we set the username the email but its not ideal 
                NormalizedEmail = normalizedEmail   //bunu yapmadığımızda login yaparken sorun oluyor??
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);           
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User); //if we want to create an admin you need to create a spesific admin portal and give people the ability to add roles but his is just a regular user we use AddToRoleAsync
                _context.SaveChanges();
            }
            else
            {
                TempData["Error"] = "Couldn't be created. Try again.";
                return View(registerViewModel);
            }

            return RedirectToAction("Index", "Race");
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Race");
        }


    }
}
