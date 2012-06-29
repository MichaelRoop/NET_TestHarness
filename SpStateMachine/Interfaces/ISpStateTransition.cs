using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Interfaces {
    
    public interface ISpStateTransition {

        bool HasTransition { get; set; }

        ISpState NextState { get; set; }

        ISpMessage ReturnMessage { get; set; }
        
    }
}
