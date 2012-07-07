using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestCases.TestToolSet;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using SpStateMachine.Converters;
using Rhino.Mocks;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class SpToolsTests {

        #region Data

        HelperLogReader logReader = new HelperLogReader();

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
            TestHelpers.CatchUnexpected(() => {
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
            TestHelpers.CatchUnexpected(() => {
                ret = SpTools.GetIdString(100, cache, (key) => { converterCalled = true; return "One Hundred State"; });
            });
            Assert.IsTrue(converterCalled, "Converter should have been called");
            Assert.AreEqual("One Hundred State", ret);

            // Check if it is good and well cached by the method
            ret = "";
            converterCalled = false;
            TestHelpers.CatchUnexpected(() => {
                ret = SpTools.GetIdString(100, cache, (key) => { converterCalled = true; return "Blah!"; });
            });
            Assert.IsFalse(converterCalled, "Converter should have been called");
            Assert.AreEqual("One Hundred State", ret);
        }
        
        [Test]
        public void _51000_GetIdString_NullDictionary() {
            TestHelpers.CatchExpected(51000, this.className, "GetIdString", "Null currentStrings Argument", () => {
                SpTools.GetIdString(0, null, (key) => { return ""; });
            });
        }

        [Test]
        public void _51001_GetIdString_NullFunc() {
            TestHelpers.CatchExpected(51001, this.className, "GetIdString", "Null converterFunc Argument", () => {
                SpTools.GetIdString(0, new Dictionary<int,string>(), null);
            });
        }

        [Test]
        public void _51003_GetIdString_ConverterError() {
            TestHelpers.CatchExpected(51003, this.className, "GetIdString", "Error in Calling Id to String Converter Method", () => {
                SpTools.GetIdString(0, new Dictionary<int, string>(), (key) => { throw new Exception("Blah!"); });
            });
        }

        #endregion

        #region Transition Samples

        private ISpEventMessage validMsg = new SpBaseEventMsg(new SpIntToInt(2), new SpIntToInt(41));
        private ISpEventMessage validMsg2 = new SpBaseEventMsg(new SpIntToInt(3), new SpIntToInt(42));

        private ISpStateTransition validTransition = 
            new SpStateTransition(SpStateTransitionType.SameState, null, new SpBaseEventMsg(new SpIntToInt(22), new SpIntToInt(34)));
        private ISpStateTransition validTransition2 = 
            new SpStateTransition(SpStateTransitionType.Defered, null, new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(98)));

        #endregion

        #region RegisterTransition


        [Test]
        public void _0_RegisterTransition_Recoverable() {
            Dictionary<int,ISpStateTransition> store = new Dictionary<int, ISpStateTransition>();
            TestHelpers.CatchUnexpected(() => {
                SpTools.RegisterTransition("OnResult", new SpIntToInt(91), this.validTransition, store);
                SpTools.RegisterTransition("OnResult", new SpIntToInt(29), this.validTransition2, store);
            });
            Assert.IsTrue(store.Keys.Contains(91), "Missing key 91");
            Assert.IsTrue(store.Keys.Contains(29), "Missing key 29");
        }
        
        [Test]
        public void _51004_RegisterTransition_NullEventIdConverter() {
            TestHelpers.CatchExpected(51004, this.className, "RegisterTransition", "Null eventId Argument", () => {
                SpTools.RegisterTransition("OnResult", null, this.validTransition, new Dictionary<int,ISpStateTransition>());
            });
        }

        [Test]
        public void _51005_RegisterTransition_NullTransition() {
            TestHelpers.CatchExpected(51005, this.className, "RegisterTransition", "Null transition Argument", () => {
                SpTools.RegisterTransition("OnResult", new SpIntToInt(2), null, new Dictionary<int,ISpStateTransition>());
            });
        }


        [Test]
        public void _51006_RegisterTransition_NullDictionary() {
            TestHelpers.CatchExpected(51006, this.className, "RegisterTransition", "Null store Argument", () => {
                SpTools.RegisterTransition("OnResult", new SpIntToInt(2), this.validTransition, null);
            });
        }

        [Test]
        public void _51007_RegisterTransition_IdConverterFails() {
            ISpToInt toInt = MockRepository.GenerateMock<ISpToInt>();
            toInt.Expect((o) => o.ToInt()).Throw(new Exception("Woof Exception"));

            Dictionary<int,ISpStateTransition> store = new Dictionary<int,ISpStateTransition>();
            TestHelpers.CatchExpected(51007, this.className, "RegisterTransition", "Error on Event Id Converter for 'OnResult' Event Type", () => {
                SpTools.RegisterTransition("OnResult", toInt, this.validTransition, store);
            });
        }


        [Test]
        public void _51008_RegisterTransition_AlreadyRegistered() {
            Dictionary<int,ISpStateTransition> store = new Dictionary<int,ISpStateTransition>();
            store.Add(22, this.validTransition);

            TestHelpers.CatchExpected(51008, this.className, "RegisterTransition", "Already Contain a 'OnResult' Transition for Id:22", () => {
                SpTools.RegisterTransition("OnResult", new SpIntToInt(22), this.validTransition, store);
            });
        }



        #endregion 
        
        #region GetTransitionCloneFromStore

        [Test]
        public void _0_GetTransitionCloneFromStore_Ok() {
            Dictionary<int,ISpStateTransition> store = new Dictionary<int, ISpStateTransition>();
            TestHelpers.CatchUnexpected(() => {
                SpTools.RegisterTransition("OnResult", new SpIntToInt(this.validMsg.EventId), this.validTransition, store);
                SpTools.RegisterTransition("OnResult", new SpIntToInt(this.validMsg2.EventId), this.validTransition2, store);
            });

            ISpStateTransition t = SpTools.GetTransitionCloneFromStore(store, this.validMsg2);
            Assert.AreEqual(t.NextState, this.validTransition2.NextState);
            Assert.AreEqual(t.ReturnMessage.EventId, this.validMsg2.EventId);
        }

        [Test]
        public void _0_GetTransitionCloneFromStore_CloneIsGood() {
            Dictionary<int,ISpStateTransition> store = new Dictionary<int, ISpStateTransition>();
            TestHelpers.CatchUnexpected(() => {
                SpTools.RegisterTransition("OnResult", new SpIntToInt(this.validMsg.EventId), this.validTransition, store);
                SpTools.RegisterTransition("OnResult", new SpIntToInt(this.validMsg2.EventId), this.validTransition2, store);
            });

            ISpStateTransition t = SpTools.GetTransitionCloneFromStore(store, this.validMsg2);
            Assert.AreEqual(t.TransitionType, this.validTransition2.TransitionType);
            Assert.AreEqual(t.ReturnMessage.EventId, this.validMsg2.EventId);

            t.TransitionType = SpStateTransitionType.ExitState;
            t.ReturnMessage = null;

            Assert.AreNotEqual(t.TransitionType, this.validTransition2.TransitionType);
            Assert.IsNotNull(this.validTransition2.ReturnMessage);
        }

        [Test]
        public void _51009_GetTransitionCloneFromStore_NullEventIdConverter() {
            TestHelpers.CatchExpected(51009, this.className, "GetTransitionCloneFromStore", "Null store Argument", () => {
                SpTools.GetTransitionCloneFromStore(null, this.validMsg);
            });
        }

        [Test]
        public void _51010_GetTransitionCloneFromStore_NullEventMsg() {
            TestHelpers.CatchExpected(51010, this.className, "GetTransitionCloneFromStore", "Null eventMsg Argument", () => {
                Dictionary<int,ISpStateTransition> store = new Dictionary<int, ISpStateTransition>();
                SpTools.RegisterTransition("OnResult", new SpIntToInt(22), this.validTransition, store);
                SpTools.GetTransitionCloneFromStore(store, null);
            });
        }


        [Test]
        public void _51011_GetTransitionCloneFromStore_ErrorOnClone() {
            TestHelpers.CatchExpected(51011, this.className, "GetTransitionCloneFromStore", "Unexpected Error Occured", () => {
                ISpStateTransition tr = MockRepository.GenerateMock<ISpStateTransition>();
                tr.Expect(o => o.Clone()).Throw(new Exception("Clone Exception"));

                Dictionary<int,ISpStateTransition> store = new Dictionary<int, ISpStateTransition>();
                SpTools.RegisterTransition("OnResult", new SpIntToInt(this.validMsg.EventId), tr, store);
                SpTools.RegisterTransition("OnResult", new SpIntToInt(23), this.validTransition, store);

                SpTools.GetTransitionCloneFromStore(store, this.validMsg);
            });
        }

        #endregion



    }
}
