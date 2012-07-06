using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestCases.TestToolSet;
using SpStateMachine.States;
using SpStateMachine.Interfaces;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using TestCases.SpStateMachineTests.TestImplementations;
using SpStateMachine.Core;
using Rhino.Mocks;
using SpStateMachine.Messages;
using SpStateMachine.Converters;

namespace TestCases.SpStateMachineTests {

    #region Test class implementations
    
    //internal interface ITstObj : IDisposable {
    //}


    //internal class TstObj {
    //}

    public class StImpl<T> : SpState<T> where T : class, IDisposable {

        public StImpl(ISpState parent, ISpMsgFactory msgFactory, ISpIdConverter idConverter, int id, T wo)
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

        #region Constructors

        [Test]
        public void _50200_Constructor_NullWrappedObject() {
            TestHelpers.CatchExpected(50200, "SpStateBase`1", ".ctor", "Null wrappedObject Argument", () => {
                ISpState st = new StImpl<IDisposable>(MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, null);
            });
        }

        #endregion

        #region OnEntry

        [Test]
        public void _0_OnEntry_ExecutedTwiceWithOnExitBetween() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpers.CatchUnexpected(() => {
                ISpState st = new StImpl<IDisposable>(MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, wo);
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
                st.OnExit();
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
            });
        }


        [Test]
        public void _50201_OnEntry_ExecutedTwice() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpers.CatchExpected(50201, "SpStateBase`1", "OnEntry", "OnEntry Cannot be Executed More Than Once Until OnExit is Called", () => {
                ISpState st = new StImpl<IDisposable>(MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, wo);
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
            });
        }

        #endregion

        #region OnTick

        [Test]
        public void _0_OnTick_AfterOnEntry() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpers.CatchUnexpected(() => {
                ISpState st = new StImpl<IDisposable>(MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, wo);
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
                st.OnTick(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
            });
        }

        [Test]
        public void _50205_OnTick_WithoutOnEntry() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpers.CatchExpected(50205, "SpStateBase`1", "OnTick", "OnTick for '1' State Cannot be Executed Before OnEntry", () => {
                ISpState st = new StImpl<IDisposable>(MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, wo);
                st.OnTick(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
            });
        }

        #endregion

        #region InitStateIds

        class tstA {
        }

        class tstState : SpState<tstA> {
            public tstState(ISpState parent, tstA o)
                : base(parent, MyMsgFactory.Instance, SpIntToIntConverter.Instance, new SpIntToInt(3), o) {
            }
        }

        class tstSuperState : SpSuperState<tstA> {
            public tstSuperState(tstA o)
                : base(MyMsgFactory.Instance, SpIntToIntConverter.Instance, new SpIntToInt(1), o) {
            }
        }

        [Test]
        public void _0_() {
            tstA o = new tstA();

            ISpState ss = new tstSuperState(o);
            ISpState s = new tstState(ss, o);

            Console.WriteLine("SS name:{0} - FullName:{1}", ss.Name, ss.FullName);
            Console.WriteLine("S name:{0} - FullName:{1}", s.Name, s.FullName);


        }



        [Test]
        public void _50206_InitStateIds_ParentNullIdChain() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            ISpState parent = MockRepository.GenerateMock<ISpState>();
            parent.Expect(o => o.IdChain).IgnoreArguments().Return(null);

            TestHelpers.CatchExpected(50206, "SpStateBase`1", "InitStateIds", "The Parent has a Null Id Chain", () => {
                ISpState st = new StImpl<IDisposable>(parent, MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, wo);
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
                st.OnEntry(new SpBaseEventMsg(new SpIntToInt(33), new SpIntToInt(22)));
            });
        }


        [Test]
        public void _50207_InitStateIds_UnexpectedError() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            ISpState parent = MockRepository.GenerateMock<ISpState>();
            parent.Expect(o => o.IdChain).IgnoreArguments().Throw(new Exception("Exception from IdChain property"));

            TestHelpers.CatchExpected(50207, "SpStateBase`1", "InitStateIds", "Unexpected Error Occured", () => {
                ISpState st = new StImpl<IDisposable>(parent, MyMsgFactory.Instance, SpIntToIntConverter.Instance, 1, wo);
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

        //    TestHelpers.CatchExpected(50209, "SpState`1", "InitStateIds", "Unexpected Error Occured", () => {
        //    //    ISpState st = new StImpl<IDisposable>(1, wo);
        //        st.OnEntry(new SpBaseEventMsg(33, 22));
        //        st.OnEntry(new SpBaseEventMsg(33, 22));
        //    });
        //}





    }
}
