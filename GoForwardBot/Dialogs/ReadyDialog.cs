using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoForwardBot.Dialogs
{
    [Serializable]
    public class ReadyDialog : IDialog
    {
        private int attempts = 3;

        private const string Done = "Done Something";
        private const string Choose = "Choose Next Challenge";
        public Task StartAsync(IDialogContext context)
        {
            try
            {
                PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() {
                Done, Choose }, "What is your choice?",
                 "Not a valid option", 3);
            }
            catch (TooManyAttemptsException ex)
            {
                context.PostAsync("exception" + ex);
            }
            return Task.CompletedTask;
            //await context.PostAsync("Ну рад за тебя. Для начала хватит лежать.");
            //context.Done(this);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case Done:
                        await context.PostAsync("Enter number of task:");
                        context.Wait(this.MessageForDelete);
                        break;

                    case Choose:
                        await context.PostAsync("I'll show you what you need to do in a week or with a high priority");
                        int k = 1;
                        bool flag = true;
                        foreach (var i in RootDialog.tasks)
                        {
                            if (i.Deadline < DateTime.Now.AddDays(7) || i.Priority == 3)
                            {
                                await context.PostAsync(String.Format("<{0}>\n\n{1}", k++, i.ToString()));
                                flag = false;
                            }
                        }
                        if (flag)
                            await context.PostAsync("I haven't found any tasks for you. Well, you can relax ;)");
                        context.Done(this);
                        break;
                }
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");
            }
        }

        private async Task MessageForDelete(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            /* If the message returned is a valid name, return it to the calling dialog. */
            if ((int.TryParse(message.Text, out int index)) && (index > 0 && index <= RootDialog.tasks.Count))
            {
                RootDialog.tasks.RemoveAt(index-1);
                await context.PostAsync("Good work!");
                context.Done(this);
            }
            /* Else, try again by re-prompting the user. */
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("Number is not correct. Try again, please.");

                    context.Wait(this.MessageForDelete);
                }
                else
                {
                    /* Fails the current dialog, removes it from the dialog stack, and returns the exception to the 
                        parent/calling dialog. */
                    context.Fail(new TooManyAttemptsException("Message was not a string or was an empty string."));
                }
            }
        }
    }
}