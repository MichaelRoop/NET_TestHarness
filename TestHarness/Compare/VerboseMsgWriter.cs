using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ca.Roop.TestHarness.Core;

namespace Ca.Roop.TestHarness.Compare {

public class VerboseMsgWriter : IBufferWriter {

    protected ITestable testable;
    private int tickCount;

    private VerboseMsgWriter() { }


    public VerboseMsgWriter(ITestable testable) {
        this.testable = testable;
        tickCount = 0;
    }


    public void Write(string msg) {
        if (msg.Length > 0) {
            if (this.testable.VerboseBuffer.Length > 0) {
                this.testable.VerboseBuffer.AppendLine("");
            }
            this.testable.VerboseBuffer.Append(msg);
        }
    }


    public virtual bool StopOnFirstError() {
        return false;
    }


    public virtual bool WriteOnError<T, T2>(bool success, T expected, T2 actual) {
        throw new NotImplementedException();
    }


    public virtual void Tick() {
        this.tickCount += 1;
    }


    protected int GetTickCount() {
        return this.tickCount;
    }

}

}
