using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.DataLayer;
using Server.DataLayer.Queries;

namespace ServerTests.Database.Queries
{
    class TestClass : DBEntity
    {
        public string Name { get; set; }
    }

    [TestClass]
    public class SelectQueryTests
    {
        [TestMethod]
        public void SelectQueryTest_NoOverrides()
        {
            var query = new SelectQuery<TestClass>();

            Assert.AreEqual("SELECT * FROM SERVERTESTS_DATABASE_QUERIES_TESTCLASS", query.GenerateSql().ToUpper());
        }

        [TestMethod]
        public void SelectQueryTest_Overrides()
        {
            var query = new SelectQuery<TestClass>().Select(nameof(TestClass.Name));

            Assert.AreEqual("SELECT NAME FROM SERVERTESTS_DATABASE_QUERIES_TESTCLASS", query.GenerateSql().ToUpper());
        }

        [TestMethod]
        public void SelectQueryTest_One_And_Filter()
        {
            var query = new SelectQuery<TestClass>().AndEquals(nameof(TestClass.Name), "ABC");

            Assert.AreEqual("SELECT * FROM SERVERTESTS_DATABASE_QUERIES_TESTCLASS WHERE NAME = 'ABC'", query.GenerateSql().ToUpper());
        }

        [TestMethod]
        public void SelectQueryTest_Two_And_Filter()
        {
            var query = new SelectQuery<TestClass>().AndEquals(nameof(TestClass.Name), "ABC").AndEquals(nameof(TestClass.Id), "23");

            Assert.AreEqual("SELECT * FROM SERVERTESTS_DATABASE_QUERIES_TESTCLASS WHERE NAME = 'ABC' AND ID = 23", query.GenerateSql().ToUpper());
        }

        [TestMethod]
        public void SelectQueryTest_Limit()
        {
            var query = new SelectQuery<TestClass>().Limit(32);

            Assert.AreEqual("SELECT TOP 32 * FROM SERVERTESTS_DATABASE_QUERIES_TESTCLASS", query.GenerateSql().ToUpper());
        }
    }
}
