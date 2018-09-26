using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoForwardBot
{
    [Serializable]
    public class UserTask
    {
        string name;
        DateTime deadline;
        int priority;
        string notes;
        static readonly DateTime date_default = new DateTime(2015, 7, 20, 18, 30, 25);

        public string Name
        {
            set
            {
                if (value != null && value.Length < 50)
                    name = value;
                else
                    name = "New task";
            }
            get
            {
                return name;
            }
        }
        public int Priority
        {
            set
            {
                if (value == 1 || value == 2 || value == 3)
                    priority = value;
                else
                    priority = 1;
            }
            get
            {
                return priority;
            }
        }
        public DateTime Deadline
        {
            get
            {
                return deadline;
            }
            set
            {
                if (value >= DateTime.Now)
                    deadline = value;
                else
                    deadline = DateTime.Today;
            }
        }
        public string Notes
        {
            set { notes = value; }
            get { return notes; }
        }
        public UserTask(DateTime deadline, string name = "New task", int priority = 1, string notes = null)
        {
            Deadline = deadline;
            Priority = priority;
            Name = name;
            Notes = notes;
        }
        public UserTask() : this(date_default, null, 1, null) { }
        string PriorityToString()
        {
            if (priority == 1)
                return "low";
            else if (priority == 2)
                return "medium";
            else
                return "high";
        }
        public override string ToString()
        {
            return String.Format("Task: {0}\n\nPriority: {1}\n\nDeadline: {2}", name, PriorityToString(), deadline.ToString("d")) + (notes == null ? "" : "\n\nNote: \n" + notes);
        }
        public static string PrintAll(List<UserTask> tasks)
        {
            string res = "";
            int k = 1;
            foreach(var i in tasks)
            {
                res += String.Format("<{0}>\n\n{1}\n\n\n\n", k++, i.ToString());
            }
            return res;
        }
    }
}