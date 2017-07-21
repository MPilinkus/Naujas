using Main.Handlers;
using Main;
using NSubstitute;
using Slack.Webhooks.Core;
using System;
using Xunit;
using Main.Interfaces;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

            var client = Substitute.For<IMsgClient>();
            var handler = new SlackHandler(client);
            var author = "a";
            var message = "a";
            var receiverFn = "";
            var receiverLn = "";

            var msg = new SlackMessage();

            client.FormatMsg(author, message, receiverFn, receiverLn).Returns(msg);

            // Act
            handler.SendSlackMessage(author, message, receiverFn, receiverLn);

            // Assert
            client.Received().FormatMsg(author, message, receiverFn, receiverLn);
            client.Received().PostMsg(msg);
        }
        [Fact]
        public void Test2()
        {

        }
    }
}
