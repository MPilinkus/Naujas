using Slack.Webhooks.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Interfaces
{
    public interface IMsgClient
    {
        void PostMsg(SlackMessage Message);
        SlackMessage FormatMsg(string author, string message, string recieverFN, string recieverLN);
    }

    public class MsgClient : IMsgClient
    {
        private readonly SlackClient _slackClient;
        public MsgClient(SlackClient slackClient)
        {
            _slackClient = slackClient;
        }

        public void PostMsg(SlackMessage Message)
        {
            _slackClient.Post(Message);
        }

        public SlackMessage FormatMsg(string author, string message, string recieverFN, string recieverLN)
        {
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
                Fallback = recieverFN + " " + recieverLN + " is celebrating birthday!",
                Text = recieverFN + " " + recieverLN + " is celebrating birthday!",
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
            return slackMessage;
        }
    }
}
