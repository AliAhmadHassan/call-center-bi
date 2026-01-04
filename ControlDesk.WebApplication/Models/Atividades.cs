using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlDesk.WebApplication.Models
{
    public class Atividades
    {
        public int Id { get; set; }
        public string Operador { get; set; }
        public string Status { get; set; }
        public TimeSpan TempoStatus { get; set; }
        public string Campanha { get; set; }
        public DateTime Data { get; set; }
    }
}