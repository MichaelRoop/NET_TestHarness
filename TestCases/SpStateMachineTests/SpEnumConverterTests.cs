using NUnit.Framework;
using SpStateMachine.Converters;
using TestCases.TestToolSet.Net;

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

        #region ToInt

        [Test]
        public void _0_ToInt_Extension_Good() {
            int val = 99;
            TestHelpersNet.CatchUnexpected(() => {
                val = TestEnum.Whoopee.ToInt();
            });
            Assert.AreEqual(2, val, "Did not return right number for TestEnum.Whoopee");
        }

        [Test]
        public void _0_ToInt_Direct_Good() {
            int val = 99;
            TestHelpersNet.CatchUnexpected(() => {
                val = SpEnumConverterExtensions.ToInt(TestEnum.Gnarly);
            });
            Assert.AreEqual(0, val, "Did not return right number for TestEnum.Gnarly");
        }


        [Test]
        public void _0_ToInt_ConverterClass_Good() {
            int val = 99;
            TestHelpersNet.CatchUnexpected(() => {
                val = SpConverter.EnumToInt(TestEnum.Gnarly);
            });
            Assert.AreEqual(0, val, "Did not return right number for TestEnum.Gnarly");
        }


        #endregion
        
        #region ToEnum

        [Test]
        public void _9999_ToEnum_DirectCall_UsingNonEnum() {
            TestHelpersNet.CatchExpected(9999, "SpEnumConverterExtensions", "ToEnum", "Enum Conversion Failed Attempting to Convert to Type 'Int32' with Value '10'", () => {
                SpEnumConverterExtensions.ToEnum<int>(10);
            });
        }
        
        [Test]
        public void _9999_ToEnum_Extension_UsingNonEnum() {
            TestHelpersNet.CatchExpected(9999, "SpEnumConverterExtensions", "ToEnum", "Enum Conversion Failed Attempting to Convert to Type 'Int32' with Value '10'", () => {
                int id = 10;
                id.ToEnum<int>();
            });
        }


        [Test]
        public void _9999_ToEnum_DirectCall_OutOfRange() {
            TestHelpersNet.CatchExpected(9999, "SpEnumConverterExtensions", "ToEnum", "Enum Conversion Out of Range Attempting to Convert to Type 'TestEnum' with Value '15'", () => {
                SpEnumConverterExtensions.ToEnum<TestEnum>(15);
            });
        }


        [Test]
        public void _9999_ToEnum_Extension_OutOfRange() {
            TestHelpersNet.CatchExpected(9999, "SpEnumConverterExtensions", "ToEnum", "Enum Conversion Out of Range Attempting to Convert to Type 'TestEnum' with Value '35'", () => {
                int id = 35;
                id.ToEnum<TestEnum>();
            });
        }


        [Test]
        public void _0_ToEnum_Extension_Good() {
            TestHelpersNet.CatchUnexpected(() => {
                int id = 2;
                id.ToEnum<TestEnum>();
            });
        }

        #endregion

        //#region SpIntToEnum class wrapper

        //[Test]
        //public void _0_SpIntToEnum_FromConstructionParam() {
        //    Assert.AreEqual(GrouchTestEnum.Groucho, new SpIntToEnum<GrouchTestEnum>(2).ToEnum());
        //}

        //[Test]
        //public void _9999_SpIntToEnum_OutOfRange() {
        //    TestHelpersNet.CatchExpected(9999, "SpEnumConverterExtensions", "ToEnum", "Enum Conversion Out of Range Attempting to Convert to Type 'GrouchTestEnum' with Value '39'", () => {
        //        new SpIntToEnum<GrouchTestEnum>(39).ToEnum();
        //    });
        //}

        //#endregion


    }


}
