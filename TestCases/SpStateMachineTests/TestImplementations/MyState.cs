using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.States;
using SpStateMachine.Interfaces;
using LogUtils;
using System.Threading;
using SpStateMachine.Messages;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using ChkUtils;

namespace TestCases.SpStateMachineTests.TestImplementations {

    /// <summary>
    /// Derived class to attach an object that the state machine represents
    /// </summary>
    /// <remarks>Note usage of enum to enforce strong typing at implementation level</remarks>
    public class MyState : SpState<MyDataClass> {


        public MyState(MyStateID id, MyDataClass dataClass)
            : base(id.Int(), dataClass) {
        }

        public MyState(ISpState parent, MyStateID id, MyDataClass dataClass)
            : base(parent, id.Int(), dataClass) {
        }


        #region Safe Guard access to register events

        bool registerOnEventUsedDirectly = true;
        bool registerOnResultUsedDirectly = true;


        public void RegisterOnEventTransition(MyEventType eventId, ISpStateTransition transition) {
            WrapErr.ToErrorReportException(9999,
                delegate {
                    this.registerOnEventUsedDirectly = false;
                    this.RegisterOnEventTransition(eventId.Int(), transition);
                },
                delegate {
                    this.registerOnEventUsedDirectly = true;
                });
        }

        public void RegisterOnResultTransition(MyEventType eventId, ISpStateTransition transition) {
            WrapErr.ToErrorReportException(9999,
                delegate {
                    this.registerOnResultUsedDirectly = false;
                    this.RegisterOnResultTransition(eventId.Int(), transition);
                },
                delegate {
                    this.registerOnResultUsedDirectly = true;
                });
        }


        /// <summary>
        /// Register a state transition from incoming event. Version protected against direct use
        /// </summary>
        /// <param name="eventId">The id of the incoming event</param>
        /// <param name="transition">The transition object</param>
        public override void RegisterOnEventTransition(int eventId, ISpStateTransition transition) {
            WrapErr.ChkFalse(this.registerOnEventUsedDirectly, 9999, "Use the RegisterOnEventTransition version with event id enum");
            base.RegisterOnEventTransition(eventId, transition);
        }


        /// <summary>
        /// Register a state transition from the result of state processing. Version protected against direct use
        /// </summary>
        /// <param name="eventId">The id of the event as the result of state processing</param>
        /// <param name="transition">The transition object</param>
        public override void RegisterOnResultTransition(int eventId, ISpStateTransition transition) {
            WrapErr.ChkFalse(this.registerOnResultUsedDirectly, 9999, "Use the RegisterOnEventTransition version with event id enum");
            base.RegisterOnResultTransition(eventId, transition);
        }

        #endregion


        protected override string ConvertStateIdToString(int id) {
            return id.ToStateId().ToString();
        }

        /// <summary>
        /// Allows derived classes to convert the event id to string if they are using strongly 
        /// typed convetible enums. By default this level just calls int.ToString(). It will also 
        /// make the logs more readeable
        /// </summary>
        /// <param name="id">The id to convert to string</param>
        /// <returns></returns>
        protected override string ConvertEventIdToString(int id) {
            return id.ToEventType().ToString();
        }


        /// <summary>
        /// Allows derived classes to convert the message id to string if they are using strongly 
        /// typed convetible enums. By default this level just calls int.ToString(). It will also 
        /// make the logs more readeable
        /// </summary>
        /// <param name="id">The message id to convert to string</param>
        /// <returns></returns>
        protected override string ConvertMsgTypedToString(int id) {
            return id.ToMsgType().ToString();
        }



        protected override ISpMessage ExecOnEntry(ISpMessage msg) {
            Log.Info("MyState", "ExecOnEntry", String.Format("Raised {0}", msg.EventId));
            This.StrVal = "The message set on Entry";
            This.IntVal = 9876;
            return this.GetDefaultReturnMsg(msg);
        }

        protected override ISpMessage ExecOnTick(ISpMessage msg) {
            Thread.Sleep(200);
            Log.Info("MyState", "ExecOnTick", String.Format("Raised {0} StrVal:{1} IntVal:{2}", msg.EventId, This.StrVal, This.IntVal));
            //return this.GetDefaultReturnMsg(msg);
            return msg;
        }


        protected override void ExecOnExit() {
            Log.Info("MyState", "ExecOnExit", "");
        }


        /// <summary>
        /// Provides the default return msg
        /// </summary>
        /// <param name="msg">The incomming message</param>
        protected override ISpMessage GetDefaultReturnMsg(ISpMessage msg) {
            return MySpTools.GetDefaultReturnMsg(msg);
        }



        protected override ISpMessage GetReponseMsg(ISpMessage msg) {
            Log.Info("MyState", "GetResponseMsg", String.Format("For msg:{0}", msg.TypeId.ToStateId()));

            MyBaseResponse response = new MyBaseResponse(MyMsgType.SimpleResponse, (MyBaseMsg)msg, MyReturnCode.FailedPresure, "lalalal");

            Log.Info("MyState", "GetResponseMsg", String.Format("Made bogus response msg:{0}", response.TypeId.ToStateId()));

            return response;


            //// will get it from a factory eventually
            //int responseMsgTypeId = 22;
            //return new SpBaseResponse(responseMsgTypeId, (SpBaseMsg)msg);
        }


    }
}
