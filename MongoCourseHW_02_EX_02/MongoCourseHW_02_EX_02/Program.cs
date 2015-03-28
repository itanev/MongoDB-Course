using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoCourseHW_02_EX_02
{
    public class Program
    {
        static void Main()
        {
            MainFunc().GetAwaiter().GetResult();
        }

        public static async Task MainFunc()
        {
            BsonClassMap.RegisterClassMap<Grade>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(x => x.StudentId).SetElementName("student_id");
                cm.MapMember(x => x.Type).SetElementName("type");
                cm.MapMember(x => x.Score).SetElementName("score");
            });

            const string connectionString = "mongodb://localhost:12345";
            var client = new MongoClient(connectionString);

            var db = client.GetDatabase("students");
            var grades = db.GetCollection<Grade>("grades");

            var homeworkGrades = await grades.Find(new BsonDocument("type", "homework"))
                .SortBy(x => x.StudentId)
                .ThenBy(x => x.Score)
                .ToListAsync();

            for (int i = 0; i < homeworkGrades.Count; i += 2)
            {
                var grade = homeworkGrades[i];
                await grades.DeleteOneAsync(x => x.Id == grade.Id);
            }
        }
    }
}
