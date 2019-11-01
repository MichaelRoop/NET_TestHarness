using NUnit.Framework;
using Rhino.Mocks;
using SpStateMachine.Converters;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using SpStateMachine.States;
using System;
using TestCases.SpStateMachineTests.TestImplementations;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using TestCases.TestToolSet.Net;

namespace TestCases.SpStateMachineTests {

    #region Test class implementations

    //internal interface ITstObj : IDisposable {
    //}


    //internal class TstObj {
    //}

    public enum TSID {
        first = 1,
        third = 3,
    };

    public enum TMsg {

        _33 = 33,
    }




    public class StImpl<T,T2,T3,TMsg> : SpState<T,T2,T3,TMsg> where T : class, IDisposable where T2 : struct where T3 : struct where TMsg : struct {

        public StImpl(ISpState<T2> parent, ISpMsgFactory msgFactory, T3 id, T wo)
            : base(parent, msgFactory, id, wo) {
        }

        public StImpl(ISpMsgFactory msgFactory, T3 id, T wo)
            : this(null, msgFactory, id, wo) {
        }
    }

    #endregion

    [TestFixture]
    public class SpStateTests {

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

        #region Constructors

        [Test]
        public void _50200_Constructor_NullWrappedObject() {
            TestHelpersNet.CatchExpected(50200, "SpStateBase`4", ".ctor", "Null wrappedObject Argument", () => {
                ISpState<MyMsgId> st = new StImpl<IDisposable,MyMsgId,TSID,TMsg>(
                    MyDummyDI.MsgFactoryInstance, TSID.first, null);
            });
        }

        #endregion

        #region OnEntry

        [Test]
        public void _0_OnEntry_ExecutedTwiceWithOnExitBetween() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchUnexpected(() => {
                ISpState<MyMsgId> st = 
                    new StImpl<IDisposable,MyMsgId,TSID,TMsg>(
                        MyDummyDI.MsgFactoryInstance, TSID.first, wo);

                st.OnEntry(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
                st.OnExit();
                st.OnEntry(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
            });
        }


        [Test]
        public void _50201_OnEntry_ExecutedTwice() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50201, "SpStateBase`4", "OnEntry", "OnEntry Cannot be Executed More Than Once Until OnExit is Called", () => {
                ISpState<MyMsgId> st = 
                    new StImpl<IDisposable,MyMsgId,TSID,TMsg>(
                        MyDummyDI.MsgFactoryInstance, TSID.first, wo);

                st.OnEntry(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
                st.OnEntry(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
            });
        }

        #endregion

        #region OnTick

        [Test]
        public void _0_OnTick_AfterOnEntry() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchUnexpected(() => {
                ISpState<MyMsgId> st = 
                    new StImpl<IDisposable, MyMsgId, TSID,TMsg>(
                        MyDummyDI.MsgFactoryInstance, TSID.first, wo);
                st.OnEntry(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
                st.OnTick(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
            });
        }

        [Test]
        public void _50205_OnTick_WithoutOnEntry() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50205, "SpStateBase`4", "OnTick", "OnTick for 'first' State Cannot be Executed Before OnEntry", () => {
                ISpState<MyMsgId> st = new StImpl<IDisposable, MyMsgId,TSID,TMsg>(
                    MyDummyDI.MsgFactoryInstance, TSID.first, wo);
                st.OnTick(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
            });
        }

        #endregion

        #region InitStateIds

        class tstA {
        }

        class tstState : SpState<tstA,MyMsgId,TSID,int> {
            public tstState(ISpState<MyMsgId> parent, tstA o)
                : base(parent, MyDummyDI.MsgFactoryInstance, TSID.third, o) {
            }
        }

        class tstSuperState : SpSuperState<tstA,MyMsgId, TSID,int> {
            public tstSuperState(tstA o)
                : base(MyDummyDI.MsgFactoryInstance, TSID.first, o) {
            }
        }

        [Test]
        public void _0_() {
            tstA o = new tstA();
            ISpState<MyMsgId> ss = new tstSuperState(o);
            ISpState<MyMsgId> s = new tstState(ss, o);
            Console.WriteLine("SS name:{0} - FullName:{1}", ss.Name, ss.FullName);
            Console.WriteLine("S name:{0} - FullName:{1}", s.Name, s.FullName);
        }


        [Test]
        public void _50206_InitStateIds_ParentNullIdChain() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            ISpState<MyMsgId> parent = MockRepository.GenerateMock<ISpState<MyMsgId>>();
            parent.Expect(o => o.IdChain).IgnoreArguments().Return(null);

            TestHelpersNet.CatchExpected(50206, "SpStateBase`4", "InitStateIds", "The Parent has a Null Id Chain", () => {
                ISpState<MyMsgId> st = 
                    new StImpl<IDisposable, MyMsgId,TSID,TMsg>(
                        parent, MyDummyDI.MsgFactoryInstance, TSID.first, wo);
                st.OnEntry(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
                st.OnEntry(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
            });
        }


        [Test]
        public void _50207_InitStateIds_UnexpectedError() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            ISpState<MyMsgId> parent = MockRepository.GenerateMock<ISpState<MyMsgId>>();
            parent.Expect(o => o.IdChain).IgnoreArguments().Throw(new Exception("Exception from IdChain property"));

            TestHelpersNet.CatchExpected(50207, "SpStateBase`4", "InitStateIds", "Exception from IdChain property", () => {
                ISpState<MyMsgId> st = 
                    new StImpl<IDisposable, MyMsgId,TSID,TMsg>(
                        parent, MyDummyDI.MsgFactoryInstance, TSID.first, wo);
                st.OnEntry(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
                st.OnEntry(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Stop));
            });
        }



        #endregion



        //[Test]
        //public void _5020x_BuildName_ParentNullIdChain() {
        //    IDisposable wo = MockRepository.GenerateMock<IDisposable>();
        //    //ISpState parent = MockRepository.GenerateMock<ISpState>();
        //    //parent.Expect(o => o.IdChain).IgnoreArguments().Throw(new Exception("Exception from IdChain property"));

        //    ISpState st = MockRepository.GenerateMock<SpState<IDisposable>>(1, wo);
        //    st.Expect(o => o.

        //    TestHelpersNet.CatchExpected(50209, "SpState`1", "InitStateIds", "Unexpected Error Occured", () => {
        //    //    ISpState st = new StImpl<IDisposable>(1, wo);
        //        st.OnEntry(new SpBaseEventMsg(33, 22));
        //        st.OnEntry(new SpBaseEventMsg(33, 22));
        //    });
        //}





    }
}
