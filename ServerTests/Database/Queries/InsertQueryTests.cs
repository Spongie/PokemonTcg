using DataLayer;
using DataLayer.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServerTests.Database.Queries
{
    class TestInsertEntity : DBEntity
    {
        public string Name { get; set; }
        public int Height { get; set; }
    }

    [TestClass]
    public class InsertQueryTests
    {
        [TestMethod]
        public void InsertQuery_Test()
        {
            var result = new InsertQuery<TestInsertEntity>(new TestInsertEntity
            {
                Name = "PHILIP",
                Height = 44
            }).GenerateSql();


            var expected = "INSERT INTO SERVERTESTS_DATABASE_QUERIES_TESTINSERTENTITY (NAME,HEIGHT) VALUES ('PHILIP',44)";
            Assert.AreEqual(expected, result.ToUpper());
        }
    }
}
