using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using USSEScoreboard.Data;
using USSEScoreboard.Models;

namespace USSEScoreboard.Controllers
{
    public class GlobalScoreEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GlobalScoreEntriesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: GlobalScoreEntries
        public async Task<IActionResult> Index()
        {
            return View(await _context.GlobalScoreEntry.ToListAsync());
        }

        // GET: GlobalScoreEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var globalScoreEntry = await _context.GlobalScoreEntry.SingleOrDefaultAsync(m => m.GlobalScoreEntryId == id);
            if (globalScoreEntry == null)
            {
                return NotFound();
            }

            return View(globalScoreEntry);
        }

        // GET: GlobalScoreEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GlobalScoreEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GlobalScoreEntryId,DateCreated,DateModified,GlobalScoreType,TimeFrameTotal,WeekEnding")] GlobalScoreEntry globalScoreEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(globalScoreEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(globalScoreEntry);
        }

        // GET: GlobalScoreEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var globalScoreEntry = await _context.GlobalScoreEntry.SingleOrDefaultAsync(m => m.GlobalScoreEntryId == id);
            if (globalScoreEntry == null)
            {
                return NotFound();
            }
            return View(globalScoreEntry);
        }

        // POST: GlobalScoreEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GlobalScoreEntryId,DateCreated,DateModified,GlobalScoreType,TimeFrameTotal,WeekEnding")] GlobalScoreEntry globalScoreEntry)
        {
            if (id != globalScoreEntry.GlobalScoreEntryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(globalScoreEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GlobalScoreEntryExists(globalScoreEntry.GlobalScoreEntryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(globalScoreEntry);
        }

        // GET: GlobalScoreEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var globalScoreEntry = await _context.GlobalScoreEntry.SingleOrDefaultAsync(m => m.GlobalScoreEntryId == id);
            if (globalScoreEntry == null)
            {
                return NotFound();
            }

            return View(globalScoreEntry);
        }

        // POST: GlobalScoreEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var globalScoreEntry = await _context.GlobalScoreEntry.SingleOrDefaultAsync(m => m.GlobalScoreEntryId == id);
            _context.GlobalScoreEntry.Remove(globalScoreEntry);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool GlobalScoreEntryExists(int id)
        {
            return _context.GlobalScoreEntry.Any(e => e.GlobalScoreEntryId == id);
        }
    }
}
