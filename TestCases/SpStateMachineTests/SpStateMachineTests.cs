﻿using NUnit.Framework;
using Rhino.Mocks;
using SpStateMachine.Converters;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using SpStateMachine.Messages;
using System;
using TestCases.TestToolSet.Net;

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
            ISpState st = MockRepository.GenerateMock<ISpState>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchUnexpected(() => {
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
            TestHelpersNet.CatchExpected(50173, "SpMachine`1", "Dispose", "Managed Dispose Exception", () => {
                ISpStateMachine sm = new smDerivedManagedFail(wo, st);
                sm.Dispose();
            });
        }

        [Test]
        public void _50174_Dispose_ChildManagedDisposeFail() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50174, "SpMachine`1", "Dispose", "Native Dispose Exception", () => {
                ISpStateMachine sm = new smDerivedNativeFail(wo, st);
                sm.Dispose();
            });
        }
        
        [Test]
        public void _50175_Dispose_ManagedResources_Error() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            wo.Expect((o) => o.Dispose()).Throw(new Exception("Wrapped Object Threw Exception on Dispose"));

            TestHelpersNet.CatchExpected(50175, "SpMachine`1", "DisposeManagedResources", "Wrapped Object Threw Exception on Dispose", () => {
                ISpStateMachine sm = new SpMachine<IDisposable>(wo, st);
                sm.Dispose();
            });
        }

        #endregion

        #region Constructor

        [Test]
        public void _50170_Ctor_WrappedObject() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            TestHelpersNet.CatchExpected(50170, "SpMachine`1", ".ctor", "Null wrappedObject Argument", () => {
                ISpStateMachine sm = new SpMachine<IDisposable>(null, st);
                sm.Dispose();
            });
        }

        [Test]
        public void _50171_Ctor_NullState() {
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50171, "SpMachine`1", ".ctor", "Null state Argument", () => {
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
            st.Expect((o) => o.OnEntry(null)).IgnoreArguments().Return(
                new SpStateTransition(SpStateTransitionType.SameState, null, new SpBaseEventMsg(new SpIntToInt(3), new SpIntToInt(3))));
            TestHelpersNet.CatchUnexpected(() => {
                ISpStateMachine sm = new SpMachine<IDisposable>(wo, st);
                sm.Tick(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(1)));
            });
        }



        [Test]
        public void _50177_Tick_StateNullTransition() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            st.Expect((o) => o.FullName).Return("Main.FirstState.Init");
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            st.Expect((o) => o.OnEntry(null)).IgnoreArguments().Return(null);
            TestHelpersNet.CatchExpected(50177, "SpMachine`1", "Tick", "The State 'Main.FirstState.Init' OnEntry Returned a Null Transition", () => {
                ISpStateMachine sm = new SpMachine<IDisposable>(wo, st);
                sm.Tick(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(1)));
            });
        }


        [Test]
        public void _50172_Tick_NullMsg() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50172, "SpMachine`1", "Tick", "Null msg Argument", () => {
                ISpStateMachine sm = new SpMachine<IDisposable>(wo, st);
                sm.Tick(null);
            });
        }


        [Test]
        public void _50176_Tick_Disposed() {
            ISpState st = MockRepository.GenerateMock<ISpState>();
            IDisposable wo = MockRepository.GenerateMock<IDisposable>();
            TestHelpersNet.CatchExpected(50176, "SpMachine`1", "Tick", "Attempting to use Disposed Object", () => {
                ISpStateMachine sm = new SpMachine<IDisposable>(wo, st);
                sm.Dispose();
                sm.Tick(new SpBaseEventMsg(new SpIntToInt(1), new SpIntToInt(1)));
            });
        }


        #endregion

    }


}
