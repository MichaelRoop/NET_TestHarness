using NUnit.Framework;
using SpStateMachine.Core;
using SpStateMachine.EventStores;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using TestCases.TestToolSet;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class EventStoreTests {

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
        
        #region Base 

        [Test]
        public void _0_Base_MutilDisposed() {
            TestHelpers.CatchUnexpected(() => {
                ISpEventStore d = new SimpleDequeEventStore(new SpBaseEventMsg(25, 100));
                d.Dispose();
                d.Dispose();
                d.Dispose();
            });
        }

        [Test]
        public void _50110_Base_Constructor_NullParam() {
            TestHelpers.CatchExpected(50110, "BaseEventStore", ".ctor", "Null defaultTick Argument", () => {
                ISpEventStore d = new SimpleDequeEventStore(null);
            });
        }

        [Test]
        public void _50111_Base_Add_Disposed() {
            TestHelpers.CatchExpected(50111, "BaseEventStore", "Add", "Attempting to use Disposed Object", () => {
                ISpEventStore d = new SimpleDequeEventStore(new SpBaseEventMsg(25, 100));
                d.Dispose();
                d.Add(new SpBaseEventMsg(25, 100));
            });
        }

        [Test]
        public void _50112_Base_Add_NullMsg() {
            TestHelpers.CatchExpected(50112, "BaseEventStore", "Add", "Null msg Argument", () => {
                ISpEventStore d = new SimpleDequeEventStore(new SpBaseEventMsg(25, 100));
                d.Add(null);
            });
        }

        [Test]
        public void _50113_Base_Get_Disposed() {
            TestHelpers.CatchExpected(50113, "BaseEventStore", "Get", "Attempting to use Disposed Object", () => {
                ISpEventStore d = new SimpleDequeEventStore(new SpBaseEventMsg(25, 100));
                d.Dispose();
                d.Get();
            });
        }
        
        #endregion

        #region SimpleDequeQueue

        [Test]
        public void _0_SimpleDequeQueue_Add() {
            TestHelpers.CatchUnexpected(() => {
                ISpEventStore d = new SimpleDequeEventStore(new SpBaseEventMsg(25, 100));
                d.Add(new SpBaseEventMsg(25, 100));
                d.Add(new SpBaseEventMsg(25, 100));
                d.Add(new SpBaseEventMsg(25, 100));
            });
        }

        [Test]
        public void _0_SimpleDequeQueue_AddSequencing() {
            ISpEventStore d = null;
            TestHelpers.CatchUnexpected(() => {
                d = new SimpleDequeEventStore(new SpBaseEventMsg(25, 100));
                d.Add(new SpBaseEventMsg(1, 101));
                d.Add(new SpBaseEventMsg(2, 102));
                d.Add(new SpBaseEventMsg(3, 103));
            });

            this.MsgEqual(new SpBaseEventMsg(1, 101), d.Get());
            this.MsgEqual(new SpBaseEventMsg(2, 102), d.Get());
            this.MsgEqual(new SpBaseEventMsg(3, 103), d.Get());
            this.MsgEqual(new SpBaseEventMsg(25, 100), d.Get());
            
            TestHelpers.CatchUnexpected(() => {
                d.Dispose();
            });
        }

        #endregion

        #region Priority Queue

        [Test]
        public void _0_PriorityEventStore_Add() {
            TestHelpers.CatchUnexpected(() => {
                ISpEventStore d = new SimpleDequeEventStore(new SpBaseEventMsg(25, 100));
                d.Add(new SpBaseEventMsg(1, 100, SpEventPriority.Undefined));
                d.Add(new SpBaseEventMsg(1, 100, SpEventPriority.High));
                d.Add(new SpBaseEventMsg(1, 100, SpEventPriority.Normal));
                d.Add(new SpBaseEventMsg(1, 100, SpEventPriority.Low));
            });
        }
        
        [Test]
        public void _0_PriorityEventStore_AddSequence() {
            ISpEventStore d = null;
            TestHelpers.CatchUnexpected(() => {
                d = new PriorityEventStore(new SpBaseEventMsg(25, 100));
                d.Add(new SpBaseEventMsg(1, 100, SpEventPriority.Low));
                d.Add(new SpBaseEventMsg(1, 101, SpEventPriority.Low));

                d.Add(new SpBaseEventMsg(1, 102, SpEventPriority.Normal));
                d.Add(new SpBaseEventMsg(1, 103, SpEventPriority.Normal));

                d.Add(new SpBaseEventMsg(1, 104, SpEventPriority.High));
                d.Add(new SpBaseEventMsg(1, 105, SpEventPriority.High));

                d.Add(new SpBaseEventMsg(1, 106, SpEventPriority.Urgent));
                d.Add(new SpBaseEventMsg(1, 107, SpEventPriority.Urgent));
            });

            // Validate sequence by priority and sequence within priority

            this.MsgEqual(new SpBaseEventMsg(1, 106, SpEventPriority.Urgent), d.Get());
            this.MsgEqual(new SpBaseEventMsg(1, 107, SpEventPriority.Urgent), d.Get());

            this.MsgEqual(new SpBaseEventMsg(1, 104, SpEventPriority.High), d.Get());
            this.MsgEqual(new SpBaseEventMsg(1, 105, SpEventPriority.High), d.Get());

            this.MsgEqual(new SpBaseEventMsg(1, 102, SpEventPriority.Normal), d.Get());
            this.MsgEqual(new SpBaseEventMsg(1, 103, SpEventPriority.Normal), d.Get());

            this.MsgEqual(new SpBaseEventMsg(1, 100, SpEventPriority.Low), d.Get());
            this.MsgEqual(new SpBaseEventMsg(1, 101, SpEventPriority.Low), d.Get());

            this.MsgEqual(new SpBaseEventMsg(25, 100), d.Get());

            TestHelpers.CatchUnexpected(() => {
                d.Dispose();
            });
        }


        [Test]
        public void _50150_PriorityEventStore_AddEvent_UnhandledPriority() {
            TestHelpers.CatchExpected(50150, "PriorityEventStore", "AddEvent", "The Priority Type 'Undefined' is not Handled", () => {
                ISpEventStore d = new PriorityEventStore(new SpBaseEventMsg(25, 100));
                d.Add(new SpBaseEventMsg(1, 1, SpEventPriority.Undefined));
            });
        }

        #endregion

        #region Private Methods

        private void MsgEqual(ISpEventMessage expected, ISpEventMessage actual) {
            Assert.IsNotNull(actual, "Current message null");
            Assert.AreEqual(expected.EventId, actual.EventId, "Event Id Mismatch");
            Assert.AreEqual(expected.Priority, actual.Priority, "Priority Id Mismatch");
            Assert.AreEqual(expected.TypeId, actual.TypeId, "Type Id Mismatch");
        }
        
        #endregion

    }
}
