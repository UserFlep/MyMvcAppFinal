using Microsoft.EntityFrameworkCore;
using MyMvcAppFinal.Data;
using MyMvcAppFinal.Models;

namespace MyMvcAppFinal.Services
{
    public interface IUnitService
    {
        public DbSet<Unit> Units();
        public Task<List<Unit>> Index();
        public Task<Unit?> Details(int id);
        public Task<int> Create(Unit unit);
        public ValueTask<Unit?> GetUnitById(int id);
        public Task<int> Edit(Unit unit);
        public Task<int> DeleteConfirmed(Unit unit);
        public bool UnitExists(int id);
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

        public DbSet<Unit> Units() {
            return _context.Units;
        }

        public Task<List<Unit>> Index() {
            return _context.Units.Include(u => u.Parent).ToListAsync();
        }

        public Task<Unit?> Details(int id)
        {
            return _context.Units
                .Include(u => u.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public Task<int> Create(Unit unit)
        {
            _context.Add(unit);
            return _context.SaveChangesAsync();
        }

        public ValueTask<Unit?> GetUnitById(int id)
        {
            return _context.Units.FindAsync(id);
        }

        public Task<int> Edit(Unit unit)
        {
            _context.Update(unit);
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteConfirmed(Unit unit)
        {
            _context.Units.Remove(unit);
            return _context.SaveChangesAsync();
        }

        public bool UnitExists(int id)
        {
            return _context.Units.Any(e => e.Id == id);
        }
    }
}
