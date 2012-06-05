using System;

namespace Ca.Roop.TestHarness.Compare {


/// <summary>
/// Generic class to check objects to see if they are wrappers for primitive types. 
/// </summary>
/// <remarks>
/// The Primitive type and value classes are used in generic programming when you want to pass 
/// around objects representing types.  Seems you cannot use primitive types in generics.  They 
/// will be automatically wrapped and passed in with the corresponding objects (Short,Integer 
/// classes, etc..).
/// <p>
/// However, these are not flagged by the isPrimitive() method.  This will only work with the 
/// actual primitive types (int, short, boolean, etc) which are, as is turns out, objects 
/// anyways. These classes allow you to easily identify the types as representing primitives.
/// </remarks>
public class  PrimitiveTypeCheck {

    public static bool IsByte<T>(T type) {
		return type is Byte;
    }

    public static bool IsSByte<T>(T type) {
        return type is SByte;
    }

    public static bool IsShort<T>(T type) {
		return type is Int16;
	}

    public static bool IsUShort<T>(T type) {
        return type is UInt16;
    }

    public static bool IsInt<T>(T type) {
		return type is Int32;
	}

    public static bool IsUInt<T>(T type) {
        return type is UInt32;
    }

    public static bool IsLong<T>(T type) {
		return type is Int64;
	}

    public static bool IsULong<T>(T type) {
        return type is UInt64;
    }


    public static bool IsSignedNumber<T>(T type) {
        return IsSByte(type) || IsShort(type) || IsInt(type) || IsLong(type);
	}


    public static bool IsUnsignedNumber<T>(T type) {
        return IsByte(type) || IsUShort(type) || IsUInt(type) || IsULong(type);
    }

    public static bool IsFloat<T>(T type) {
		return  type is Single;
	}


	public static bool IsDouble<T>(T type) {
		return  type is Double;
	}


	public static bool IsReal<T>(T type) {
		return IsFloat(type) || IsDouble(type);
	}


    public static bool IsDecimal<T>(T type) {
        return type is Decimal;
    }


	public static bool IsBoolean<T>(T a) {
		return  a is Boolean;
	}


	public static bool IsChar<T>(T a) {
		return  a is Char;
	}


    public static bool IsEnum<T>(T a) {
        return a is Enum;
    }

}

    

}
