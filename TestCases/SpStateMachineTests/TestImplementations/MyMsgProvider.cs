using SpStateMachine.Converters;
using SpStateMachine.Interfaces;
using TestCases.SpStateMachineTests.TestImplementations.Messages;

namespace TestCases.SpStateMachineTests.TestImplementations {


    public class MyMsgProvider : ISpMsgProvider {

        public MyMsgProvider() {
        }

        #region ISpMsgProvider Members

        public ISpEventMessage DefaultMsg(ISpEventMessage msg) {
            return new MySimpleOkResponse(msg);
        }

        public ISpEventMessage Response(ISpEventMessage msg) {

            MyMsgId eventType = SpConverter.IntToEnum<MyMsgId>(msg.EventId);
            MyMsgType msgType = SpConverter.IntToEnum<MyMsgType>(msg.TypeId);

            // All my messages are Simple Types so I do not need any other info for types. Otherwise I would need a switch
            return new MyBaseMsg(msgType, eventType, msg.Priority) {
                ReturnCode = msg.ReturnCode,
                StringPayload = msg.StringPayload,
                ReturnStatus = msg.ReturnStatus
            };
        }

        public ISpEventMessage Response(ISpEventMessage msg, ISpEventMessage registeredMsg) {
            if (registeredMsg == null) {
                return this.Response(msg);
            }

            // In this scenario we would make a copy of the registered msg and copy in the data from the original incoming message

            // We only have simple messages. Now we can change the data from the cloned message stored in the transition with
            // the incoming message data. We will preserve the event and message type from stored object
            ISpEventMessage ret = this.Response(registeredMsg);
            ret.Priority = msg.Priority;
            ret.ReturnCode = msg.ReturnCode;
            ret.ReturnStatus = msg.ReturnStatus;
            ret.StringPayload = msg.StringPayload;
            return ret;
        }

        #endregion
    }


    ///// <summary>
    ///// Static wrapper to deliver an instance of the SpMsgFactory with the 
    ///// MyMsgProvider implementation
    ///// </summary>
    //public class MyMsgFactory {

    //    #region Static Singleton Support

    //    private static ISpMsgFactory instance = null;

    //    public static ISpMsgFactory Instance {
    //        get {
    //            if (MyMsgFactory.instance == null) {
    //                MyMsgFactory.instance = new SpMsgFactory(new MyMsgProvider());
    //            }
    //            return MyMsgFactory.instance;
    //        }
    //    }

    //    #endregion

    //    #region Constructors

    //    #endregion


    //    #region ISpMsgFactory Members

    //    //public ISpEventMessage GetDefaultResponse(ISpEventMessage msg) {
    //    //    return MySpTools.GetDefaultReturnMsg(msg);
    //    //}


    //    //// this will be to transfer content
    //    //public ISpEventMessage GetResponse(ISpEventMessage msg, ISpEventMessage registeredMsg) {
    //    //    if (registeredMsg == null) {
    //    //        return this.GetResponse(msg);
    //    //    }

    //    //    // In this scenario we would make a copy of the registered msg and copy in the data from the original incoming message

    //    //    // We only have simple messages. Now we can change the data from the cloned message stored in the transition with
    //    //    // the incoming message data. We will preserve the event and message type from stored object
    //    //    ISpEventMessage ret = this.GetResponse(registeredMsg);
    //    //    ret.Priority = msg.Priority;
    //    //    ret.ReturnCode = msg.ReturnCode;
    //    //    ret.ReturnStatus = msg.ReturnStatus;
    //    //    ret.StringPayload = msg.StringPayload;
    //    //    return ret;

    //    //}



    //    //public ISpEventMessage GetResponse(ISpEventMessage msg) {

    //    //    MyEventType eventType = SpConverter.IntToEnum<MyEventType>(msg.EventId);
    //    //    MyMsgType msgType = SpConverter.IntToEnum<MyMsgType>(msg.TypeId);

    //    //    // All my messages are Simple Types so I do not need any other info for types. Otherwise I would need a switch
    //    //    return new MyBaseMsg(msgType, eventType, msg.Priority) { 
    //    //        ReturnCode = msg.ReturnCode, 
    //    //        StringPayload = msg.StringPayload, 
    //    //        ReturnStatus = msg.ReturnStatus  
    //    //    };




    //    //    ////Log.Info("MyState", "GetResponseMsg", String.Format("For msg:{0}", SpConverter.IntToEnum<MyMsgType>(msg.TypeId)));
    //    //    //MyBaseResponse response = new MyBaseResponse(MyMsgType.SimpleResponse, msg, MyReturnCode.FailedPresure, "lalalal");
    //    //    //Log.Info("MyState", "GetResponseMsg", String.Format(" ********** Made bogus response msg:{0}", SpConverter.IntToEnum<MyMsgType>(response.TypeId)));
    //    //    //return response;
            
    //    //    //// will get it from a factory eventually
    //    //    //int responseMsgTypeId = 22;
    //    //    //return new SpBaseResponse(responseMsgTypeId, (SpBaseMsg)msg);
    //    //}

    //    #endregion
    //}
}
