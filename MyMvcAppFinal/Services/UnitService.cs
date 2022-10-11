using MyMvcAppFinal.Data;
using MyMvcAppFinal.Models;

namespace MyMvcAppFinal.Services
{
    public interface IUnitService
    {
        
    }

    public class UnitService : IUnitService
    {
        private readonly UnitContext _context;
        private readonly IStatusService _StatusService;

        public UnitService(UnitContext context, IStatusService statusService)
        {
            _context = context;
            _StatusService = statusService;
        }
    }
}
