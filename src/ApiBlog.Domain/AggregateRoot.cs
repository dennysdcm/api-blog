using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiBlog.Domain;

public class AggregateRoot<TKey>
{
    [BsonRepresentation(BsonType.ObjectId)]
    public TKey? Id { get; protected set; }
}