using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;


namespace GoForwardBot.Controllers
{
    public class Deadline
    {
        [Required]
        public DateTime? Date { get; set; }

        public static Deadline Parse(dynamic o)
        {
            try
            {
                return new Deadline
                {
                    Date = DateTime.Parse(o.Checkin.ToString()),
                };
            }
            catch
            {
                throw new InvalidCastException("Date could not be read");
            }
        }
    }
}