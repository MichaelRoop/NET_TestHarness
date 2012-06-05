using System;
using System.Collections.Generic;
using Ca.Roop.TestHarness.Core.Test;

namespace Ca.Roop.TestHarness.Compare
{

public class Comparator {

    /// <summary>Compares two generic values for equality.</summary>
    /// <typeparam name="T">First generic type to compare.</typeparam>
    /// <typeparam name="T2">Second generic type to compare.</typeparam>
    /// <param name="a">First type to compare.</param>
    /// <param name="b">Second type to compare.</param>
    /// <returns>true if equal, otherwise false.</returns>
    public static bool IsEqualTo<T, T2>(T expected, T2 actual, IBufferWriter errWriter) {
        if (expected == null || actual == null) {
            return errWriter.WriteOnError(expected == null && actual == null,expected, actual);
        }

        // recurse until any args that are TestArgs are converted to their held objects.
        if (expected is TestArg || actual is TestArg) {
            return ConvertTestArgToObject(expected, actual, errWriter);
        }

        Type typeA = expected.GetType();
        Type typeB = actual.GetType();

        // reference equality.
        if (typeA.IsByRef && typeB.IsByRef) {
            if (object.ReferenceEquals(expected, actual)) {
                return true;
            }
        }

        // Equality of primitives.
        if (typeA.IsPrimitive || typeB.IsPrimitive) {
            return errWriter.WriteOnError(
                PrimitiveComparator.IsEqualTo(expected, actual), expected, actual);
        }

        // Only want to do compare on the List without what it may be holding.  Only way I 
        // can find at this point for equality without knowing what the type of the Items are
        // so I can create a comparison object.
        Type l = typeof(List<>);
        if (typeA.Name == l.Name || typeB.Name == l.Name) {
            return ListComparator.IsEqualTo(expected, actual, errWriter);
        }

        // Default equality check.
        return errWriter.WriteOnError(expected.Equals(actual), expected, actual);
    }


    public static bool IsNotEqualTo<T, T2>(T notExpected, T2 actual, IBufferWriter errWriter) {
        return !IsEqualTo(notExpected, actual, errWriter);
    }


    private static bool ConvertTestArgToObject<T, T2>(T a, T2 b, IBufferWriter errWriter) {
        if (a is TestArg) {
            return IsEqualTo(ArgParser.GetValue(a), b, errWriter);
        }
        else if (b is TestArg) {
            return IsEqualTo(a, ArgParser.GetValue(b), errWriter);
        }
        throw new ArgumentException("Neither argument is a TestArg");
    }

}


}
