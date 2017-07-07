using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
    public class WorkersSecondNameViewModel
    {
        public List<Worker> workers;
        public SelectList SecondNames;
        public string workerSecondName { get; set; }
    }
}
