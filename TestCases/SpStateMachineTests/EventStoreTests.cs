using NUnit.Framework;
using SpStateMachine.Core;
using SpStateMachine.EventStores;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using SpStateMachine.Converters;
using TestCases.TestToolSet.Net;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class EventStoreTests {

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
            System.Threading.Thread.Sleep(500);
            this.logReader.Clear();
        }

        #endregion
        
        #region Base 

        [Test]
        public void _0_Base_MutilDisposed() {
            TestHelpersNet.CatchUnexpected(() => {
                ISpEventStore d = new SimpleDequeEventStore(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
                d.Dispose();
                d.Dispose();
                d.Dispose();
            });
        }

        [Test]
        public void _50110_Base_Constructor_NullParam() {
            TestHelpersNet.CatchExpected(50110, "BaseEventStore", ".ctor", "Null defaultTick Argument", () => {
                ISpEventStore d = new SimpleDequeEventStore(null);
            });
        }

        [Test]
        public void _50111_Base_Add_Disposed() {
            TestHelpersNet.CatchExpected(50111, "BaseEventStore", "Add", "Attempting to use Disposed Object", () => {
                ISpEventStore d = new SimpleDequeEventStore(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
                d.Dispose();
                d.Add(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
            });
        }

        [Test]
        public void _50112_Base_Add_NullMsg() {
            TestHelpersNet.CatchExpected(50112, "BaseEventStore", "Add", "Null msg Argument", () => {
                ISpEventStore d = new SimpleDequeEventStore(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
                d.Add(null);
            });
        }

        [Test]
        public void _50113_Base_Get_Disposed() {
            TestHelpersNet.CatchExpected(50113, "BaseEventStore", "Get", "Attempting to use Disposed Object", () => {
                ISpEventStore d = new SimpleDequeEventStore(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
                d.Dispose();
                d.Get();
            });
        }
        
        #endregion

        #region SimpleDequeQueue

        [Test]
        public void _0_SimpleDequeQueue_Add() {
            TestHelpers.CatchUnexpected(() => {
                ISpEventStore d = new SimpleDequeEventStore(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
                d.Add(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
                d.Add(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
                d.Add(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
            });
        }

        [Test]
        public void _0_SimpleDequeQueue_AddSequencing() {
            ISpEventStore d = null;
            TestHelpers.CatchUnexpected(() => {
                d = new SimpleDequeEventStore(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(101)));
                d.Add(new SpBaseEventMsg(new SpIntToInt(2), new SpIntToInt(102)));
                d.Add(new SpBaseEventMsg(new SpIntToInt(3), new SpIntToInt(103)));
            });

            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(101)), d.Get());
            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(2), new SpIntToInt(102)), d.Get());
            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(3), new SpIntToInt(103)), d.Get());
            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)), d.Get());
            
            TestHelpers.CatchUnexpected(() => {
                d.Dispose();
            });
        }

        #endregion

        #region Priority Queue

        [Test]
        public void _0_PriorityEventStore_Add() {
            TestHelpersNet.CatchUnexpected(() => {
                ISpEventStore d = new SimpleDequeEventStore(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(100), SpEventPriority.Undefined));
                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(100), SpEventPriority.High));
                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(100), SpEventPriority.Normal));
                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(100), SpEventPriority.Low));
            });
        }
        
        [Test]
        public void _0_PriorityEventStore_AddSequence() {
            ISpEventStore d = null;
            TestHelpersNet.CatchUnexpected(() => {
                d = new PriorityEventStore(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(100), SpEventPriority.Low));
                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(101), SpEventPriority.Low));

                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(102), SpEventPriority.Normal));
                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(103), SpEventPriority.Normal));

                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(104), SpEventPriority.High));
                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(105), SpEventPriority.High));

                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(106), SpEventPriority.Urgent));
                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(107), SpEventPriority.Urgent));
            });

            // Validate sequence by priority and sequence within priority

            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(106), SpEventPriority.Urgent), d.Get());
            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(107), SpEventPriority.Urgent), d.Get());

            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(104), SpEventPriority.High), d.Get());
            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(105), SpEventPriority.High), d.Get());

            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(102), SpEventPriority.Normal), d.Get());
            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(103), SpEventPriority.Normal), d.Get());

            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(100), SpEventPriority.Low), d.Get());
            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(101), SpEventPriority.Low), d.Get());

            this.MsgEqual(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)), d.Get());

            TestHelpersNet.CatchUnexpected(() => {
                d.Dispose();
            });
        }


        [Test]
        public void _50150_PriorityEventStore_AddEvent_UnhandledPriority() {
            TestHelpersNet.CatchExpected(50150, "PriorityEventStore", "AddEvent", "The Priority Type 'Undefined' is not Handled", () => {
                ISpEventStore d = new PriorityEventStore(new SpBaseEventMsg(new SpIntToInt(25), new SpIntToInt(100)));
                d.Add(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(1), SpEventPriority.Undefined));
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
