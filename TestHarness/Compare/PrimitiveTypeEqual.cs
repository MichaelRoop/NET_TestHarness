
namespace Ca.Roop.TestHarness.Compare {

/// <summary>
/// Generic class to check object equality on value if they wrap a primitive type.
/// </summary>
/// <remarks>
/// The Primitive type and value classes are used in generic programming when you want to pass 
/// around objects representing types.  You cannot use primitive types as Java generic argument 
/// types.  Seems C# will automatically wrap any primitives passed in with the corresponding 
/// objects (Int16,Int32 classes, etc..).
/// </remarks>
public class PrimitiveTypeEqual {


    /// <summary>Check floating numbers for equality.</summary>
    /// <remarks>
    /// It will check between double and float and also against non real numbers.
    /// </remarks>
    /// <typeparam name="T1">First generic type in the equality check.</typeparam>
    /// <typeparam name="T2">Second generic type in the equality check.</typeparam>
    /// <param name="a">First type.</param>
    /// <param name="b">Second type.</param>
    /// <returns>true if equal, otherwise false.</returns>
	public static bool IsFloatingEqual<T1,T2>(T1 a, T2 b) {
		if (PrimitiveTypeCheck.IsReal(a) && PrimitiveTypeCheck.IsReal(b)) {
			return PrimitiveValue.GetRealNumber(a) == PrimitiveValue.GetRealNumber(b);
		}
		else if (PrimitiveTypeCheck.IsSignedNumber(a) || PrimitiveTypeCheck.IsSignedNumber(b)) {
			long num = PrimitiveTypeCheck.IsSignedNumber(a)
                ? PrimitiveValue.GetSignedNumber(a)
                : PrimitiveValue.GetSignedNumber(b);
			double fNum	= PrimitiveTypeCheck.IsReal(a)
                ? PrimitiveValue.GetRealNumber(a)
                : PrimitiveValue.GetRealNumber(b);
			return num == fNum;
		}
		else {
			return false;
		}
	}


    public static bool IsSignedNumberEqual<T1, T2>(T1 a, T2 b) {
        if (PrimitiveTypeCheck.IsSignedNumber(a) && PrimitiveTypeCheck.IsSignedNumber(b)) {
            return PrimitiveValue.GetSignedNumber(a) == PrimitiveValue.GetSignedNumber(b);
        }
        return false;
    }


    public static bool IsUnsignedNumberEqual<T1, T2>(T1 a, T2 b) {
        if (PrimitiveTypeCheck.IsUnsignedNumber(a) && PrimitiveTypeCheck.IsUnsignedNumber(b)) {
            return PrimitiveValue.GetUnsignedNumber(a) == PrimitiveValue.GetUnsignedNumber(b);
        }
        return false;
    }





	public static bool IsBooleanEqual<T1,T2>( T1 a, T2 b ) {
		if (PrimitiveTypeCheck.IsBoolean(a) && PrimitiveTypeCheck.IsBoolean(b)) {
			return PrimitiveValue.GetBoolean(a) == PrimitiveValue.GetBoolean(b);
		}
		return false;
	}


    public static bool IsCharEqual<T1, T2>(T1 a, T2 b) {
		if (PrimitiveTypeCheck.IsChar(a) && PrimitiveTypeCheck.IsChar(b)) {
			return PrimitiveValue.GetChar(a) == PrimitiveValue.GetChar(b);
		}
        else if (PrimitiveTypeCheck.IsSignedNumber(a) || PrimitiveTypeCheck.IsSignedNumber(b)) {
            long num = PrimitiveTypeCheck.IsSignedNumber(a)
                ? PrimitiveValue.GetSignedNumber(a)
                : PrimitiveValue.GetSignedNumber(b);
			char ch = PrimitiveTypeCheck.IsChar(a)
                ? PrimitiveValue.GetChar(a)
                : PrimitiveValue.GetChar(b);
			return num == ch;
		}
		else {
			return false;
		}
	}



}



}
