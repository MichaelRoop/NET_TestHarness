using NUnit.Framework;
using Rhino.Mocks;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using System;
using TestCases.SpStateMachineTests.TestImplementations;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using TestCases.TestToolSet.Net;

namespace TestCases.SpStateMachineTests {

    #region Internal Test Classes

    public class smDerivedManagedFail : SpMachine<IDisposable,MyMsgId> {
        public smDerivedManagedFail(IDisposable wo, ISpState<MyMsgId> state)
            : base(wo, state) {
        }
        protected override void  DisposeManagedResources() {
            throw new Exception("Managed Dispose Exception");
        }
    }

    public class smDerivedNativeFail : SpMachine<IDisposable, MyMsgId> {
        public smDerivedNativeFail(IDisposable wo, ISpState<MyMsgId> state)
            : base(wo, state) {
        }
        protected override void DisposeNativeResources() {
            throw new Exception("Native Dispose Exception");
        }
    }

    #endregion

    [TestFixture]
    public class SpStateMachineTests {

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

        #region Dispose

        [Test]
        public void _0_Dispose_Multi() {
            ISpState<MyMsgId> st = MockRepository.GenerateMock<ISpState<MyMsgId>>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchUnexpected(() => {
                ISpStateMachine sm = new SpMachine<IDisposable, MyMsgId>(wo, st);
                sm.Dispose();
                sm.Dispose();
                sm.Dispose();
            });
        }

        [Test]
        public void _50173_Dispose_ChildManagedDisposeFail() {
            ISpState<MyMsgId> st = MockRepository.GenerateMock<ISpState<MyMsgId>>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50173, "SpMachine`2", "Dispose", "Managed Dispose Exception", () => {
                ISpStateMachine sm = new smDerivedManagedFail(wo, st);
                sm.Dispose();
            });
        }

        [Test]
        public void _50174_Dispose_ChildManagedDisposeFail() {
            ISpState<MyMsgId> st = MockRepository.GenerateMock<ISpState<MyMsgId>>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50174, "SpMachine`2", "Dispose", "Native Dispose Exception", () => {
                ISpStateMachine sm = new smDerivedNativeFail(wo, st);
                sm.Dispose();
            });
        }
        
        [Test]
        public void _50175_Dispose_ManagedResources_Error() {
            ISpState<MyMsgId> st = MockRepository.GenerateMock<ISpState<MyMsgId>>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            wo.Expect((o) => o.Dispose()).Throw(new Exception("Wrapped Object Threw Exception on Dispose"));

            TestHelpersNet.CatchExpected(50175, "SpMachine`2", "DisposeManagedResources", "Wrapped Object Threw Exception on Dispose", () => {
                ISpStateMachine sm = new SpMachine<IDisposable,MyMsgId>(wo, st);
                sm.Dispose();
            });
        }

        #endregion

        #region Constructor

        [Test]
        public void _50170_Ctor_WrappedObject() {
            ISpState<MyMsgId> st = MockRepository.GenerateMock<ISpState<MyMsgId>>();
            TestHelpersNet.CatchExpected(50170, "SpMachine`2", ".ctor", "Null wrappedObject Argument", () => {
                ISpStateMachine sm = new SpMachine<IDisposable,MyMsgId>(null, st);
                sm.Dispose();
            });
        }

        [Test]
        public void _50171_Ctor_NullState() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50171, "SpMachine`2", ".ctor", "Null state Argument", () => {
                ISpStateMachine sm = new SpMachine<IDisposable,MyMsgId>(wo, null);
                sm.Dispose();
            });
        }

        #endregion

        #region Tick

        [Test]
        public void _0_Tick_Ok() {
            ISpState<MyMsgId> st = MockRepository.GenerateMock<ISpState<MyMsgId>>();
            st.Expect((o) => o.FullName).Return("Main.FirstState.Init");
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            st.Expect((o) => o.OnEntry(null)).IgnoreArguments().Return(
                new SpStateTransition<MyMsgId>(
                    SpStateTransitionType.SameState, null, new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Start)));

            TestHelpersNet.CatchUnexpected(() => {
                ISpStateMachine sm = new SpMachine<IDisposable,MyMsgId>(wo, st);
                sm.Tick(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Tick));
            });
        }


        [Test]
        public void _50177_Tick_StateNullTransition() {
            ISpState<MyMsgId> st = MockRepository.GenerateMock<ISpState<MyMsgId>>();
            st.Expect((o) => o.FullName).Return("Main.FirstState.Init");
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            st.Expect((o) => o.OnEntry(null)).IgnoreArguments().Return(null);
            TestHelpersNet.CatchExpected(50177, "SpMachine`2", "Tick", "The State 'Main.FirstState.Init' OnEntry Returned a Null Transition", () => {
                ISpStateMachine sm = new SpMachine<IDisposable,MyMsgId>(wo, st);
                sm.Tick(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Tick));
            });
        }


        [Test]
        public void _50172_Tick_NullMsg() {
            ISpState<MyMsgId> st = MockRepository.GenerateMock<ISpState<MyMsgId>>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50172, "SpMachine`2", "Tick", "Null msg Argument", () => {
                ISpStateMachine sm = new SpMachine<IDisposable,MyMsgId>(wo, st);
                sm.Tick(null);
            });
        }


        [Test]
        public void _50176_Tick_Disposed() {
            ISpState<MyMsgId> st = MockRepository.GenerateMock<ISpState<MyMsgId>>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50176, "SpMachine`2", "Tick", "Attempting to use Disposed Object", () => {
                ISpStateMachine sm = new SpMachine<IDisposable,MyMsgId>(wo, st);
                sm.Dispose();
                sm.Tick(new MyBaseMsg(MyMsgType.SimpleMsg, MyMsgId.Tick));
            });
        }

        #endregion

    }

}
