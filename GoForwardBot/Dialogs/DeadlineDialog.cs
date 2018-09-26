using System;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace GoForwardBot.Dialogs
{
    [Serializable]
    public class DeadlineDialog : IDialog<DateTime>
    {
        private int attempts = 3;
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"What is the deadline of your task?\n\n(e.g. '28 / 04 / 2019')");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            DateTime deadline;
            if (DateTime.TryParse(message.Text, out deadline) && !(deadline < DateTime.Now))
            {
                context.Done(deadline);
            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    if (DateTime.TryParse(message.Text, out deadline) && (deadline < DateTime.Now))
                        await context.PostAsync("Your deadline is in the past. - " + message.Text);
                    else
                        await context.PostAsync(message.Text + " = I'm sorry, I don't understand your reply.\n\nWhat is your deadline (e.g. '06 / 06 / 6666')?");

                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("Message was not a valid deadline."));
                }
            }
        }


    }
}