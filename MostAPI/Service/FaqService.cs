using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MostAPI.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FaqService
{
    private readonly IMongoCollection<Faq> _faqs;

    public FaqService(IMongoCollection<Faq> faqs)
    {
        _faqs = faqs;
    }

    // Получить все записи (без пагинации)
    public async Task<List<Faq>> GetAllAsync() =>
        await _faqs.Find(f => true).ToListAsync();

    // Получить 5 случайных записей
    public async Task<List<Faq>> GetRandomAsync(int count = 5)
    {
        return await _faqs.AsQueryable()
                          .OrderBy(_ => Guid.NewGuid()) // Случайная сортировка
                          .Take(count)
                          .ToListAsync();
    }

    // Получить запись по ID
    public async Task<Faq> GetByIdAsync(int id)
    {
        return await _faqs.Find(f => f.Id == id).FirstOrDefaultAsync();
    }

    // Создать новую запись
    public async Task CreateAsync(Faq faq) =>
        await _faqs.InsertOneAsync(faq);

    // Обновить запись
    public async Task UpdateAsync(int id, Faq faq)
    {
        await _faqs.ReplaceOneAsync(f => f.Id == id, faq);
    }

    // Удалить запись
    public async Task DeleteAsync(int id)
    {
        await _faqs.DeleteOneAsync(f => f.Id == id);
    }
}
