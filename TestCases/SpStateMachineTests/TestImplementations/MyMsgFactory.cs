using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.Interfaces;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using SpStateMachine.Converters;
using LogUtils;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public class MyMsgFactory : ISpMsgFactory {

        #region Static Singleton Support

        private static MyMsgFactory instance = null;

        public static MyMsgFactory Instance {
            get {
                if (MyMsgFactory.instance == null) {
                    MyMsgFactory.instance = new MyMsgFactory();
                }
                return MyMsgFactory.instance;
            }
        }

        #endregion

        #region ISpMsgFactory Members

        public ISpEventMessage GetDefaultResponse(ISpEventMessage msg) {
            return MySpTools.GetDefaultReturnMsg(msg);
        }


        // this will be to transfer content
        public ISpEventMessage GetResponse(ISpEventMessage msg, ISpEventMessage registeredMsg) {
            if (registeredMsg == null) {
                return this.GetResponse(msg);
            }

            // In this scenario we would make a copy of the registered msg and copy in the data from the original incoming message

            // We only have simple messages. Now we can change the data from the cloned message stored in the transition with
            // the incoming message data. We will preserve the event and message type from stored object
            ISpEventMessage ret = this.GetResponse(registeredMsg);
            ret.Priority = msg.Priority;
            ret.ReturnCode = msg.ReturnCode;
            ret.ReturnStatus = msg.ReturnStatus;
            ret.StringPayload = msg.StringPayload;
            return ret;

        }



        public ISpEventMessage GetResponse(ISpEventMessage msg) {

            MyEventType eventType = SpConverter.IntToEnum<MyEventType>(msg.EventId);
            MyMsgType msgType = SpConverter.IntToEnum<MyMsgType>(msg.TypeId);

            // All my messages are Simple Types so I do not need any other info for types. Otherwise I would need a switch
            return new MyBaseMsg(msgType, eventType, msg.Priority) { 
                ReturnCode = msg.ReturnCode, 
                StringPayload = msg.StringPayload, 
                ReturnStatus = msg.ReturnStatus  
            };




            ////Log.Info("MyState", "GetResponseMsg", String.Format("For msg:{0}", SpConverter.IntToEnum<MyMsgType>(msg.TypeId)));
            //MyBaseResponse response = new MyBaseResponse(MyMsgType.SimpleResponse, msg, MyReturnCode.FailedPresure, "lalalal");
            //Log.Info("MyState", "GetResponseMsg", String.Format(" ********** Made bogus response msg:{0}", SpConverter.IntToEnum<MyMsgType>(response.TypeId)));
            //return response;
            
            //// will get it from a factory eventually
            //int responseMsgTypeId = 22;
            //return new SpBaseResponse(responseMsgTypeId, (SpBaseMsg)msg);
        }

        #endregion
    }
}
