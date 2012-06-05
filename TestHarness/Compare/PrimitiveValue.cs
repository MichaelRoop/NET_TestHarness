using System;
using System.Globalization;

namespace Ca.Roop.TestHarness.Compare {

/**
 * Generic class to retrieve object values if they wrap a primitive type.
 * <p>
 * The Primitive type and value classes are used in generic programming when you want to pass around objects
 * representing types.  Seems you cannot use primitive types as C# generic argument types.  C# will
 * automatically wrap any primitives passed in with the corresponding objects (Int16,Int32 classes, etc..).
 * <p>
 * @author Michael
 */
public class PrimitiveValue {

    private static NumberFormatInfo currentFormat = NumberFormatInfo.CurrentInfo;


    public static byte GetByte<T>(T obj) {
        if (PrimitiveTypeCheck.IsByte(obj)) {
            return (obj as IConvertible).ToByte(currentFormat);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    public static sbyte GetSByte<T>(T obj) {
        if (PrimitiveTypeCheck.IsSByte(obj)) {
            return (obj as IConvertible).ToSByte(currentFormat);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    public static short GetShort<T>(T obj) {
        if (PrimitiveTypeCheck.IsShort(obj)) {
            return (obj as IConvertible).ToInt16(currentFormat);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    public static ushort GetUShort<T>(T obj) {
        if (PrimitiveTypeCheck.IsUShort(obj)) {
            return (obj as IConvertible).ToUInt16(currentFormat);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    public static int GetInt<T>(T obj) {
        if (PrimitiveTypeCheck.IsInt(obj)) {
            return (obj as IConvertible).ToInt32(currentFormat);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    public static uint GetUInt<T>(T obj) {
        if (PrimitiveTypeCheck.IsUInt(obj)) {
            return (obj as IConvertible).ToUInt32(currentFormat);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    public static long GetLong<T>(T obj) {
        if (PrimitiveTypeCheck.IsLong(obj)) {
            return (obj as IConvertible).ToInt64(currentFormat);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    public static ulong GetULong<T>(T obj) {
        if (PrimitiveTypeCheck.IsULong(obj)) {
            return (obj as IConvertible).ToUInt64(currentFormat);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    /// <summary>Cast up to the highest numeric type for comparisons.</summary>
    /// <typeparam name="T">Generic type to cast up.</typeparam>
    /// <param name="obj">Type to cast.</param>
    /// <returns>The signed numeric type cast up to a long.</returns>
    /// <exception cref="ArgumentException" />
    public static long GetSignedNumber<T>(T obj) {
        if (PrimitiveTypeCheck.IsSignedNumber(obj)) {
            if (PrimitiveTypeCheck.IsSByte(obj)) {
                return GetSByte(obj);
            }
            else if(PrimitiveTypeCheck.IsShort(obj)) {
                return GetShort(obj);
            }
            else if(PrimitiveTypeCheck.IsInt(obj)) {
                return GetInt(obj);
            }
            else if(PrimitiveTypeCheck.IsLong(obj))  {
                return GetLong(obj);
            }
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for signed number");
    }


    /// <summary>Cast up to the highest unsigned numeric type for comparisons.</summary>
    /// <typeparam name="T">Generic type to cast up.</typeparam>
    /// <param name="obj">Type to cast.</param>
    /// <returns>The signed numeric type cast up to a ulong.</returns>
    /// <exception cref="ArgumentException" />
    public static ulong GetUnsignedNumber<T>(T obj) {
        if (PrimitiveTypeCheck.IsUnsignedNumber(obj)) {
            if (PrimitiveTypeCheck.IsByte(obj)) {
                return GetByte(obj);
            }
            else if (PrimitiveTypeCheck.IsUShort(obj)) {
                return GetUShort(obj);
            }
            else if (PrimitiveTypeCheck.IsUInt(obj)) {
                return GetUInt(obj);
            }
            else if (PrimitiveTypeCheck.IsULong(obj)) {
                return GetULong(obj);
            }
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for unsigned number");
    }


    public static float GetFloat<T>(T obj) {
        if (PrimitiveTypeCheck.IsFloat(obj)) {
            return (obj as IConvertible).ToSingle(currentFormat);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    public static double GetDouble<T>(T obj) {
        if (PrimitiveTypeCheck.IsDouble(obj)) {
            return (obj as IConvertible).ToDouble(currentFormat);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    public static double GetRealNumber<T>(T obj) {
        if (PrimitiveTypeCheck.IsFloat(obj)) {
            return GetFloat(obj);
        }
        else if(PrimitiveTypeCheck.IsDouble(obj))  {
            return GetDouble(obj);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    public static bool GetBoolean<T>(T obj) {
        if (PrimitiveTypeCheck.IsBoolean(obj)) {
            return (obj as IConvertible).ToBoolean(currentFormat);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    public static char GetChar<T>(T obj) {
        if (PrimitiveTypeCheck.IsChar(obj)) {
            return (obj as IConvertible).ToChar(currentFormat);
        }
        throw new ArgumentException("Invalid type:" + obj.GetType().Name + " for conversion");
    }


    //public static Enum GetEnum<T>(T obj) {
    //    if (PrimitiveTypeCheck.IsEnum(obj)) {
    //        return (obj as IConvertible).to


}



}
