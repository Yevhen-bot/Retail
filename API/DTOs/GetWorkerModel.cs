using Core.ValueObj;

namespace API.DTOs
{
    public class GetWorkerModel
    {
        public int Id { get; set; }
        public Name Name { get; set; }
        public Age Age { get; set; }
        public Email Email { get; set; }
        public Adress HomeAdress { get; set; }
        public Salary Salary { get; set; }
        public ExhaustionLevel ExaustionLevel { get; set; }
        public Role Role { get; set; }
    }
}