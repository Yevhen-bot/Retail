namespace API.DTOs
{
    public class BringGoodsModel
    {
        public string ProductName { get; set; }
        public double MPU { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int BuildingId {  get; set; }
    }
}
