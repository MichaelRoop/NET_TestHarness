using NUnit.Framework;
using Rhino.Mocks;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using TestCases.SpStateMachineTests.TestImplementations;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using TestCases.TestToolSet.Net;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class SpToolsTests {

        #region Data

        HelperLogReaderNet logReader = new HelperLogReaderNet();

        #endregion

        #region Setup

        [SetUp]
        public void TestSetup() {
            this.logReader.StartLogging();
        }

        [TearDown]
        public void TestTeardown() {
            this.logReader.StopLogging();
            this.logReader.Clear();
        }

        #endregion

        #region Test Data

        private readonly string className = "SpTools";

        #endregion
        
        #region GetIdString

        [Test]
        public void _0_GetIdString_OkFromCache() {

            Dictionary<int, string> cache = new Dictionary<int, string>();
            cache.Add(0, "Zero State");
            cache.Add(100, "One Hundred State");
            cache.Add(1000, "One Thousand State");

            string ret = "";
            bool converterCalled = false;
            TestHelpersNet.CatchUnexpected(() => {
                ret = SpTools.GetIdString(100, cache, (key) => { converterCalled = true; return "Blah"; });
            });
            Assert.IsFalse(converterCalled, "Converter should not have been called");
            Assert.AreEqual("One Hundred State", ret);
        }


        [Test]
        public void _0_GetIdString_OkAddedToCache() {

            Dictionary<int, string> cache = new Dictionary<int, string>();
            cache.Add(0, "Zero State");
            cache.Add(1000, "One Thousand State");

            // Not cached
            string ret = "";
            bool converterCalled = false;
            TestHelpersNet.CatchUnexpected(() => {
                ret = SpTools.GetIdString(100, cache, (key) => { converterCalled = true; return "One Hundred State"; });
            });
            Assert.IsTrue(converterCalled, "Converter should have been called");
            Assert.AreEqual("One Hundred State", ret);

            // Check if it is good and well cached by the method
            ret = "";
            converterCalled = false;
            TestHelpersNet.CatchUnexpected(() => {
                ret = SpTools.GetIdString(100, cache, (key) => { converterCalled = true; return "Blah!"; });
            });
            Assert.IsFalse(converterCalled, "Converter should have been called");
            Assert.AreEqual("One Hundred State", ret);
        }
        
        [Test]
        public void _51000_GetIdString_NullDictionary() {
            TestHelpersNet.CatchExpected(51000, this.className, "GetIdString", "Null currentStrings Argument", () => {
                SpTools.GetIdString(0, null, (key) => { return ""; });
            });
        }

        [Test]
        public void _51001_GetIdString_NullFunc() {
            TestHelpersNet.CatchExpected(51001, this.className, "GetIdString", "Null converterFunc Argument", () => {
                SpTools.GetIdString(0, new Dictionary<int,string>(), null);
            });
        }

        [Test]
        public void _51003_GetIdString_ConverterError() {
            TestHelpersNet.CatchExpected(51003, this.className, "GetIdString", "Error in Calling Id to String Converter Method", () => {
                SpTools.GetIdString(0, new Dictionary<int, string>(), (key) => { throw new Exception("Blah!"); });
            });
        }

        #endregion

        #region Transition Samples

        private ISpEventMessage validMsg = new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Tick);
        private ISpEventMessage validMsg2 = new MyBaseMsg(MyMsgType.DataStrMsg, MyEventType.Start);

        private ISpStateTransition<MyEventType> validTransition =
            new SpStateTransition<MyEventType>(SpStateTransitionType.SameState, null, new MyBaseMsg(MyMsgType.SimpleMsg, MyEventType.Tick));
        private ISpStateTransition<MyEventType> validTransition2 =
            new SpStateTransition<MyEventType>(SpStateTransitionType.Defered, null, new MyBaseMsg(MyMsgType.DataStrMsg, MyEventType.Start));

        #endregion

        #region RegisterTransition


        [Test]
        public void _0_RegisterTransition_Recoverable() {
            Dictionary<int,ISpStateTransition<MyEventType>> store = new Dictionary<int, ISpStateTransition<MyEventType>>();
            TestHelpersNet.CatchUnexpected(() => {
                SpTools.RegisterTransition("OnResult", MyEventType.Start, this.validTransition, store);
                SpTools.RegisterTransition("OnResult", MyEventType.Stop, this.validTransition2, store);
            });
            Assert.IsTrue(store.Keys.Contains((int)MyEventType.Start), "Missing key Start");
            Assert.IsTrue(store.Keys.Contains((int)MyEventType.Stop), "Missing key Start");
        }
        

        [Test]
        public void _51005_RegisterTransition_NullTransition() {
            TestHelpersNet.CatchExpected(51005, this.className, "RegisterTransition", "Null transition Argument", () => {
                SpTools.RegisterTransition("OnResult", MyEventType.Start, (ISpStateTransition<MyEventType>)null, new Dictionary<int, ISpStateTransition<MyEventType>>());
            });
        }


        [Test]
        public void _51006_RegisterTransition_NullDictionary() {
            TestHelpersNet.CatchExpected(51006, this.className, "RegisterTransition", "Null store Argument", () => {
                SpTools.RegisterTransition("OnResult", MyEventType.Start, this.validTransition, null);
            });
        }


        [Test]
        public void _51008_RegisterTransition_AlreadyRegistered() {
            int evVal = (int)MyEventType.Start;
            Dictionary<int, ISpStateTransition<MyEventType>> store = new Dictionary<int, ISpStateTransition<MyEventType>>();
            store.Add(evVal, this.validTransition);

            TestHelpersNet.CatchExpected(51008, this.className, "RegisterTransition", string.Format("Already Contain a 'OnResult' Transition for Id:{0}", evVal), () => {
                SpTools.RegisterTransition("OnResult", MyEventType.Start, this.validTransition, store);
            });
        }

        #endregion

        #region GetTransitionCloneFromStore

        [Test]
        public void _0_GetTransitionCloneFromStore_Ok() {
            Dictionary<int,ISpStateTransition<MyEventType>> store = new Dictionary<int, ISpStateTransition<MyEventType>>();
            TestHelpersNet.CatchUnexpected(() => {
                SpTools.RegisterTransition("OnResult", MyEventType.Tick, this.validTransition, store);
                SpTools.RegisterTransition("OnResult", MyEventType.Start, this.validTransition2, store);

            });

            ISpStateTransition<MyEventType> t = SpTools.GetTransitionCloneFromStore(store, this.validMsg2);
            Assert.AreEqual(t.NextState, this.validTransition2.NextState);
            Assert.AreEqual(t.ReturnMessage.EventId, this.validMsg2.EventId);
        }

        [Test]
        public void _0_GetTransitionCloneFromStore_CloneIsGood() {
            Dictionary<int,ISpStateTransition<MyEventType>> store = new Dictionary<int, ISpStateTransition<MyEventType>>();
            TestHelpersNet.CatchUnexpected(() => {
                SpTools.RegisterTransition("OnResult", MyEventType.Tick, this.validTransition, store);
                SpTools.RegisterTransition("OnResult", MyEventType.Start, this.validTransition2, store);
            });

            ISpStateTransition<MyEventType> t = SpTools.GetTransitionCloneFromStore(store, this.validMsg2);
            Assert.AreEqual(t.TransitionType, this.validTransition2.TransitionType);

            // TODO - determine what is transfered
            //Assert.AreEqual(t.ReturnMessage.EventId, this.validMsg2.EventId);

            t.TransitionType = SpStateTransitionType.ExitState;
            t.ReturnMessage = null;

            Assert.AreNotEqual(t.TransitionType, this.validTransition2.TransitionType);
            Assert.IsNotNull(this.validTransition2.ReturnMessage);
        }


        [Test]
        public void _51009_GetTransitionCloneFromStore_NullEventIdConverter() {
            TestHelpersNet.CatchExpected(51009, this.className, "GetTransitionCloneFromStore", "Null store Argument", () => {
                SpTools.GetTransitionCloneFromStore<MyEventType>(null, this.validMsg);
            });
        }

        [Test]
        public void _51010_GetTransitionCloneFromStore_NullEventMsg() {
            TestHelpersNet.CatchExpected(51010, this.className, "GetTransitionCloneFromStore", "Null eventMsg Argument", () => {
                Dictionary<int,ISpStateTransition<MyEventType>> store = new Dictionary<int, ISpStateTransition<MyEventType>>();
                //SpTools.RegisterTransition("OnResult", new SpIntToInt(22), this.validTransition, store);
                SpTools.RegisterTransition("OnResult", MyEventType.Tick, this.validTransition, store);
                SpTools.GetTransitionCloneFromStore(store, null);
            });
        }


        [Test]
        public void _51011_GetTransitionCloneFromStore_ErrorOnClone() {
            TestHelpersNet.CatchExpected(51011, this.className, "GetTransitionCloneFromStore", "Clone Exception", () => {
                ISpStateTransition<MyEventType> tr = MockRepository.GenerateMock<ISpStateTransition<MyEventType>>();
                tr.Expect(o => o.Clone()).Throw(new Exception("Clone Exception"));

                Dictionary<int,ISpStateTransition<MyEventType>> store = new Dictionary<int, ISpStateTransition<MyEventType>>();
                SpTools.RegisterTransition("OnResult", MyEventType.Tick, tr, store);
                SpTools.RegisterTransition("OnResult", MyEventType.Start, this.validTransition, store);
                SpTools.GetTransitionCloneFromStore(store, this.validMsg);
            });
        }

        #endregion



    }
}
