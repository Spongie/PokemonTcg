using DataLayer;
using DataLayer.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServerTests.Database.Queries
{
    class TestEntity : DBEntity
    {
        public string Name { get; set; }
    }

    [TestClass]
    public class UpdateQueryTests
    {
        [TestMethod]
        public void UpdateQuery_Test()
        {
            var result = new UpdateQuery<TestEntity>(new TestEntity
            {
                Id = 23,
                Name = "NEJ"
            }).GenerateSql();

            var expected = "UPDATE SERVERTESTS_DATABASE_QUERIES_TESTENTITY SET NAME = 'NEJ' WHERE ID = 23";

            Assert.AreEqual(expected, result.ToUpper());
        }
    }
}
