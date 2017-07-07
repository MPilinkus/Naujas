using System;
using System.ComponentModel.DataAnnotations;


namespace Main.Models
{
    public class Worker
    {
        public int ID { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Second Name")]
        public string SecondName { get; set; }
        [Display(Name = "Birthday Date")]
        [DataType(DataType.Date)]
        public DateTime BirthdayDate { get; set; }
        [Display(Name = "Started Working")]
        [DataType(DataType.Date)]
        public DateTime WorkStartDate { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return SecondName + ", " + FirstName;
            }
        }

        public Boolean isTodayBirthday(DateTime BirthdayDate) { //A Method to know if today is someones birthday
            bool result = false;
            DateTime today = DateTime.Today;
            if ((BirthdayDate.Month == today.Month)&&(BirthdayDate.Day == today.Day)) result = true;
            else result = false;
                return result;
        }

        
    }
}
