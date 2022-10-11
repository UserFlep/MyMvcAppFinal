namespace MyMvcAppFinal.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Unit Parent { get; set; }
        public int? ParentId { get; set; }
        public ICollection<Unit>? SubUnits { get; } = new List<Unit>();
        
    }
}
