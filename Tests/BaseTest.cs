using ClassicalAlgorithmsKPI.Helpers;
using NUnit.Framework;

namespace Tests
{
    class BaseTest
    {
    }

    [SetUpFixture]
    class TestSession
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            SQLiteClient.GenerateTestDataDb();
        }
    }
}
