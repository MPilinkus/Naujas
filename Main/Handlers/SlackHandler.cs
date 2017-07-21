using Main.Interfaces;
using Slack.Webhooks.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Main.Handlers
{
    public class SlackHandler
    {
        private readonly IMsgClient _client;
        public SlackHandler(IMsgClient client)
        {
            _client = client;
        }

        public void SendSlackMessage(string author, string message, string FirstName, string SecondName)
        {
            if (author != "" && author != null && message != "" && message != null)
            {
                var Message = _client.FormatMsg(author, message, FirstName, SecondName);
                _client.PostMsg(Message);
            }
        }
    }
}
