using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using USSEScoreboard.Data;
using USSEScoreboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using USSEScoreboard.Models.CommitmentViewModels;

namespace USSEScoreboard.Controllers
{
    [Authorize]
    public class CommitmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommitmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;    
            _userManager = userManager;
        }

        // GET: Commitments
        public async Task<IActionResult> Index()
        {
            var model = new ListCommitmentsViewModel();
            model.Commitments = await _context.Commitment.Include(u => u.UserProfile).ToListAsync();
            return View(model);
        }

        // GET: Commitments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commitment = await _context.Commitment.SingleOrDefaultAsync(m => m.Id == id);
            if (commitment == null)
            {
                return NotFound();
            }

            return View(commitment);
        }

        // GET: Commitments/Create
        public async Task<ViewResult> Create()
        {
            var model = new CreateCommitmentViewModel();
            model.Users = await _context.UserProfile.ToListAsync();
            return View(model);
        }

        // POST: Commitments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Status,Title,SelectedUserID")] CreateCommitmentViewModel model)
        {
            //TODO: Remove CommitmentViewModel as it is no longer needed
            if (ModelState.IsValid)
            {
                var commitment = new Commitment();
                commitment.DateCreated = System.DateTime.Now;
                commitment.Description = model.Description;
                commitment.Status = model.Status;
                commitment.Title = model.Title;
                commitment.UserProfileId = model.SelectedUserID;
                _context.Add(commitment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(model);
        }

        // GET: Commitments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commitment = await _context.Commitment.SingleOrDefaultAsync(m => m.Id == id);
            if (commitment == null)
            {
                return NotFound();
            }
            return View(commitment);
        }

        // POST: Commitments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateCreated,Description,Status,Title")] Commitment commitment)
        {
            if (id != commitment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commitment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommitmentExists(commitment.Id))
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
            return View(commitment);
        }

        // GET: Commitments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commitment = await _context.Commitment.SingleOrDefaultAsync(m => m.Id == id);
            if (commitment == null)
            {
                return NotFound();
            }

            return View(commitment);
        }

        // POST: Commitments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commitment = await _context.Commitment.SingleOrDefaultAsync(m => m.Id == id);
            _context.Commitment.Remove(commitment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CommitmentExists(int id)
        {
            return _context.Commitment.Any(e => e.Id == id);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private Task<IList<ApplicationUser>> GetUsersAsync()
        {
            return null;
        }

        private Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            var user = _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
