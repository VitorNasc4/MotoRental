using System;
using System.Collections.Generic;
using MotoRental.Core.DTOs;

namespace MotoRental.Core.Entities
{
    public class DeliveryPerson : BaseEntity
    {
        private DeliveryPerson() { }
        public DeliveryPerson(string fullName, string cnpj, DateTime birthday, string cnhNumber, string cnhType, string cnhImage)
        {
            FullName = fullName;
            CNPJ = cnpj;
            Birthday = birthday;
            CNH_Number = cnhNumber;
            CNH_Type = cnhType;
            CNH_Image = cnhImage ?? "";
        }

        public string FullName { get; private set; }
        public string CNPJ { get; private set; }
        public DateTime Birthday { get; private set; }
        public string CNH_Number { get; private set; }
        public string CNH_Type { get; private set; }
        public string CNH_Image { get; private set; }

        public void SetCNH_Image(string cnh_image)
        {
            CNH_Image = cnh_image;
        }

        public static bool IsValidCNH_Type(string cnh_type)
        {
            var cnh = cnh_type.ToUpper();

            if (cnh != CNH_Types.Type_A && cnh != CNH_Types.Type_B && cnh != CNH_Types.Type_AB)
            {
                return false;
            }

            return true;
        }
    }

    public static class CNH_Types
    {
        public static readonly string Type_A = "A";
        public static readonly string Type_B = "B";
        public static readonly string Type_AB = "A+B";
    }


}
