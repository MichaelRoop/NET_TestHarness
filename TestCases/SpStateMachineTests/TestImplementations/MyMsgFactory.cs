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

        public ISpEventMessage GetResponse(ISpEventMessage msg) {
            //Log.Info("MyState", "GetResponseMsg", String.Format("For msg:{0}", SpConverter.IntToEnum<MyMsgType>(msg.TypeId)));
            MyBaseResponse response = new MyBaseResponse(MyMsgType.SimpleResponse, msg, MyReturnCode.FailedPresure, "lalalal");
            Log.Info("MyState", "GetResponseMsg", String.Format(" ********** Made bogus response msg:{0}", SpConverter.IntToEnum<MyMsgType>(response.TypeId)));
            return response;
            
            //// will get it from a factory eventually
            //int responseMsgTypeId = 22;
            //return new SpBaseResponse(responseMsgTypeId, (SpBaseMsg)msg);
        }

        #endregion
    }
}
