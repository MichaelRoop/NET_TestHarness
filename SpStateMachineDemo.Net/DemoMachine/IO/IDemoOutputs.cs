using System;
using System.Collections.Generic;
using System.Text;

namespace SpStateMachineDemo.Net.DemoMachine.IO {


    public interface IDemoOutputs<TId> where TId : struct {

        event EventHandler StateChange;

        void Add(OutputId id);

        IOState GetState(OutputId id);

        void SetState(OutputId id, IOState state);


    }
}
