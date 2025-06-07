using Data_Access.Adapters;
using Data_Access.Entities;

namespace API.DTOs
{
    public class GetBuildingModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Adress { get; set; } = null!;
        public double Area { get; set; }
        public string Type { get; set; } = null!;
        public List<GetProductModel> Products { get; set; } = [];
        public List<GetWorkerModel> Workers { get; set; } = [];
        public List<GetClientModel> Clients { get; set; } = [];
    }
}
