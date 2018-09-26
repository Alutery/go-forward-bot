using System;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GoForwardBot.Dialogs
{
    [Serializable]
    public class PriorityDialog : IDialog<int>
    {
        private const string LowPriority = "Low";
        private const string MediumPriority = "Medium";
        private const string HighPriority = "High";

        public Task StartAsync(IDialogContext context)
        {
            try
            {
                PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() {
                LowPriority, MediumPriority,HighPriority }, "What is your priority?",
                 "Not a valid option", 3);
            }
            catch (TooManyAttemptsException ex)
            {
                context.PostAsync("exception" + ex);
            }
            return Task.CompletedTask;
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case LowPriority:
                        context.Done(1);
                        break;

                    case MediumPriority:
                        context.Done(2);
                        break;

                    case HighPriority:
                        context.Done(3);
                        break;
                }
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");
            }
        }
    }
}