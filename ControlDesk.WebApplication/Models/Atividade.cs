using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControlDesk.WebApplication.Models
{
    public class Atividade
    {
        public Atividade()
        {
            QtdPausas = -1;
            Atendidas = -1;
        }


        [Display(Name = "Operador")]
        public string Operador { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Tempo")]
        public TimeSpan TempoStatus { get; set; }

        [Display(Name = "Campanha")]
        public string Campanha { get; set; }

        [Display(Name = "Data")]
        public DateTime Data { get; set; }

        [Display(Name = "Atividade - Ultima Atualização")]
        public DateTime DataAtualizacaoAtividade { get; set; }

        [Display(Name = "Pausas")]
        public int QtdPausas { get; set; }

        [Display(Name = "Tempo")]
        public TimeSpan TotalPausas { get; set; }

        [Display(Name = "Pausa - Ultima Atualização")]
        public DateTime DataAtualizacaoPausa { get; set; }

        [Display(Name = "Qtd. Atendidas")]
        public int Atendidas { get; set; }

        public TimeSpan TMA { get; set; }

    }
}