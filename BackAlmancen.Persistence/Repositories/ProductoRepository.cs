using BackAlmancen.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BackAlmancen.Persistence.Repositories;

public class ProductoRepository: IRepository<Producto>
{

    private readonly ContextDB _context;

    public ProductoRepository(ContextDB context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Producto>> GetAllAsync()
    {
        return await _context.Producto.AsNoTracking().ToListAsync();
    }

    public async Task<Producto?> GetByIdAsync(int id)
    {
        return await _context.Producto.FindAsync(id);
    }

    public async Task<Producto> AddAsync(Producto entity)
    {
        await _context.Producto.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Producto entity)
    {
        var existing = await _context.Producto.FindAsync(entity.Id);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Producto.FindAsync(id);
        if (product == null) return false;

        _context.Producto.Remove(product);
        return await _context.SaveChangesAsync() > 0;
    }
}
