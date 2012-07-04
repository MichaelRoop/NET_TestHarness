using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestCases.TestToolSet;
using SpStateMachine.Utils;

namespace TestCases.SpStateMachineTests.TestImplementations {

    enum TestEnum {
        Gnarly,
        Farly,
        Whoopee,
        Splat,
    }


    [TestFixture]
    public class SpEnumConverterTests {

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

        #region ToEnum

        [Test]
        public void _9999_ToEnum_DirectCall_UsingNonEnum() {
            TestHelpers.CatchExpected(9999, "SpEnumConverterExtensions", "ToEnum", "Enum Conversion Failed Attempting to Convert to Type 'Int32' with Value '10'", () => {
                SpEnumConverterExtensions.ToEnum<int>(10);
            });

            //Error Converting int '10' to Enum Type 'Int32'
        }
        
        [Test]
        public void _9999_ToEnum_Extension_UsingNonEnum() {
            TestHelpers.CatchExpected(9999, "SpEnumConverterExtensions", "ToEnum", "Enum Conversion Failed Attempting to Convert to Type 'Int32' with Value '10'", () => {
                int id = 10;
                id.ToEnum<int>();
            });
        }


        [Test]
        public void _9999_ToEnum_DirectCall_OutOfRange() {
            TestHelpers.CatchExpected(9999, "SpEnumConverterExtensions", "ToEnum", "Enum Conversion Out of Range Attempting to Convert to Type 'TestEnum' with Value '15'", () => {
                SpEnumConverterExtensions.ToEnum<TestEnum>(15);
            });
        }


        [Test]
        public void _9999_ToEnum_Extension_OutOfRange() {
            TestHelpers.CatchExpected(9999, "SpEnumConverterExtensions", "ToEnum", "Enum Conversion Out of Range Attempting to Convert to Type 'TestEnum' with Value '35'", () => {
                int id = 35;
                id.ToEnum<TestEnum>();
            });
        }


        [Test]
        public void _0_ToEnum_Extension_Good() {
            TestHelpers.CatchUnexpected(() => {
                int id = 2;
                id.ToEnum<TestEnum>();
            });
        }




//            return WrapErr.ToErrorReportException(9999, "Error Converting from Int to Enum", () => {


        #endregion



    }
}
