namespace MotoRental.Core.Entities
{
    public class Motorcycle : BaseEntity
    {
        public Motorcycle(string year, string model, string plate)
        {
            Year = year;
            Model = model;
            Plate = plate;
        }

        public string Year { get; private set; }
        public string Model { get; private set; }
        public string Plate { get; private set; }

        public void ChangePlate(string newPlate)
        {
            Plate = newPlate;
        }

    }
}
