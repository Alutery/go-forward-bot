using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace GoForwardBot.Dialogs
{
    [Serializable]
    public class ShowAllDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            int k = 1;
            foreach (var i in RootDialog.tasks)
            {
                await context.PostAsync(String.Format("<{0}>\n\n{1}", k++, i.ToString()));
            }
            if(k == 1)
                await context.PostAsync("Your list is empty :( Enter 'Create a new task' to add");

            //var reply = context.MakeMessage();
            //reply.Text = "conversation ended";
            //await context.PostAsync(reply);
            context.Done(this);
        }

    }
}