//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using ChkUtils;
//using System.Diagnostics;
//using System.Reflection;

//namespace TestCases.ChkUtilsTests {

//    [TestFixture]
//    public class StackFrameToolsTests {

//        #region ClassName

//        [Test]
//        public void ClassName_nullFrame() {
//            Assert.AreEqual("NoClassName", StackFrameTools.ClassName(null));
//        }

//        [Test]
//        public void ClassName_valid() {
//            Assert.AreEqual("StackFrameToolsTests", StackFrameTools.ClassName(new StackTrace().GetFrame(0)));
//        }
        
//        #endregion

//        #region MethodName

//        [Test]
//        public void MethodName_nullFrame() {
//            Assert.AreEqual("NoMethodName", StackFrameTools.MethodName(null));
//        }

//        [Test]
//        public void MethodName_valid() {
//            Assert.AreEqual("MethodName_valid", StackFrameTools.MethodName(new StackTrace().GetFrame(0)));
//        }

//        #endregion

//        #region FileName

//        [Test]
//        public void FileName_nullFrame() {
//            Assert.AreEqual("NoFileName", StackFrameTools.FileName(null));
//        }

//        [Test]
//        public void FileName_OutOfBoundsIndex() {
//            Assert.AreEqual("NoFileName", StackFrameTools.FileName(new StackTrace().GetFrame(30000)));
//        }

//        // TODO - figure out why this does not work
//        //[Test]
//        //public void FileName_valid() {
//        //    Assert.AreEqual("StackFrameToolsTests.cs", StackFrameTools.FileName(new StackTrace().GetFrame(0)));
//        //}

//        #endregion

//        #region LineNumber

//        [Test]
//        public void LineNumber_nullFrame() {
//            Assert.AreEqual(0, StackFrameTools.Line(null));
//        }


//        #endregion

//        #region FirstNonWrappedMethod

//        [Test]
//        public void FirstNonWrappedMethod_valid() {
//            this.TestMethodBase("FirstNonWrappedMethod_valid", StackFrameTools.FirstNonWrappedMethod(typeof(WrapErr)));
//        }

//        [Test]
//        public void FirstNonWrappedMethod_nullType() {
//            this.TestMethodBase("FirstNonWrappedMethod_nullType", StackFrameTools.FirstNonWrappedMethod(null));
//        }

//        [Test]
//        public void FirstNonWrappedMethod_caught() {
//            // The catcher calls the FirstNonWrappedMethod and tells it to ignore any of its class
//            // methods in order to retrieve the first method beyond it
//            this.TestMethodBase("FirstNonWrappedMethod_caught", new ExceptionCatcher().DoIt());
//        }

//        private void TestMethodBase(string method, MethodBase mb) {
//            Console.WriteLine("{0}.{1}", mb.DeclaringType.Name, mb.Name);
//            Assert.AreEqual(method, mb.Name);
//            Assert.AreEqual("StackFrameToolsTests", mb.DeclaringType.Name);
//        }

//        public class ExceptionThrower {
//            public void DoIt() {
//                throw new Exception("Blah");
//            }
//        }

//        public class ExceptionCatcher {

//            public MethodBase DoIt3() {
//                try {
//                    new ExceptionThrower().DoIt();
//                    Assert.Fail("It should not have gotten here - exception not thrown");
//                    return StackFrameTools.FirstNonWrappedMethod(null);
//                }
//                catch (Exception) {
//                    // Ignore all methods from this type to get the first method above it
//                    return StackFrameTools.FirstNonWrappedMethod(this.GetType());
//                }
//            }

//            public MethodBase DoIt2()   { return this.DoIt3(); }
//            public MethodBase DoIt()    { return this.DoIt2(); }
//        }


//        #endregion

//    }
//}
