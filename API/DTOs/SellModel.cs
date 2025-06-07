namespace API.DTOs
{
    public class SellModel
    {
        public string ProductName {  get; set; }
        public double Price { get; set; }
        public double MPU { get; set; }
        public int ProductCount { get; set; }
        public int StoreId {  get; set; }
    }
}
