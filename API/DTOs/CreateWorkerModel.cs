namespace API.DTOs
{
    public class CreateWorkerModel
    {
        public RegisterModel RegisterModel { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public DateOnly Datebirth { get; set; }
        public decimal Salary { get; set; }
        public int BuildingId { get; set; }

    }
}
