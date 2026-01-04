using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlDesk.WebApplication.Models
{
    public class Operador:Dominio.Operador
    {
        public List<Operador> Operadores()
        {

            List<Operador> operadores = new List<Operador>();

            foreach (Dominio.Operador dominioOperador in new Dominio.Operador().Operadores())
            {
                Operador operador = new Operador();
                operador.Fim = dominioOperador.Fim;
                operador.Id = dominioOperador.Id;
                operador.IdGrupo = dominioOperador.IdGrupo;
                operador.Inicio = dominioOperador.Inicio;
                operador.Nome = dominioOperador.Nome;
                operador.TotalPausa07 = dominioOperador.TotalPausa07;
                operador.TotalPausa08 = dominioOperador.TotalPausa08;
                operador.TotalPausa09 = dominioOperador.TotalPausa09;
                operador.TotalPausa10 = dominioOperador.TotalPausa10;
                operador.TotalPausa11 = dominioOperador.TotalPausa11;
                operador.TotalPausa12 = dominioOperador.TotalPausa12;
                operador.TotalPausa13 = dominioOperador.TotalPausa13;
                operador.TotalPausa14 = dominioOperador.TotalPausa14;
                operador.TotalPausa15 = dominioOperador.TotalPausa15;
                operador.TotalPausa16 = dominioOperador.TotalPausa16;
                operador.TotalPausa17 = dominioOperador.TotalPausa17;
                operador.TotalPausa18 = dominioOperador.TotalPausa18;
                operador.TotalPausa19 = dominioOperador.TotalPausa19;
                operador.TotalPausa20 = dominioOperador.TotalPausa20;
                operador.TotalPausa21 = dominioOperador.TotalPausa21;

                operadores.Add(operador);
            }


            return operadores;
        }
    }
}