using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using UAVTWebapi.Utils;

namespace UAVTWebapi.Test {
    [TestFixture]
    public class Tester {
        [Test]
        public void TestStringOperations()
        {
            var normalizedInput = Utilities.NormalizeForEnglish("ÇANKIRI");
            var lowerCasedFirstInput = Utilities.LowercaseFirstLetter("Çankırı");

            Assert.AreEqual("CANKIRI",normalizedInput);
            Assert.AreEqual("cankiri",lowerCasedFirstInput);
        }
    }
}