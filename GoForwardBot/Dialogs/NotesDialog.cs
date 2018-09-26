using System;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace GoForwardBot.Dialogs
{
    [Serializable]
    public class NotesDialog : IDialog<string>
    {
        private const string yes = "Yes";
        private const string no = "No";
        public Task StartAsync(IDialogContext context)
        {
            try
            {
                PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() {
                yes, no }, "Some notes?",
                 "Not a valid option", 3);
            }
            catch (TooManyAttemptsException ex)
            {
                context.PostAsync("exception" + ex);
            }
            return Task.CompletedTask;
            //await context.PostAsync($"It's almost the end!");
            //context.Wait (this.MessageReceivedAsync);
            /*PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    "Some notes?",
                    retry: "Didn't get that!",
                    promptStyle: PromptStyle.None,
                    patterns: new string[][] {
                        new string[] { "yes", "да", "yep","хочу" },
                        new string[]{ "no","нет","не хочу" } });
            return Task.CompletedTask;*/
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case yes:
                        await context.PostAsync("Enter your note: ");
                        context.Wait(GetNoteAsync);
                        break;

                    case no:
                        context.Done((string)null);
                        break;
                }
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");
            }
        }

        private async Task AfterResetAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Enter your note: ");
            context.Wait(this.GetNoteAsync);
        }
    

        
        /*private async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> result)
        {
            var confirm = await result;
            if (confirm)
            {
                await context.PostAsync("Enter your note: ");
                context.Wait(this.GetNoteAsync);
            }
            else
            {
                context.Done((string)null);
            }
        }*/

        private async Task GetNoteAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            context.Done(message.Text);
        }
    }
}