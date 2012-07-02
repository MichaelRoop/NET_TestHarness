using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestCases.TestToolSet;
using Rhino.Mocks;
using SpStateMachine.Interfaces;
using SpStateMachine.Core;
using SpStateMachine.Messages;

namespace TestCases.SpStateMachineTests {

    #region Internal Test Classes

    public class smDerivedManagedFail : SpMachine<IDisposable> {
        public smDerivedManagedFail(IDisposable wo, ISpState state)
            : base(wo, state) {
        }
        protected override void  DisposeManagedResources() {
            throw new Exception("Managed Dispose Exception");
        }
    }

    public class smDerivedNativeFail : SpMachine<IDisposable> {
        public smDerivedNativeFail(IDisposable wo, ISpState state)
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

        #region Dispose

        [Test]
        public void _0_Dispose_Multi() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpers.CatchUnexpected(() => {
                ISpStateMachine sm = new SpMachine<IDisposable>(wo, st);
                sm.Dispose();
                sm.Dispose();
                sm.Dispose();
            });
        }

        [Test]
        public void _50173_Dispose_ChildManagedDisposeFail() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpers.CatchExpected(50173, "SpMachine`1", "Dispose", "Unexpected Error Occured", () => {
                ISpStateMachine sm = new smDerivedManagedFail(wo, st);
                sm.Dispose();
            });
        }

        [Test]
        public void _50174_Dispose_ChildManagedDisposeFail() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpers.CatchExpected(50174, "SpMachine`1", "Dispose", "Unexpected Error Occured", () => {
                ISpStateMachine sm = new smDerivedNativeFail(wo, st);
                sm.Dispose();
            });
        }
        
        [Test]
        public void _50175_Dispose_ManagedResources_Error() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            wo.Expect((o) => o.Dispose()).Throw(new Exception("Wrapped Object Threw Exception on Dispose"));

            TestHelpers.CatchExpected(50175, "SpMachine`1", "DisposeManagedResources", "Unexpected Error Occured", () => {
                ISpStateMachine sm = new SpMachine<IDisposable>(wo, st);
                sm.Dispose();
            });
        }

        #endregion

        #region Constructor

        [Test]
        public void _50170_Ctor_WrappedObject() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            TestHelpers.CatchExpected(50170, "SpMachine`1", ".ctor", "Null wrappedObject Argument", () => {
                ISpStateMachine sm = new SpMachine<IDisposable>(null, st);
                sm.Dispose();
            });
        }

        [Test]
        public void _50171_Ctor_NullState() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpers.CatchExpected(50171, "SpMachine`1", ".ctor", "Null state Argument", () => {
                ISpStateMachine sm = new SpMachine<IDisposable>(wo, null);
                sm.Dispose();
            });
        }

        #endregion

        #region Tick

        [Test]
        public void _0_Tick_Ok() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            st.Expect((o) => o.FullName).Return("Main.FirstState.Init");
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            st.Expect((o) => o.OnEntry(null)).IgnoreArguments().Return(new SpStateTransition(SpStateTransitionType.SameState, null, new SpBaseMsg(3, 3)));
            TestHelpers.CatchUnexpected(() => {
                ISpStateMachine sm = new SpMachine<IDisposable>(wo, st);
                sm.Tick(new SpBaseMsg(1, 1));
            });
        }



        [Test]
        public void _50177_Tick_StateNullTransition() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            st.Expect((o) => o.FullName).Return("Main.FirstState.Init");
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            st.Expect((o) => o.OnEntry(null)).IgnoreArguments().Return(null);
            TestHelpers.CatchExpected(50177, "SpMachine`1", "Tick", "The State 'Main.FirstState.Init' OnEntry Returned a Null Transition", () => {
                ISpStateMachine sm = new SpMachine<IDisposable>(wo, st);
                sm.Tick(new SpBaseMsg(1, 1));
            });
        }


        [Test]
        public void _50172_Tick_NullMsg() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpers.CatchExpected(50172, "SpMachine`1", "Tick", "Null msg Argument", () => {
                ISpStateMachine sm = new SpMachine<IDisposable>(wo, st);
                sm.Tick(null);
            });
        }


        [Test]
        public void _50176_Tick_Disposed() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpers.CatchExpected(50176, "SpMachine`1", "Tick", "Attempting to use Disposed Object", () => {
                ISpStateMachine sm = new SpMachine<IDisposable>(wo, st);
                sm.Dispose();
                sm.Tick(new SpBaseMsg(1, 1));
            });
        }


        #endregion

    }


}
