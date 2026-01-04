using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControlDesk.WebApplication.Models
{
    public class Logados
    {
        [Display(Name="Seguimentos")]
        public string GrupoAtendimento { get; set; }

        [Display(Name = "Logados")]
        public int PessoasLogadas { get; set; }

        [Display(Name = "Capacity")]
        public int Capacity { get; set; }
    }
}