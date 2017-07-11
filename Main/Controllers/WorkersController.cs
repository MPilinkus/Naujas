using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Main.Models;
using Slack.Webhooks.Core;

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
                workers = workers.Where(w => w.FullName.Contains(searchString));
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
        public async Task<IActionResult> Create([Bind("ID,FirstName,SecondName,BirthdayDate,WorkStartDate,Email,congratsFlag")] Worker worker)
        {

            if (ModelState.IsValid)
            {
                _context.Add(worker);
                worker.congratsFlag = false;
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,SecondName,BirthdayDate,WorkStartDate,Email")] Worker worker)
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

        // GET: Workers/SendEmail/5

        public async Task<IActionResult> SendEmail(int? id)
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

            return View();
        }

        // POST: Workers/SendEmail/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(int id, [Bind("Author,Message")]EmailMessage birthdayman)
        {
            birthdayman.RecieverID = id;
            var worker = await _context.Worker
    .SingleOrDefaultAsync(m => m.ID == id);
            if (ModelState.IsValid)
            {
                birthdayman.MailSend(birthdayman.Author, birthdayman.FullName, birthdayman.Message, birthdayman.Email);

            }


            return View();
        }

        public async Task<IActionResult> SendSlack(int id, string author, string message) {

            var worker = await _context.Worker
                .SingleOrDefaultAsync(m => m.ID == id);
            var slackClient = new SlackClient("https://hooks.slack.com/services/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");
            var slackMessage = new SlackMessage
            {
                Channel = "#general",
                Text = "Congratulation:",
                IconEmoji = Emoji.Cake,
                Username = "BirthdayBot"
            };
            slackMessage.Mrkdwn = false;
            var slackAttachment = new SlackAttachment
            {
                Fallback = worker.FirstName + " " + worker.SecondName + " is celebrating birthday!",
                Text = worker.FirstName + " " + worker.SecondName + " is celebrating birthday!",
                Color = "#D00000",
                Fields =
            new List<SlackField>
                {
                    new SlackField
                        {
                            Title = author + " says to you:",
                            Value = message
                        }
                }
            };
            slackMessage.Attachments = new List<SlackAttachment> { slackAttachment };
            await slackClient.PostAsync(slackMessage);

            return View("AfterMessage");
        }

        public IActionResult AfterMessage() {
            return View();
        }

        /* public async void dailySlackMessage(int id)
         {
             var worker = await _context.Worker
                 .SingleOrDefaultAsync(m => m.ID == id);
             var slackClient = new SlackClient("https://hooks.slack.com/services/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");
             var slackMessage = new SlackMessage
             {
                 Channel = "#general",
                 Text = "Congratulation:",
                 IconEmoji = Emoji.Cake,
                 Username = "BirthdayBot"
             };
             slackMessage.Mrkdwn = false;
             var slackAttachment = new SlackAttachment
             {
                 Fallback = worker.FirstName + " " + worker.SecondName + " is celebrating his birthday!",
                 Text = worker.FirstName + " " + worker.SecondName + " is celebrating his birthday!",
                 Color = "#D00000",
                 Fields =
             new List<SlackField>
                 {
                     new SlackField
                         {

                         }
                 }
             };
             slackMessage.Attachments = new List<SlackAttachment> { slackAttachment };
             slackClient.Post(slackMessage);
         }*/

        public async Task<Boolean> birthdayMessageCheck(int id) //true if the message was sent and false if the message was not sent
        {
            var worker = await _context.Worker
    .SingleOrDefaultAsync(m => m.ID == id);

            return true;
        }

    }
}
