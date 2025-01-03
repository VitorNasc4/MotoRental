﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MotoRental.Application.ViewModels
{
    public class RentalViewModel
    {
        public RentalViewModel(
            string Identificador, 
            decimal valorDiaria, 
            string entregadorId, 
            string motoId, 
            DateTime dataInicio, 
            DateTime dataTermino, 
            DateTime dataPrevistaTermino, 
            DateTime? dataDevolucao,
            decimal totalCost,
            decimal penalty)
        {
            identificador = Identificador;
            valor_diaria = valorDiaria;
            entregador_id = entregadorId;
            moto_id = motoId;
            data_inicio = dataInicio;
            data_termino = dataTermino;
            data_prevista_termino = dataPrevistaTermino;
            data_devolucao = dataDevolucao;
            custo_total = totalCost;
            multa = penalty;
        }

        public string identificador { get; private set; }
        public decimal valor_diaria { get; private set; }
        public string entregador_id { get; private set; }
        public string moto_id { get; private set; }
        public DateTime data_inicio { get; private set; }
        public DateTime data_termino { get; private set; }
        public DateTime data_prevista_termino { get; private set; }
        public DateTime? data_devolucao { get; private set; }
        public decimal custo_total { get; private set; }
        public decimal multa { get; private set; }
    }
}
