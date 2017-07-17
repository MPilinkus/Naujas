using System;
using System.ComponentModel.DataAnnotations;


namespace Main.Models
{
    public class BirthdayNotification
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FirstNotification { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastNotification { get; set; }

        public int WorkerID { get; set; } 

        public Worker Worker { get; set; }
    }
}
