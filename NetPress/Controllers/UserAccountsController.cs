using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetPress.Models;
using Microsoft.AspNetCore.Authorization;


namespace NetPress.Controllers
{
   
    public class UserAccountsController : Controller
    {
        private readonly UserAccountsContext userAccountContext;
       
       
        public UserAccountsController(UserAccountsContext context)
        {
            userAccountContext = context;

            
        }

        // GET: UserAccounts
        public async Task<IActionResult> Index()
        {
            string sessionRole = HttpContext.Session.GetString("NetPressRole");

            if (UserAdmin(sessionRole))
            {
                    ViewData["SessionData"] = HttpContext.Session.GetString("NetPressUsername");
                    return View(await userAccountContext.UserAccounts.ToListAsync());
                               
            }
                return RedirectToAction("Index", "Home");
           
        }

        
        // GET: UserAccounts/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            string sessionRole = HttpContext.Session.GetString("NetPressRole");

            if (ValidUser(sessionRole))
            {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var userAccounts = await userAccountContext.UserAccounts
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (userAccounts == null)
                    {
                        return NotFound();
                    }

                    TempData["SessionData"] = HttpContext.Session.GetString("NetPressRole");
                    return View(userAccounts);
                

            }
            return RedirectToAction("Index", "Home");

           
        }

        // GET: UserAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,UserEmail,UserPassword")] UserAccounts userAccounts, string ConfirmPassword)
        {         
            if (ModelState.IsValid) 
            {
                if (!EmailExists(userAccounts.UserEmail))
                {

                    if (userAccounts.UserPassword.Equals(ConfirmPassword))
                    {
                      
                        userAccountContext.Add(userAccounts);
                        await userAccountContext.SaveChangesAsync();

                        var role = await userAccountContext.Roles
                         .FirstOrDefaultAsync(thisRole => thisRole.Id == 2);

                        userAccounts.Role = role;
                        userAccounts.RolesType = "Publisher";

                        userAccountContext.Update(userAccounts);
                        await userAccountContext.SaveChangesAsync();

                        return RedirectToAction("Details", new {userAccounts.Id });
                    } else
                    {
                        ViewData["PasswordMismatch"] = "Password does not match";
                        return View();
                    }
                }
                else
                {                    
                        ViewData["EmailExists"]= "Email already exists";
                        return View();
                }
                
            }
            return View(userAccounts);
        }

        private bool EmailExists(string email)
        {
            return userAccountContext.UserAccounts.Any(user => user.UserEmail.Equals(email));
        }


        // GET: UserAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string sessionRole = HttpContext.Session.GetString("NetPressRole");
            if (ValidUser(sessionRole))
            {

                if (id == null)
                {
                    return NotFound();
                }

                var userAccounts = await userAccountContext.UserAccounts.FindAsync(id);
                if (userAccounts == null)
                {
                    return NotFound();
                }

                return View(userAccounts);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: UserAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserEmail,UserPassword")] UserAccounts userAccounts)
        {
            if (id != userAccounts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userAccountContext.Update(userAccounts);
                    await userAccountContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAccountsExists(userAccounts.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userAccounts);
        }

        // GET: UserAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            string sessionRole = HttpContext.Session.GetString("NetPressRole");
            if (ValidUser(sessionRole))
            {

                if (id == null)
                {
                    return NotFound();
                }

                var userAccounts = await userAccountContext.UserAccounts
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (userAccounts == null)
                {
                    return NotFound();
                }

                return View(userAccounts);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userAccounts = await userAccountContext.UserAccounts.FindAsync(id);
            userAccountContext.UserAccounts.Remove(userAccounts);
            await userAccountContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAccountsExists(int id)
        {
            return userAccountContext.UserAccounts.Any(e => e.Id == id);
        }

        public ActionResult Login()
        {
            if (TempData["LoginInvalid"] != null)
            {
                ViewBag.LoginInvalid = true;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string UserEmail, string Password)
        {
            var user = await userAccountContext.UserAccounts.FirstOrDefaultAsync(userContext => userContext.UserEmail == UserEmail);

            if (user != null)
            {
               // string checkPassword = HashSHA512String(Password, user.UserId.ToString());

                if (user.UserPassword.Equals(Password))
                {
                    //HttpContext.Session.SetString("NemesysUserId", user.Id.ToString());
                    HttpContext.Session.SetString("NetPressUsername", user.UserName);

                    HttpContext.Session.SetString("NetPressRole", user.RolesType.ToString());
                    return RedirectToAction("Details",new { user.Id });

                }
                else
                {
                    ViewBag.LoginInvalid = true;
                    return View();
                }

            }
            else
            {

                ViewBag.LoginInvalid = true;
                return View();
            }


        }

        public bool ValidUser(string sessionRole)
        {
            if (sessionRole != null)
            {
                return true;
            }
            return false;

        }

        public bool UserAdmin(string sessionRole)
        {
            if (sessionRole != null)
            {
                if (sessionRole.Equals("Admin"))
                {
                    return true;
                }
            }
            return false;

        }
    }
}
