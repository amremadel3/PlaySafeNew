using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlaySafe.Data;
using PlaySafe.Models;

namespace PlaySafe.Controllers
{
    public class userTypesController : Controller
    {
        private readonly dbContext _context;

        public userTypesController(dbContext context)
        {
            _context = context;
        }

        // GET: userTypes
        public async Task<IActionResult> Index()
        {
              return View(await _context.userType.ToListAsync());
        }

        // GET: userTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.userType == null)
            {
                return NotFound();
            }

            var userType = await _context.userType
                .FirstOrDefaultAsync(m => m.id == id);
            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        // GET: userTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: userTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,usersType")] userType userType)
        {
            if (ModelState.IsValid)
            {
                userType.id = Guid.NewGuid();
                _context.Add(userType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userType);
        }

        // GET: userTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.userType == null)
            {
                return NotFound();
            }

            var userType = await _context.userType.FindAsync(id);
            if (userType == null)
            {
                return NotFound();
            }
            return View(userType);
        }

        // POST: userTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id,usersType")] userType userType)
        {
            if (id != userType.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!userTypeExists(userType.id))
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
            return View(userType);
        }

        // GET: userTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.userType == null)
            {
                return NotFound();
            }

            var userType = await _context.userType
                .FirstOrDefaultAsync(m => m.id == id);
            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        // POST: userTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.userType == null)
            {
                return Problem("Entity set 'dbContext.userType'  is null.");
            }
            var userType = await _context.userType.FindAsync(id);
            if (userType != null)
            {
                _context.userType.Remove(userType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool userTypeExists(Guid id)
        {
          return _context.userType.Any(e => e.id == id);
        }
        
    }
}
