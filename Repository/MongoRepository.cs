using MongoDB.Bson;
using MongoDB.Driver;
using Section3Crud.Model;
using Section3Crud.Utility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public interface IMongoRepository
{
    Task<ReturnObject> GetAll();

    Task<ReturnObject> Get(string id);

    Task<ReturnObject> Insert(MongoCustomerModel entity);
}
public class MongoRepository : IMongoRepository
{
    private readonly IMongoCollection<MongoCustomerModel> _collection;
    private readonly IConfiguration _config;
    public MongoRepository(IConfiguration config)
    {
        _config = config;

        string? connectionURI = _config.GetSection("MongoDB").GetSection("ConnectionURI").Value;
        string? databaseName = _config.GetSection("MongoDB").GetSection("DatabaseName").Value;

        MongoClient client = new(connectionURI);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<MongoCustomerModel>(GetCollectionName(typeof(MongoCustomerModel)));
    }

    public async Task<ReturnObject> Get(string id)
    {
        var r = new ReturnObject();
        var objectId = new ObjectId(id);
        var filter = Builders<MongoCustomerModel>.Filter.Eq(doc => doc.CustomerId, objectId);
        var eget = await _collection.Find(filter).FirstOrDefaultAsync();
        r.status = eget != null ? true : false;
        r.message = eget != null ? "Record Fetched Successfully" : "No Record Found";
        r.data = eget != null ? eget : null;
        return r;
    }

    public async Task<ReturnObject> GetAll()
    {
        var r = new ReturnObject();
        var eget = await _collection.AsQueryable().ToListAsync();
        r.status = eget.Any() ? true : false;
        r.message = eget.Any() ? "Record Fetched Successfully" : "No Record Found";
        r.data = eget.Any() ? eget : new List<MongoCustomerModel>();
        return r;
    }

    public async Task<ReturnObject> Insert(MongoCustomerModel entity)
    {
        try
        {
            await Task.Run(() => _collection.InsertOneAsync(entity));
            return new ReturnObject { status = true, message = "Record Saved Successfully." };
        }
        catch (Exception ex)
        {
            // writing the error to a text file on the server 
            ErrorLog.SendErrorToText(ex);
            return new ReturnObject { status = false, message = ex.Message };
        }
    }

    protected string? GetCollectionName(Type documentType)
    {
        return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute),
                true)
            .FirstOrDefault())?.CollectionName;
    }

}
