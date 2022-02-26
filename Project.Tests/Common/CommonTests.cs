using project.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Common;

namespace project.Tests
{
    [TestClass]
    public class CommonTests
    {

        public CommonTests()
        {

        }

        [TestMethod]
        public void HashPassword()
        {
            string passwordToHash = "passwd";

            string hashedPassword = Helpers.HashPassword(passwordToHash);

            Assert.IsTrue(passwordToHash != hashedPassword && hashedPassword == "zTl+pF+wQaw3VbGoV41B5AEkKqsLXtcFWpXTkCf+/Gs=");
        }
    }
}
