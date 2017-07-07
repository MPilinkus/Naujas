using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Main.Models;

namespace Main.Controllers
{
    public class WorkersController : Controller
    {
        private readonly MainContext _context;

        public WorkersController(MainContext context)
        {
            _context = context;    
        }

        // GET: Workers

        public async Task<IActionResult> Index(
            //string workerSecondName, 
            string sortOrder,
            string searchString,
            string currentFilter,
            int? page
            )
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstNameSortParm"] = sortOrder == "NameUp" ? "NameDown" : "NameUp";
            ViewData["SecondNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["BirthdaySortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["WorkStartSortParm"] = sortOrder == "WDate" ? "Wdate_desc" : "WDate";
            // Use LINQ to get list of genres.
            /*IQueryable <string> SecondNameQuery = from w in _context.Worker
                                            orderby w.SecondName
                                            select w.SecondName;*/
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var workers = from w in _context.Worker
                         select w;
            if (!String.IsNullOrEmpty(searchString))
            {
                workers = workers.Where(w => w.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "NameUp":
                    workers = workers.OrderBy(w => w.FirstName);
                    break;
                case "NameDown":
                    workers = workers.OrderByDescending(w => w.FirstName);
                    break;
                case "name_desc":
                    workers = workers.OrderBy(w => w.SecondName);
                    break;
                case "Date":
                    workers = workers.OrderBy(w => w.BirthdayDate);
                    break;
                case "date_desc":
                    workers = workers.OrderByDescending(w => w.BirthdayDate);
                    break;
                case "WDate":
                    workers = workers.OrderBy(w => w.WorkStartDate);
                    break;
                case "Wdate_desc":
                    workers = workers.OrderByDescending(w => w.WorkStartDate);
                    break;
                default:
                    workers = workers.OrderByDescending(w => w.SecondName);
                    break;
            }

            var today = DateTime.Today;

            

            /*if (!String.IsNullOrEmpty(searchString))
            {
                workers = workers.Where(s => s.FirstName.Contains(searchString));
            }

            /*if (!String.IsNullOrEmpty(workerSecondName))
            {
                workers = workers.Where(x => x.SecondName == workerSecondName);
            }*/

            //var workerSecondNameVM = new WorkersSecondNameViewModel();
            //workerSecondNameVM.SecondNames = new SelectList(await SecondNameQuery.Distinct().ToListAsync());
           // workerSecondNameVM.workers = await workers.ToListAsync();

            int pageSize = 5;
            return View(await PaginatedList<Worker>.CreateAsync(workers.AsNoTracking(), page ?? 1, pageSize));
        }


        // GET: Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker
                .SingleOrDefaultAsync(m => m.ID == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Workers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,SecondName,BirthdayDate,WorkStartDate")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(worker);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(worker);
        }

        // GET: Workers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker.SingleOrDefaultAsync(m => m.ID == id);
            if (worker == null)
            {
                return NotFound();
            }
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,SecondName,BirthdayDate,WorkStartDate")] Worker worker)
        {
            if (id != worker.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.ID))
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
            return View(worker);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker
                .SingleOrDefaultAsync(m => m.ID == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var worker = await _context.Worker.SingleOrDefaultAsync(m => m.ID == id);
            _context.Worker.Remove(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool WorkerExists(int id)
        {
            return _context.Worker.Any(e => e.ID == id);
        }

       
    }
}
