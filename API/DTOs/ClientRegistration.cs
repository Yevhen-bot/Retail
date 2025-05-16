namespace API.DTOs
{
    public class ClientRegistration
    {
        public RegisterModel Register { get; set; }
        public DateOnly BirthDate {  get; set; }
        public decimal Salary { get; set; }
        public decimal Money {  get; set; }
    }
}
