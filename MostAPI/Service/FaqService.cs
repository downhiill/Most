using MongoDB.Driver;
using MongoDB.Bson;
using MostAPI.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

    // Получить запись по ID
    public async Task<Faq> GetByIdAsync(string id)
    {
        // Преобразуем строковый ID в ObjectId
        var objectId = ObjectId.Parse(id);
        return await _faqs.Find(f => f.Id == objectId).FirstOrDefaultAsync();
    }

    // Создать новую запись
    public async Task CreateAsync(Faq faq) =>
        await _faqs.InsertOneAsync(faq);

    // Обновить запись
    public async Task UpdateAsync(string id, Faq faq)
    {
        var objectId = ObjectId.Parse(id);
        await _faqs.ReplaceOneAsync(f => f.Id == objectId, faq);
    }

    // Удалить запись
    public async Task DeleteAsync(string id)
    {
        var objectId = ObjectId.Parse(id);
        await _faqs.DeleteOneAsync(f => f.Id == objectId);
    }
}
