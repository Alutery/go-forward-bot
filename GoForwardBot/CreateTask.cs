using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoForwardBot
{
    public enum Importance
    {
        Important, NotImportant
    };
    public enum ToppingOptions
    {
        Urgent, NotUrgent
    };
    public enum Priority
    {
        Urgent, High, Medium, Low
    };

    [Serializable]
    public class CreateTask
    {
        public string name;
        public DateTime date;

        //public SandwichOptions? Sandwich;
        //public LengthOptions? Length;
        //public BreadOptions? Bread;
        //public CheeseOptions? Cheese;
        public List<ToppingOptions> Toppings;
        //public List<SauceOptions> Sauce;

        public static IForm<CreateTask> BuildForm()
        {
            return new FormBuilder<CreateTask>()
                    .Message("Welcome to the simple sandwich order bot!")
                    .Build();
        }
    };
}