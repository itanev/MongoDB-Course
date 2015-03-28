using System;
using MongoDB.Bson;

namespace MongoCourseHW_02_EX_02
{
    public class Grade
    {
        public ObjectId Id { get; set; }
        public int StudentId { get; set; }
        public string Type { get; set; }
        public double Score { get; set; }

        public override string ToString()
        {
            return String.Format("Id: {0}, StudentId: {1}, Type: {2}, Score: {3}", Id, StudentId, Type, Score);
        }
    }
}
