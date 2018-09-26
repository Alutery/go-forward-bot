using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using GoForwardBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace GoForwardBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// 
        
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            //ConnectorClient  connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            if (activity.Type == ActivityTypes.Message)
            {
                
                //var game = (new UserTask( DateTime.Now, null, 2, "Убрать тарелки"+activity.Text)).ToString();
                //string message = game.Play(activity.Text);
                //Activity reply = activity.CreateReply(game);
                //await connector.Conversations.ReplyToActivityAsync(reply);
                await Conversation.SendAsync(activity, () => new DeadlineDialog_t());
            }
            else
            {
                await HandleSystemMessageAsync(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessageAsync(Activity message)
        {
            //ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                IConversationUpdateActivity update = message as IConversationUpdateActivity;
                var client = new ConnectorClient(new Uri(message.ServiceUrl), new MicrosoftAppCredentials());
                if (update.MembersAdded != null && update.MembersAdded.Any())
                {
                    foreach (var member in update.MembersAdded)
                    {
                        // if the bot is added, then   
                        if (member.Id != update.Recipient.Id)
                        {
                            var reply = ((Activity)update).CreateReply($"Hey Welcome to search chatbot.");
                            await client.Conversations.ReplyToActivityAsync(reply);
                        }
                        else
                        {
                            var reply = ((Activity)update).CreateReply($"Hey Welcome to search chatbot. ELSE");
                            await client.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                }

                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}