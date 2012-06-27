using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Interfaces {

    public interface ISpStateMachine<T> {

        void Tick(ISpEvent<T> eventObject);

    }
}
