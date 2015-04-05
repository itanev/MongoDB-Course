using MongoDB.Bson;

namespace MongoCourseHW_03_EX_01
{
    public class ScoreRecord
    {
        public string Type { get; set; }
        public BsonDouble Score { get; set; }
    }
}