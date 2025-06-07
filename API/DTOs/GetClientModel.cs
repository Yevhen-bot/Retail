using Core.ValueObj;

namespace API.DTOs
{
    public class GetClientModel
    {
        public int Id { get; set; }
        public Name Name { get; set; }
        public Age Age { get; set; }
        public Salary Salary { get; set; }
        public Email Email { get; set; }
        public decimal Money { get; set; }
        public List<GetProductModel> Preferences { get; set; }
    }
}