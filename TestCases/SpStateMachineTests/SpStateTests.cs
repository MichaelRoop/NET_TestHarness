using NUnit.Framework;
using Rhino.Mocks;
using SpStateMachine.Converters;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using SpStateMachine.States;
using System;
using TestCases.SpStateMachineTests.TestImplementations;
using TestCases.TestToolSet.Net;

namespace TestCases.SpStateMachineTests {

    #region Test class implementations

    //internal interface ITstObj : IDisposable {
    //}


    //internal class TstObj {
    //}

    public class StImpl<T,T2> : SpState<T,T2> where T : class, IDisposable where T2 : struct {

        public StImpl(ISpState<T2> parent, ISpMsgFactory msgFactory, ISpIdConverter idConverter, int id, T wo)
            : base(parent, msgFactory, idConverter, new SpIntToInt(id), wo) {
        }

        public StImpl(ISpMsgFactory msgFactory, ISpIdConverter idConverter, int id, T wo)
            : this(null, msgFactory, idConverter, id, wo) {
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
            TestHelpersNet.CatchExpected(50200, "SpStateBase`2", ".ctor", "Null wrappedObject Argument", () => {
                ISpState<MyEventType> st = new StImpl<IDisposable,MyEventType>(MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, null);
            });
        }

        #endregion

        #region OnEntry

        [Test]
        public void _0_OnEntry_ExecutedTwiceWithOnExitBetween() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchUnexpected(() => {
                ISpState<MyEventType> st = 
                    new StImpl<IDisposable,MyEventType>(MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, wo);
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
                st.OnExit();
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
            });
        }


        [Test]
        public void _50201_OnEntry_ExecutedTwice() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50201, "SpStateBase`2", "OnEntry", "OnEntry Cannot be Executed More Than Once Until OnExit is Called", () => {
                ISpState<MyEventType> st = 
                    new StImpl<IDisposable,MyEventType>(MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, wo);
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
            });
        }

        #endregion

        #region OnTick

        [Test]
        public void _0_OnTick_AfterOnEntry() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchUnexpected(() => {
                ISpState<MyEventType> st = 
                    new StImpl<IDisposable, MyEventType>(MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, wo);
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
                st.OnTick(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
            });
        }

        [Test]
        public void _50205_OnTick_WithoutOnEntry() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50205, "SpStateBase`2", "OnTick", "OnTick for '1' State Cannot be Executed Before OnEntry", () => {
                ISpState<MyEventType> st = new StImpl<IDisposable, MyEventType>(MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, wo);
                st.OnTick(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
            });
        }

        #endregion

        #region InitStateIds

        class tstA {
        }

        class tstState : SpState<tstA,MyEventType> {
            public tstState(ISpState<MyEventType> parent, tstA o)
                : base(parent, MyMsgFactory.Instance, SpIntToIntConverter.Instance, new SpIntToInt(3), o) {
            }
        }

        class tstSuperState : SpSuperState<tstA,MyEventType> {
            public tstSuperState(tstA o)
                : base(MyMsgFactory.Instance, SpIntToIntConverter.Instance, new SpIntToInt(1), o) {
            }
        }

        [Test]
        public void _0_() {
            tstA o = new tstA();

            ISpState<MyEventType> ss = new tstSuperState(o);
            ISpState<MyEventType> s = new tstState(ss, o);

            Console.WriteLine("SS name:{0} - FullName:{1}", ss.Name, ss.FullName);
            Console.WriteLine("S name:{0} - FullName:{1}", s.Name, s.FullName);


        }



        [Test]
        public void _50206_InitStateIds_ParentNullIdChain() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            ISpState<MyEventType> parent = MockRepository.GenerateMock<ISpState<MyEventType>>();
            parent.Expect(o => o.IdChain).IgnoreArguments().Return(null);

            TestHelpersNet.CatchExpected(50206, "SpStateBase`2", "InitStateIds", "The Parent has a Null Id Chain", () => {
                ISpState<MyEventType> st = 
                    new StImpl<IDisposable, MyEventType>(parent, MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, wo);
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
            });
        }


        [Test]
        public void _50207_InitStateIds_UnexpectedError() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            ISpState<MyEventType> parent = MockRepository.GenerateMock<ISpState<MyEventType>>();
            parent.Expect(o => o.IdChain).IgnoreArguments().Throw(new Exception("Exception from IdChain property"));

            TestHelpersNet.CatchExpected(50207, "SpStateBase`2", "InitStateIds", "Exception from IdChain property", () => {
                ISpState<MyEventType> st = 
                    new StImpl<IDisposable, MyEventType>(parent, MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, wo);
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
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
