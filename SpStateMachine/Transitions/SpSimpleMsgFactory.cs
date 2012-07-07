//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SpStateMachine.Interfaces;
//using SpStateMachine.Core;
//using ChkUtils;

//namespace SpStateMachine.Transitions {

//    public abstract class SpSimpleMsgFactory : ISpMsgFactory {



//        #region ISpMsgFactory Members

//        public abstract ISpEventMessage GetDefaultResponse(ISpEventMessage msg)

//        public abstract ISpEventMessage GetResponse(ISpEventMessage msg);


//        private ISpEventMessage RetrieveMsg(ISpEventMessage msg) {
//            WrapErr.ChkParam(msg, "eventMsg", 9999);
//            ISpEventMessage ret = this.GetResponse(msg);
//            WrapErr.ChkVar(ret, 9999, "The call to overriden 'GetReponseMsg' returned a null event message");
//            return ret;
//        }





//        public ISpStateTransition GetOnEventTransition(ISpStateTransition tr, ISpEventMessage msg) {
//            // No transition registered
//            if (tr == null) {
//                return null;
//            }

//            // Transition is registered. Now check if there is a return message registered with it.
//            if (tr.ReturnMessage != null) {

//            }



//                if (tr.ReturnMessage == null) {
//                    tr.ReturnMessage = this.RetrieveMsg(msg);
//                }
//                else {
//                    // Transfer existing GUID to correlate with sent message
//                    // TODO - still would have to figure out how to transfer the payload for response
//                    tr.ReturnMessage.Uid = msg.Uid;
//                }

//                // TODO - This needs some more thought - Call to derived class to get the return message related to the incoming message
//                //tr.ReturnMessage = this.OnGetResponseMsg(this.GetReponseMsg(eventMsg));
//                tr.ReturnMessage = this.GetResponseMsg(eventMsg);
//                return tr;
//            }



//            //// No Transaction was registered to the state
//            //if (registered == null) {
//            //    if (!getDefault) {
//            //        return registered;
//            //    }
//            //    // Should be cloned
//            //    ISpEventMessage ret = this.GetResponse(msg);
//            //    if (ret.Uid != msg.Uid) {
//            //        // Make sure GUID is transmitted for correlation
//            //        ret.Uid = msg.Uid;
//            //    }
//            //    return new SpStateTransition(SpStateTransitionType.SameState, null, ret);
//            //}
            
//            //// We have a return msg in the registered transaction
//            //if (registered.ReturnMessage != null) {


//            //}


//            return null;

//        }
//    }

//        //public ISpStateTransition GetDefaultTransitionClone() {
//        //    throw new NotImplementedException();
//        //}

//        #endregion
//    }
//}
