using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


[BsonIgnoreExtraElements]
[BsonCollection("Customer")]
public class MongoCustomerModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId CustomerId { get; set; } = ObjectId.GenerateNewId();
    public string Name { get; set; }
    public string Email { get; set; }
    // Just added to track when items are Added and by who for tracking thou not part of the listed model in the assessment
    public int CreatedBy { get; set; }
    public DateTime DateCreated { get; set; }
}

//to get the name of the collection/table in mongo
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
    public string CollectionName { get; }

    public BsonCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}