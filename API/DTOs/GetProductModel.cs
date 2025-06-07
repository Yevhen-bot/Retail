namespace API.DTOs
{
    public class GetProductModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        // meters per unit
        public double MPU { get; set; }
        public int Quantity { get; set; }
    }
}