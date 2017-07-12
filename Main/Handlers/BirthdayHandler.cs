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
            /*var workers = from w in _context.Worker
                          select w;*/
            var workers = from w in _context.Worker select w;
            foreach (var w in workers)
            {
                
                if ((w.isTodayBirthday(w.BirthdayDate))&&(w.congratsFlag==false))
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
                        Fallback = w.FirstName + " " + w.SecondName + " is celebrating birthday!",
                        Text = w.FirstName + " " + w.SecondName + " is celebrating birthday!",
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

                    w.congratsFlag = true;

                    _context.Update(w);

                }

            }
            _context.SaveChanges();
        }
    }
}
