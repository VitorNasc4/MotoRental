using System;
using System.Collections.Generic;
using System.Text;

namespace MotoRental.Application.ViewModels
{
    public class MotorcylceViewModel
    {
        public MotorcylceViewModel(string identifier, string year, string model, string plate)
        {
            identificador = identifier;
            ano = year;
            modelo = model;
            placa = plate;
        }

        public string identificador { get; private set; }
        public string ano { get; private set; }
        public string modelo { get; private set; }
        public string placa { get; private set; }
    }
}
