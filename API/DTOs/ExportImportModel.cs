namespace API.DTOs
{
    public class ExportImportModel
    {
        public string ProductName { get; set; }
        public double MPU { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int BuildingFromId { get; set; }
        public int BuildingToId { get; set;}
    }
}
