using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MostAPI.Data;
using MostAPI.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FaqService
{
    private readonly IMongoCollection<Faq> _faqs;

    public FaqService(IMongoCollection<Faq> faqs)
    {
        _faqs = faqs;
    }

    // Получить все записи (без пагинации, как раньше)
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

    public async Task<Faq> GetByIdAsync(string id) =>
        await _faqs.Find(f => f.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Faq faq) =>
        await _faqs.InsertOneAsync(faq);

    public async Task UpdateAsync(string id, Faq faq) =>
        await _faqs.ReplaceOneAsync(f => f.Id == id, faq);

    public async Task DeleteAsync(string id) =>
        await _faqs.DeleteOneAsync(f => f.Id == id);
}
