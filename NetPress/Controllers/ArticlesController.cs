using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetPress.Models;

namespace NetPress.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly UserAccountsContext _context;

        public ArticlesController(UserAccountsContext context)
        {
            _context = context;
        }

        // GET: Articles
       
        public async Task<IActionResult> Index()
        {
            ViewData["SessionData"] = HttpContext.Session.GetString("NetPressUsername");
            UserAccounts user = await _context.UserAccounts.FirstOrDefaultAsync(usercontext => usercontext.UserName == HttpContext.Session.GetString("NetPressUsername"));

            ViewData["AuthorId"] = user.Id;

            return View(await _context.Articles.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articles = await _context.Articles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articles == null)
            {
                return NotFound();
            }

            return View(articles);
        }

        // GET: Articles/Create
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("NetPressUsername") != null)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View();
            } else
            {
                return RedirectToAction("Index", "Home");
            } 

           
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Status,CategoryId")] Articles articles)
        {
            
            if (ModelState.IsValid)
            {

                HttpRequest status = Request;

                var cc = ModelState;
                var xx = articles;

                _context.Add(articles);
                await _context.SaveChangesAsync();

               
                string sessionUser = HttpContext.Session.GetString("NetPressUsername");

                UserAccounts user = _context.UserAccounts.FirstOrDefault(userContext => userContext.UserName == sessionUser);
                articles.AuthorId = user.Id;

                articles.DateCreated = DateTime.Now;
                articles.LastModified = DateTime.Now;
                _context.Update(articles);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(articles);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articles = await _context.Articles.FindAsync(id);
            if (articles == null)
            {
                return NotFound();
            }
            return View(articles);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,DateCreated,LastModified,Status,AuthorId,CategoryId")] Articles articles)
        {
            if (id != articles.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticlesExists(articles.Id))
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
            return View(articles);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articles = await _context.Articles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articles == null)
            {
                return NotFound();
            }

            return View(articles);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articles = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(articles);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticlesExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
