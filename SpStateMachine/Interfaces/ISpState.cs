using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Interfaces {

    public interface ISpState {

        ISpMessage OnEntry(ISpMessage msg);

        ISpMessage OnTick(ISpMessage msg);

        ISpMessage OnExit(ISpMessage msg);


    }
}
