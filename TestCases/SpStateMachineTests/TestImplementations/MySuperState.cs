using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.States;
using SpStateMachine.Interfaces;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public class MySuperState : SpSuperState<MyDataClass> {

        #region Data


        #endregion

        #region Constructors

        public MySuperState(MyStateID id, MyDataClass dataClass)
            : base(id.Int(), dataClass) {
        }

        public MySuperState(ISpState parent, MyStateID id, MyDataClass dataClass)
            : base(parent, id.Int(), dataClass) {
        }

        #endregion

        #region SpStateOverrides

        //public override string Name {
        //    get {
        //        if (this.name.Length == 0) {
        //            StringBuilder sb = new StringBuilder(75);
        //            this.IdChain.ForEach((item) => {
        //                sb.Append(String.Format(".{0}", item.ToStateId().ToString()));
        //            });
        //            this.name = sb.Length > 0 ? sb.ToString(1, sb.Length - 1) : "NameSearchFailed";
        //        }
        //        return this.name;
        //    }
        //}

        protected override string ConvertIdToString(int id) {
            return id.ToStateId().ToString();
        }


        protected override ISpMessage GetDefaultReturnMsg(ISpMessage msg) {
            throw new NotImplementedException();
        }

        protected override ISpMessage GetReponseMsg(ISpMessage msg) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
