
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Section3Crud.Model;
using Section3Crud.Utility;

public interface IMsSqlRepository
{
    Task<ReturnObject> GetAll();

    Task<ReturnObject> Get(int id);

    Task<ReturnObject> Insert(Product entity);
}
public class MsSqlRepository : IMsSqlRepository
{
    private readonly ApiDbContext _collection;
    private readonly IConfiguration _config;
    public MsSqlRepository(IConfiguration config, ApiDbContext collection)
    {
        _config = config;
        _collection = collection;
    }

    public async Task<ReturnObject> Get(int id)
    {
        var r = new ReturnObject();
        var eget = await _collection.Products.FirstOrDefaultAsync(o => o.ProductId == id);
        r.status = eget != null ? true : false;
        r.message = eget != null ? "Record Fetched Successfully" : "No Record Found";
        r.data = eget != null ? eget : null;
        return r;
    }

    public async Task<ReturnObject> GetAll()
    {
        var r = new ReturnObject();
        var eget = await _collection.Products.ToListAsync();
        r.status = eget.Any() ? true : false;
        r.message = eget.Any() ? "Record Fetched Successfully" : "No Record Found";
        r.data = eget.Any() ? eget : new List<Product>();
        return r;
    }

    public async Task<ReturnObject> Insert(Product entity)
    {
        try
        {
            _collection.Products.AddAsync(entity);
            await _collection.SaveChangesAsync();
            return new ReturnObject { status = true, message = "Record Saved Successfully." };
        }
        catch (Exception ex)
        {
            // writing the error to a text file on the server 
            ErrorLog.SendErrorToText(ex);
            return new ReturnObject { status = false, message = ex.Message };
        }
    }


}