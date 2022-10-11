using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyMvcAppFinal.Models.DTO
{
    public class UnitDto
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Родитель")]
        public Unit Parent { get; set; }
        [Display(Name = "Статус")]
        public string Status { get; set; }
    }
}
