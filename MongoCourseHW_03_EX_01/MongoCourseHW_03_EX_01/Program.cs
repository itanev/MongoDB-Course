using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoCourseHW_03_EX_01
{
    public class Program
    {
        static void Main()
        {
            MainFunc().GetAwaiter().GetResult();
        }

        public static async Task MainFunc()
        {
            BsonClassMap.RegisterClassMap<Student>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(x => x.Id).SetElementName("_id");
                cm.MapMember(x => x.Name).SetElementName("name");
                cm.MapMember(x => x.Scores).SetElementName("scores");
            });

            BsonClassMap.RegisterClassMap<ScoreRecord>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(x => x.Type).SetElementName("type");
                cm.MapMember(x => x.Score).SetElementName("score");
            });

            const string connectionString = "mongodb://localhost:12345";
            var client = new MongoClient(connectionString);

            var db = client.GetDatabase("school");
            var students = db.GetCollection<Student>("students");

            students.Find(new BsonDocument("scores.type", "homework")).Sort(new BsonDocument("scores.score", -1));
            await students.UpdateManyAsync(new BsonDocument(), new BsonDocument("$pop", new BsonDocument("scores", 1)));
        }
    }
}
