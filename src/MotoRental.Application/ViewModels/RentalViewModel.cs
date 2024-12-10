using System;
using System.Collections.Generic;
using System.Text;

namespace MotoRental.Application.ViewModels
{
    public class RentalViewModel
    {
        public RentalViewModel(int Identificador, decimal valorDiaria, int entregadorId, int motoId, DateTime dataInicio, DateTime dataTermino, DateTime dataPrevistaTermino, DateTime? dataDevolucao)
        {
            identificador = Identificador;
            valor_diaria = valorDiaria;
            entregador_id = entregadorId;
            moto_id = motoId;
            data_inicio = dataInicio;
            data_termino = dataTermino;
            data_prevista_termino = dataPrevistaTermino;
            data_devolucao = dataDevolucao;
            
        }

        public int identificador { get; private set; }
        public decimal valor_diaria { get; private set; }
        public int entregador_id { get; private set; }
        public int moto_id { get; private set; }
        public DateTime data_inicio { get; private set; }
        public DateTime data_termino { get; private set; }
        public DateTime data_prevista_termino { get; private set; }
        public DateTime? data_devolucao { get; private set; }
    }
}
