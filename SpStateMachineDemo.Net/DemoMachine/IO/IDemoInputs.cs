using System;
using System.Collections.Generic;
using System.Text;

namespace SpStateMachineDemo.Net.DemoMachine.IO {

    public interface IDemoInputs<TId> where TId : struct {

        event EventHandler StateChange;


        void Add(TId id);

        void SetState(TId id, IOState state);

        IOState GetState(TId id);

    }
}
