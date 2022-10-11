using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MyMvcAppFinal.Data;
using MyMvcAppFinal.Models;
using MyMvcAppFinal.Models.DTO;
using Newtonsoft.Json;

namespace MyMvcAppFinal.Services
{
    public interface IUnitService
    {
        public DbSet<Unit> Units();
        public Task<int> Synchronize(List<DeserializeUnitDto> localUnits);
        public Task<List<UnitDto>> Index();
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
        private readonly IStatusService _statusService;

        public UnitService(UnitContext context, IStatusService statusService)
        {
            _context = context;
            _statusService = statusService;
        }

        public DbSet<Unit> Units() {
            return _context.Units;
        }

        public Task<int> Synchronize(List<DeserializeUnitDto> localUnits) {
            foreach (var localUnit in localUnits) {
               
                var existUnit = _context.Units.Where(el => el.Name == localUnit.Name).FirstOrDefault();
                              
                //Элемент есть в бд
                if (existUnit != null)
                {
                    //Родитель в бд совпадает с родителем в файле
                    if (localUnit.Name != existUnit.Name) { continue; }
                    //Родители не совпадают, меняем родителя в бд
                    else
                    {
                        var localUnitParent = _context.Units.Where(el => el.Name == localUnit.ParentName).FirstOrDefault();
                        existUnit.ParentId = localUnitParent?.Id;
                    }

                }
                //Элемента нет в бд, добавляем
                else
                {
                    var localUnitParent = _context.Units.Where(el => el.Name == localUnit.ParentName).FirstOrDefault();
                    var newUnit = new Unit()
                    {
                        Name = localUnit.Name,
                        ParentId = localUnitParent?.Id
                    };
                    _context.Units.Add(newUnit);
                    _context.SaveChanges();
                }
            }
            return _context.SaveChangesAsync();
        }

        public async Task<List<UnitDto>> Index() {
            var units = await _context.Units.Include(u => u.Parent).ToListAsync();
            var statusedUnits = new List<UnitDto>(units.Select(unit => new UnitDto() { Id = unit.Id, Name = unit.Name, Status = _statusService.GetStatus(), Parent = unit.Parent ?? new Unit() {Name="-" } }));
            return statusedUnits;
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
