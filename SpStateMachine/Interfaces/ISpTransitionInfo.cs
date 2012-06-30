using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Core;

namespace SpStateMachine.Interfaces {

    public interface ISpTransitionInfo {

//            SpStateTransitionType type = SpStateTransitionType.OnEvent;

        ISpStateTransition transition { get; set; }

        int MsgId { get; set; }



    }
}
