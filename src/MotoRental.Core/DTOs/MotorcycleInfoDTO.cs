using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Core.Entities;

namespace MotoRental.Core.DTOs
{
    public class MotorcycleInfoDTO
    {
        public MotorcycleInfoDTO(string identifier, string year, string model, string plate)
        {
            Identifier = identifier;
            Year = year;
            Model = model;
            Plate = plate;
        }
        public string Identifier { get; set; }
        public string Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        public static Motorcycle ToEntity(MotorcycleInfoDTO dto)
        {
            return new Motorcycle(dto.Identifier, dto.Year, dto.Model, dto.Plate);
        }
    }
}