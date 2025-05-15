namespace API.DTOs
{
    public class CreateBuildingModel
    {
        public double Area { get; set; }
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
