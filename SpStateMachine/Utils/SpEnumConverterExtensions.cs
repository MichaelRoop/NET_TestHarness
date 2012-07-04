using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;

namespace SpStateMachine.Utils {

    public static class SpEnumConverterExtensions {

        public static int ToInt(this Enum value) {
            // TODO - put in checking
            return Convert.ToInt32(value);
        }


        // Force non nullable
        public static T ToEnum<T>(this int value) where T : struct {
            T enumType = default(T);
            T ret = 
            WrapErr.ToErrorReportException(9999,
                () => { return String.Format(
                    "Enum Conversion Failed Attempting to Convert to Type '{0}' with Value '{1}'", 
                    enumType.GetType().Name, value); }, 
                () => {
                    // This will throw on non enum but not out of range
                    ret = (T)Enum.Parse(typeof(T), value.ToString());
                    Console.WriteLine(ret.ToString());

                    //WrapErr.ChkTrue(Enum.IsDefined(typeof(T), ret), 9999, () => {
                    //    return String.Format(
                    //        "Enum Conversion Out of Range Attempting to Convert to Type '{0}' with Value '{1}'", 
                    //        enumType.GetType().Name, value);
                    //});


                    return ret;
                    //id = (T)Enum.Parse(typeof(T), value.ToString());

                    ////WrapErr.ToErrReport(9999, () => { return String.Format(""); }, () => {
                    ////    id = (MyEventType)Enum.Parse(typeof(MyEventType), value.ToString());
                    ////});
                    //return id;
                });

            WrapErr.ChkTrue(Enum.IsDefined(typeof(T), ret), 9999, () => {
                return String.Format(
                    "Enum Conversion Out of Range Attempting to Convert to Type '{0}' with Value '{1}'",
                    enumType.GetType().Name, value);
            });
            return ret;


        }
    }
}
