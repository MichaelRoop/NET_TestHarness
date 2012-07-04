using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestCases.TestToolSet;
using SpStateMachine.Utils;
using SpStateMachine.Converters;

namespace TestCases.SpStateMachineTests {

    enum TestEnum {
        Gnarly,
        Farly,
        Whoopee,
        Splat,
    }

    enum GrouchTestEnum {
        Larry,
        Curly,
        Groucho,
        Max,
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

        #region ToInt

        [Test]
        public void _0_ToInt_Extension_Good() {
            int val = 99;
            TestHelpers.CatchUnexpected(() => {
                val = TestEnum.Whoopee.ToInt();
            });
            Assert.AreEqual(2, val, "Did not return right number for TestEnum.Whoopee");
        }

        [Test]
        public void _0_ToInt_Direct_Good() {
            int val = 99;
            TestHelpers.CatchUnexpected(() => {
                val = SpEnumConverterExtensions.ToInt(TestEnum.Gnarly);
            });
            Assert.AreEqual(0, val, "Did not return right number for TestEnum.Gnarly");
        }

        #endregion
        
        #region ToEnum

        [Test]
        public void _9999_ToEnum_DirectCall_UsingNonEnum() {
            TestHelpers.CatchExpected(9999, "SpEnumConverterExtensions", "ToEnum", "Enum Conversion Failed Attempting to Convert to Type 'Int32' with Value '10'", () => {
                SpEnumConverterExtensions.ToEnum<int>(10);
            });
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

    [TestFixture]
    public class SpToEnumTests {

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
        public void _0_ToAnyEnum_Static_MixedTypes() {
            Assert.AreEqual(GrouchTestEnum.Groucho, SpToEnum<GrouchTestEnum>.ToAnyEnum(2));
            Assert.AreEqual(TestEnum.Gnarly, SpToEnum<TestEnum>.ToAnyEnum(0));
        }


        [Test]
        public void _0_ToEnum_FromConstructionParam() {
            Assert.AreEqual(GrouchTestEnum.Groucho, new SpToEnum<GrouchTestEnum>(2).ToEnum());
        }

        [Test]
        public void _0_ToEnum_FromMethodParam() {
            Assert.AreEqual(GrouchTestEnum.Larry, new SpToEnum<GrouchTestEnum>(1).ToEnum(0));
        }


    }



}
