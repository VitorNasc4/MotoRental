namespace MotoRental.Core.Entities
{
    public class Motorcycle : BaseEntity
    {
        public Motorcycle(string identifier,string year, string model, string plate)
        {
            Identifier = identifier;
            Year = year;
            Model = model;
            Plate = plate;
        }

        public string Identifier { get; private set; }
        public string Year { get; private set; }
        public string Model { get; private set; }
        public string Plate { get; private set; }

        public void ChangePlate(string newPlate)
        {
            Plate = newPlate;
        }

    }
}
