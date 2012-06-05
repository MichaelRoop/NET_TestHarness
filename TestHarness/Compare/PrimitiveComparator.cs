using System;

namespace Ca.Roop.TestHarness.Compare {

/// <summary>
/// 
/// </summary>
class PrimitiveComparator {

    public static bool IsEqualTo<T, T2>(T a, T2 b) {
        if (a.GetType().IsPrimitive || b.GetType().IsPrimitive) {
            if (!a.GetType().IsPrimitive || !b.GetType().IsPrimitive) {
                return false;
            }
            else if (PrimitiveTypeCheck.IsBoolean(a) || PrimitiveTypeCheck.IsBoolean(b)) {
                return PrimitiveTypeEqual.IsBooleanEqual(a, b);
            }
            else if (PrimitiveTypeCheck.IsReal(a) || PrimitiveTypeCheck.IsReal(b)) {
                return PrimitiveTypeEqual.IsFloatingEqual(a, b);
            }
            else if (PrimitiveTypeCheck.IsChar(a) || PrimitiveTypeCheck.IsChar(b)) {
                return PrimitiveTypeEqual.IsCharEqual(a, b);
            }
            else if (PrimitiveTypeCheck.IsSignedNumber(a) || PrimitiveTypeCheck.IsSignedNumber(b)) {
                return PrimitiveTypeEqual.IsSignedNumberEqual(a, b);
            }
            else if (PrimitiveTypeCheck.IsUnsignedNumber(a) || PrimitiveTypeCheck.IsUnsignedNumber(b)) {
                return PrimitiveTypeEqual.IsUnsignedNumberEqual(a, b);
            }

            // Default equality check for unhandled primitives.
            return (a as ValueType).Equals((b as ValueType));
        }
        throw new ArgumentException("Neither type is a primitive");
    }

}

}
