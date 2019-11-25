using System;
using ChkUtils.Net;
using SpStateMachine.Interfaces;

namespace SpStateMachine.Core {

    /// <summary>
    /// Implementation of the MsgFactory that takes a Message provider
    /// and wraps the message retrieval to insure safety and the transferal
    /// of GUIDs to the response to satisfy message correlation
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public sealed class SpMsgFactory : ISpMsgFactory {

        #region Data

        ISpMsgProvider provider = null;

        #endregion

        #region Constructors

        /// <summary>Default constructor in private scope to prevent usage</summary>
        private SpMsgFactory() {
        }


        /// <summary>Constructor</summary>
        /// <param name="provider">The message provider</param>
        public SpMsgFactory(ISpMsgProvider provider) {
            WrapErr.ChkParam(provider, "provider", 9999);
            this.provider = provider;
        }

        #endregion
        
        #region ISpMsgFactory Members

        /// <summary>
        /// Returns default success response with only the GUID transfered from the 
        /// incoming message to satisfy any need of msg correlation
        /// </summary>
        /// <param name="msg">The message received by the state machine</param>
        /// <returns>The default response message</returns>
        public ISpEventMessage GetDefaultResponse(ISpEventMessage msg) {
            return this.GetMsg(msg, () => {
                return this.provider.DefaultMsg(msg);
            });
        }


        public ISpEventMessage GetResponse(ISpEventMessage msg) {
            return this.GetMsg(msg, () => {
                return this.provider.Response(msg);
            });
        }

        public ISpEventMessage GetResponse(ISpEventMessage msg, ISpEventMessage registeredMsg) {
            return this.GetMsg(msg, () => {
                return this.provider.Response(msg, registeredMsg);
            });
        }

        #endregion

        #region Private Methods

        /// <summary>Safely execute function and transfer the GUID to the response</summary>
        /// <param name="msg">The incoming message</param>
        /// <param name="func">The function to invoke to generate the response message</param>
        /// <returns>A correlated response message</returns>
        private ISpEventMessage GetMsg(ISpEventMessage msg, Func<ISpEventMessage> func) {
            WrapErr.ChkParam(msg, "msg", 9999);

            return WrapErr.ToErrorReportException(9999, () => {
                ISpEventMessage ret = func.Invoke();
                WrapErr.ChkVar(ret, 9999, "The Provider Returned a Null Message");

                // Transfer the GUID for correlation
                ret.Uid = msg.Uid;
                return ret;
            });

        }

        #endregion

    }

}
