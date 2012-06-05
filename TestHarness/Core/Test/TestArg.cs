using System;

namespace Ca.Roop.TestHarness.Core.Test {

    /// <summary>Encapsulates one Test Case argument.</summary>
    public class TestArg {

        public String Name { get; private set; }

        public String Value { get; private set; }

        public String Type { get; private set; }

        public String ItemType { get; private set; }


        // Do not allow default construction
        private TestArg() { }


        /// <summary>Constructor.</summary>
        /// <param name="name">Argument name.</param>
        /// <param name="value">Argument value.</param>
        /// <param name="type">argument type.</param>
        /// <param name="itemType">
        /// This would apply if the main type is a generic container
        /// </param>
        public TestArg(String name, String value, String type, String itemType) {
            this.Name = name;
            this.Value = value;
            this.Type = type;
            this.ItemType = itemType;
        }

    }

}
