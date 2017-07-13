using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Models;
using Slack.Webhooks.Core;

namespace Main.Handlers
{
    public class BirthdayHandler
    {
        private readonly MainContext _context;


        public BirthdayHandler(MainContext context)
        {
            _context = context;
            Handle();
        }
        public void Handle()
        {
            var birthdaynotifications = from bn in _context.BirthdayNotifications select bn;
            var workers = from w in _context.Worker select w;
            foreach (var w in workers)
            {
                var bn = _context.BirthdayNotifications.SingleOrDefault(m => m.ID == w.ID);
                if(
                    (DateTime.Today>bn.LastNotification)
                    ||
                    ((DateTime.Today.Month==w.BirthdayDate.Day)&&(DateTime.Today.Day==w.BirthdayDate.Day))
                  )
                {
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
                        Fallback = w.FirstName + " " + w.SecondName + Message(w),
                        Text = w.FirstName + " " + w.SecondName + Message(w),
                        Color = "#D00000",
                        Fields =
                    new List<SlackField>
                        {
                            new SlackField
                            {
                                Title = "",
                                Value = ""
                            }
                        }
                    };
                    slackMessage.Attachments = new List<SlackAttachment> { slackAttachment };
                    slackClient.Post(slackMessage);

                    bn.LastNotification = DateTime.Today;
                    if (bn.FirstNotification == w.BirthdayDate)
                    {
                        bn.FirstNotification = DateTime.Today;
                    }
                    _context.BirthdayNotifications.Update(bn);
                }
            }
            _context.SaveChanges();
        }
        public string Message(Worker w)
        {
            if ((DateTime.Today.Month > w.BirthdayDate.Month) || (DateTime.Today.Day > w.BirthdayDate.Day))
            {
                string dayword;
                int day = w.BirthdayDate.Day;
                switch (day)
                {
                    case 1:
                        dayword = "st";
                        break;
                    case 2:
                        dayword = "nd";
                        break;
                    case 3:
                        dayword = "rd";
                        break;
                    default:
                        dayword = "th";
                        break;
                }
                string monthword;
                int month = w.BirthdayDate.Month;
                switch (month)
                {
                    case 1:
                        monthword = "January";
                        break;
                    case 2:
                        monthword = "February";
                        break;
                    case 3:
                        monthword = "March";
                        break;
                    case 4:
                        monthword = "April";
                        break;
                    case 5:
                        monthword = "May";
                        break;
                    case 6:
                        monthword = "June";
                        break;
                    case 7:
                        monthword = "July";
                        break;
                    case 8:
                        monthword = "August";
                        break;
                    case 9:
                        monthword = "September";
                        break;
                    case 10:
                        monthword = "October";
                        break;
                    case 11:
                        monthword = "November";
                        break;
                    case 12:
                        monthword = "December";
                        break;
                    default:
                        monthword = "";
                        break;
                }
                return (" was celebrating birthday on the " + w.BirthdayDate.Day + dayword + " of " + monthword + "!");
            }
                return " is celebrating birthday!";
        }
    }
}
