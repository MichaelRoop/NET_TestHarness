using NUnit.Framework;
using Rhino.Mocks;
using SpStateMachine.Core;
using SpStateMachine.Interfaces;
using System;
using System.Threading;
using TestCases.TestToolSet.Net;

namespace TestCases.SpStateMachineTests {

    [TestFixture]
    public class SpStateMachineEngineTests {

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

        [Test]
        public void OperateActualTestStateMachine() {

        }



        #region Start

        [Test]
        public void _50056_StartDisposed() {
            ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
            listner.Expect((o) => o.Dispose()).Throw(new Exception("Listner exception"));

            ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
            ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
            ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
            ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

            TestHelpersNet.CatchExpected(50056, "SpStateMachineEngine", "Start", "Attempting to use Disposed Object", () => {
                SpStateMachineEngine engine = new SpStateMachineEngine(listner, st, be, sm, tm);
                Console.WriteLine("Test: Disposing");
                engine.Dispose();
                Thread.Sleep(500); // Nothing stopping the thead internaly with mocks
                engine.Start();

            });
        }

        #endregion

        #region Stop

        [Test]
        public void _50057_StopDisposed() {
            ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
            listner.Expect((o) => o.Dispose()).Throw(new Exception("Listner exception"));

            ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
            ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
            ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
            ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

            TestHelpersNet.CatchExpected(50057, "SpStateMachineEngine", "Stop", "Attempting to use Disposed Object", () => {
                SpStateMachineEngine engine = new SpStateMachineEngine(listner, st, be, sm, tm);
                Console.WriteLine("Test: Disposing");
                engine.Dispose();
                Thread.Sleep(500); // Nothing stopping the thead internaly with mocks
                engine.Stop();

            });
        }

        #endregion

        #region Dispose

        [Test]
        public void _0_Dispose_MultiDisposeSafe() {
            ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
            listner.Expect((o) => o.Dispose()).Throw(new Exception("Listner exception"));

            ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
            ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
            ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
            ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

            TestHelpersNet.CatchUnexpected(() => {
                SpStateMachineEngine engine = new SpStateMachineEngine(listner, st, be, sm, tm);
                Console.WriteLine("Test: Disposing");
                engine.Dispose();
                engine.Dispose();
                engine.Dispose();
                engine.Dispose();
                engine.Dispose();
                Thread.Sleep(500); // Nothing stopping the thead internaly with mocks
                Console.WriteLine("Test: Finished Disposing");
            });

            this.logReader.Validate(50060, "SpStateMachineEngine", "DisposeObject", "Error Disposing Object:msgListner");
        }

        #endregion

        #region DisposeObject

        [Test]
        public void _50060_DisposeObject_ErrorDisposingInternalObjects() {
            ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
            listner.Expect((o) => o.Dispose()).Throw(new Exception("Listner exception"));

            ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
            ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
            ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
            ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

            TestHelpersNet.CatchUnexpected(() => {
                SpStateMachineEngine engine = new SpStateMachineEngine(listner, st, be, sm, tm);
                Console.WriteLine("Test: Disposing");
                engine.Dispose();
                Thread.Sleep(500); // Nothing stopping the thead internaly with mocks
                Console.WriteLine("Test: Finished Disposing");
            });

            this.logReader.Validate(50060, "SpStateMachineEngine", "DisposeObject", "Error Disposing Object:msgListner");
        }


        [Test]
        public void _0_DisposeObject_Success() {
            ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
            ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
            ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
            ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
            ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

            TestHelpersNet.CatchUnexpected(() => {
                SpStateMachineEngine engine = new SpStateMachineEngine(listner, st, be, sm, tm);
                engine.Dispose();
            });
        }

        #endregion

        #region Constructor

        [Test]
        public void _50050_SpStateMachineEngine_nullListner() {
            ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
            ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
            ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
            ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
            ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

            TestHelpersNet.CatchExpected(50050, "SpStateMachineEngine", ".ctor", "Null msgListner Argument", () => {
                SpStateMachineEngine engine = new SpStateMachineEngine(null, st, be, sm, tm);
                engine.Dispose();
            });

            //this.logReader.Validate(50060, "SpStateMachineEngine", "DisposeObject", "Error Disposing Object:msgListner");
        }

        [Test]
        public void _50051_SpStateMachineEngine_nullStore() {
            ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
            ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
            ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
            ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
            ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

            TestHelpersNet.CatchExpected(50051, "SpStateMachineEngine", ".ctor", "Null msgStore Argument", () => {
                SpStateMachineEngine engine = new SpStateMachineEngine(listner, null, be, sm, tm);
                engine.Dispose();
            });
        }

        [Test]
        public void _50052_SpStateMachineEngine_nullBehavior() {
            ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
            ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
            ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
            ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
            ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

            TestHelpersNet.CatchExpected(50052, "SpStateMachineEngine", ".ctor", "Null eventBehavior Argument", () => {
                SpStateMachineEngine engine = new SpStateMachineEngine(listner, st, null, sm, tm);
                engine.Dispose();
            });
        }

        [Test]
        public void _50053_SpStateMachineEngine_nullStateMachine() {
            ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
            ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
            ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
            ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
            ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

            TestHelpersNet.CatchExpected(50053, "SpStateMachineEngine", ".ctor", "Null stateMachine Argument", () => {
                SpStateMachineEngine engine = new SpStateMachineEngine(listner, st, be, null, tm);
                engine.Dispose();
            });
        }

        [Test]
        public void _50054_SpStateMachineEngine_nullTimer() {
            ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
            ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
            ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
            ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
            ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

            TestHelpersNet.CatchExpected(50054, "SpStateMachineEngine", ".ctor", "Null timer Argument", () => {
                SpStateMachineEngine engine = new SpStateMachineEngine(listner, st, be, sm, null);
                engine.Dispose();
            });
        }

        #endregion


        #region DriverThreade

        [Test]
        public void _50058_DriverThreadUnexpectedError() {
            ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
            listner.Expect((o) => o.Dispose()).Throw(new Exception("Listner exception"));

            ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
            ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
            be.Expect((o) => o.WaitOnEvent()).Throw(new Exception("Behavior WaitOn Exception"));

            ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
            ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

            TestHelpersNet.CatchUnexpected(() => {
                SpStateMachineEngine engine = new SpStateMachineEngine(listner, st, be, sm, tm);
                Console.WriteLine("Test: Disposing");
                engine.Dispose();
                Thread.Sleep(500); // Nothing stopping the thead internaly with mocks
            });

            this.logReader.Validate(50058, "SpStateMachineEngine", "DriverThread", "Behavior WaitOn Exception");


        }

        #endregion





        //[Test]
        //public void _50054_SpStateMachineEngine_nullListner() {
        //    //ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
        //    ISpEventListner listner = null;

        //    ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
        //    ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
        //    ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
        //    ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

        //    TestHelpersNet.CatchExpected(50050, "SpStateMachineEngine", ".ctor", "Null msgListner Argument", () => {
        //        SpStateMachineEngine engine = new SpStateMachineEngine(listner, st, be, sm, tm);
        //        engine.Dispose();
        //    });
        //}
    }
}
