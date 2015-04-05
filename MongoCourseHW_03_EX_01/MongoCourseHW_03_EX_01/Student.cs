using System.Collections.Generic;
using MongoDB.Bson;

namespace MongoCourseHW_03_EX_01
{
    public class Student
    {
        public BsonDouble Id { get; set; }
        public string Name { get; set; }
        public List<ScoreRecord> Scores { get; set; } 
    }
}
