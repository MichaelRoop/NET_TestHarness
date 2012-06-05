using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ca.Roop.TestHarness.Compare {

class ListComparator {

    public static bool IsEqualTo<T, T2>(T expected, T2 actual, IBufferWriter errWriter) {
        // Reduntant checks.        
        if (expected == null || actual == null) {
            return errWriter.WriteOnError(expected == null && actual == null, expected, actual);
        }

        Type typeExpected   = expected.GetType();
        Type typeActual     = actual.GetType();

        // reference equality.
        if (typeExpected.IsByRef && typeActual.IsByRef) {
            if (object.ReferenceEquals(expected, actual)) {
                return true;
            }
        }

        // Only want to do compare on the List without what it may be holding.  Only way I 
        // can find at this point for equality without knowing what the type of the Items are.
        Type t = typeof(List<>);
        if (typeExpected.Name != t.Name || typeActual.Name != t.Name) {
            return false;
        }

        // At this point we know we have 2 lists.
        int expectedCount = GetCount(expected);
        int actualCount = GetCount(actual);

        if (expectedCount != actualCount) {
            errWriter.Write("Size mismach between Lists");
            return errWriter.WriteOnError(false, expectedCount, actualCount);
        }

        // At this point we know both same size. Do Comparator on every element
        bool success = true;
        for (int i = 0; i < expectedCount && (success || !errWriter.StopOnFirstError()); i++) {
            errWriter.Tick();
            bool temp = Comparator.IsEqualTo(GetItem(expected, i), GetItem(actual, i), errWriter);
            if (success == true) {
                success = temp;
            }
        }
        return success;
    }


    private static int GetCount(object o) {
        return (int)o.GetType().GetProperty("Count").GetValue(o, null);
    }


    private static object GetItem(object o, int index) {
        return o.GetType().GetProperty("Item").GetValue(o, new object[]{ index });
    }


}

}
