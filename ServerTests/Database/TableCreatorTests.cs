using DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ServerTests.Database
{
    class TestType : DBEntity
    {
        [DbLength(50)]
        public string Name { get; set; }
        public int Height { get; set; }
        public DateTime Started { get; set; }
        public long Weight { get; set; }
        public string Pw { get; set; }
    }

    [TestClass]
    public class TableCreatorTests
    {
        [TestMethod]
        public void createTableTest()
        {
            var command = TableCreator.GenerateCreateTableCommand<TestType>();

            string expected = "CREATE TABLE SERVERTESTS_DATABASE_TESTTYPE (ID BIGINT IDENTITY(1,1) PRIMARY KEY,NAME VARCHAR(50),HEIGHT INT,STARTED DATETIME2(3),WEIGHT BIGINT,PW VARCHAR(1000));";

            Assert.AreEqual(expected, command.ToUpper());
        }
    }
}
