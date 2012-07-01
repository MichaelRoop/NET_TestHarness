using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpStateMachine.States;
using SpStateMachine.Interfaces;
using TestCases.SpStateMachineTests.TestImplementations.Messages;
using ChkUtils;

namespace TestCases.SpStateMachineTests.TestImplementations {

    public class MySuperState : SpSuperState<MyDataClass> {

        #region Data


        #endregion

        #region Constructors

        public MySuperState(MyStateID id, MyDataClass dataClass)
            : base(id.Int(), dataClass) {
        }

        public MySuperState(ISpState parent, MyStateID id, MyDataClass dataClass)
            : base(parent, id.Int(), dataClass) {
        }

        #endregion

        #region SpStateOverrides

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
            WrapErr.ChkFalse(this.registerOnResultUsedDirectly, 9999, "Use the RegisterOnResultTransition version with event id enum");
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




        /// <summary>
        /// Get the default return message for the success
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected override ISpMessage GetDefaultReturnMsg(ISpMessage msg) {
            return MySpTools.GetDefaultReturnMsg(msg);
        }

        protected override ISpMessage GetReponseMsg(ISpMessage msg) {
            // TODO - build a factory to get the right response message

            throw new NotImplementedException();
        }

        #endregion
    }
}
