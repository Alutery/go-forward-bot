using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace GoForwardBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        public static List<UserTask> tasks = new List<UserTask>();
        string name;
        DateTime deadline;
        int priority;
        string notes;

        private const string NewTaskOption = "Create a New Task";
        private const string ShowAllOption = "Show All My Tasks";
        private const string ReadyOption = "I Am Ready!";

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi, I'll help you. Let's get started.");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            await this.SendWelcomeMessageAsync(context);
        }

        private Task SendWelcomeMessageAsync(IDialogContext context)
        {
            //await context.PostAsync("Hi, I'll help you. Let's get started.");
            this.ShowOptions(context);
            return Task.CompletedTask;
            //context.Call(new NameDialog(), this.NameDialogResumeAfter);
        }
        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() {
                NewTaskOption, ShowAllOption,ReadyOption }, "What would you choose?", 
                "Not a valid option", 3);
        }
        private async Task NameDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            //tasks.Add(new UserTask());
            try
            {
                this.name = await result;

                context.Call(new DeadlineDialog(), this.DeadlineDialogResumeAfter);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task DeadlineDialogResumeAfter(IDialogContext context, IAwaitable<DateTime> result)
        {
            try
            {
                this.deadline = await result;

                context.Call(new PriorityDialog(), this.PriorityDialogResumeAfter);
                
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");
                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task PriorityDialogResumeAfter(IDialogContext context, IAwaitable<int> result)
        {
            try
            {
                this.priority = await result;

                context.Call(new NotesDialog(), this.NotesDialogResumeAfter);

            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");
                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task NotesDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                this.notes = await result;
                UserTask newTask = new UserTask(deadline, name, priority, notes);
                tasks.Add(newTask);
                await context.PostAsync("This is your new task:\n\n"+newTask.ToString());
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");
            }
            finally
            {
                await this.SendWelcomeMessageAsync(context);
            }
        }
        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case NewTaskOption:
                        context.Call(new NameDialog(), this.NameDialogResumeAfter);
                        break;

                    case ShowAllOption:
                        context.Call(new ShowAllDialog(), this.ShowAllDialogResumeAfter);
                        break;

                    case ReadyOption:
                        context.Call(new ReadyDialog(), this.ReadyDialogResumeAfter);
                        break;
                }
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ReadyDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                //await context.PostAsync("ТЫ в методе ReadyDialogResumeAfter");
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");
            }
            finally
            {
                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task ShowAllDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                //await context.PostAsync("ТЫ в методе ShowAllDialogResumeAfter");
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");
            }
            finally
            {
                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }

    }
}