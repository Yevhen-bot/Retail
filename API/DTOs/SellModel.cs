namespace API.DTOs
{
    public class SellModel
    {
        public string ProductName {  get; set; }
        public double Price { get; set; }
        public double MPU { get; set; }
        public string ProductCount { get; set; }
        public int StoreId {  get; set; }
    }
}
