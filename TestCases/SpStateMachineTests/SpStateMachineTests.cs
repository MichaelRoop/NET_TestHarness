using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SpStateMachine.Interfaces;
using SpStateMachine.Core;
using LogUtils;
using ChkUtils;
using ChkUtils.ErrObjects;
using TestCases.TestToolSet;

namespace TestCases.SpStateMachineTests {

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
        

        [Test]
        public void DoIt() {
            ISpEventListner listner = MockRepository.GenerateMock<ISpEventListner>();
            listner.Expect((o) => o.Dispose()).Throw(new Exception("Listner exception"));

            ISpEventStore st = MockRepository.GenerateMock<ISpEventStore>();
            ISpBehaviorOnEvent be = MockRepository.GenerateMock<ISpBehaviorOnEvent>();
            ISpStateMachine sm = MockRepository.GenerateMock<ISpStateMachine>();
            ISpPeriodicTimer tm = MockRepository.GenerateMock<ISpPeriodicTimer>();

            TestHelpers.CatchUnexpected(() => {
                SpStateMachineEngine engine = new SpStateMachineEngine(listner, st, be, sm, tm);

                Log.Debug("SpStateMachineTests", "DoIt", "Just before Dispose");
                engine.Dispose();
                Log.Debug("SpStateMachineTests", "DoIt", "Just after Dispose");
            });

            this.logReader.Validate(50050, "SpStateMachineEngine", "DisposeObject", "Error Disposing Object:msgListner");

        }




        //private void ValidateFromLog(int code) {
        //    ErrReport err = this.errors.Find((item) => item.Code == code);
        //    Assert.IsNotNull(err, String.Format("There was no error logged for code:{0}", code));


        //}


    }
}
