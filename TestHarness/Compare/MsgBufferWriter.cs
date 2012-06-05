using System;
using Ca.Roop.TestHarness.Core;

namespace Ca.Roop.TestHarness.Compare {
    
public class MsgBufferWriter : IBufferWriter{

    protected ITestable testable;


    private MsgBufferWriter() {}


    public MsgBufferWriter(ITestable testable) {
        this.testable = testable;
    }


    public void Write(String msg) {
        if (msg.Length > 0) {
            if (this.testable.MsgBuffer.Length > 0) {
                this.testable.MsgBuffer.Append(" - ");
            }
            this.testable.MsgBuffer.Append(msg);
        }
    }


    public virtual bool StopOnFirstError() {
        return true;
    }


    public virtual bool WriteOnError<T, T2>(bool success, T expected, T2 actual) {
        throw new NotImplementedException();
    }


    public virtual void Tick() {
        // Do nothing.
    }

}

}
