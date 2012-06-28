using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using LogUtils;

namespace TestCases.LogUtilsTests {

    [TestFixture]
    public class MsgLevelTests {

        #region ShortName extension

        [Test]
        public void ShortName_Info() {
            Assert.AreEqual("I", MsgLevel.Info.ShortName());
        }

        [Test]
        public void ShortName_Debug() {
            Assert.AreEqual("D", MsgLevel.Debug.ShortName());
        }

        [Test]
        public void ShortName_Error() {
            Assert.AreEqual("E", MsgLevel.Error.ShortName());
        }

        [Test]
        public void ShortName_Warning() {
            Assert.AreEqual("W", MsgLevel.Warning.ShortName());
        }

        [Test]
        public void ShortName_Critical() {
            Assert.AreEqual("C", MsgLevel.Critical.ShortName());
        }

        [Test]
        public void ShortName_Exception() {
            Assert.AreEqual("X", MsgLevel.Exception.ShortName());
        }

        #endregion

        #region GreaterOrEqual extension

        [Test]
        public void GreaterOrEqual_Debug_Info() {
            Assert.IsTrue(MsgLevel.Debug.GreaterOrEqual(MsgLevel.Info));
        }

        [Test]
        public void GreaterOrEqual_Debug_Debug() {
            Assert.IsTrue(MsgLevel.Debug.GreaterOrEqual(MsgLevel.Debug));
        }

        [Test]
        public void GreaterOrEqual_Debug_Warning() {
            Assert.IsFalse(MsgLevel.Debug.GreaterOrEqual(MsgLevel.Warning));
        }


        #endregion
    }
}
