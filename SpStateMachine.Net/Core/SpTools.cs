using ChkUtils.Net;
using SpStateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpStateMachine.Core {

    /// <summary>
    /// Collection of tools useful for factoring out common functionality
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2019 Michael Roop Used by permission</copyright> 
    public static class SpTools {
        
        /// <summary>
        /// Search through a dictionary for a string equivalent and add if not found
        /// </summary>
        /// <param name="key">The key for the string requested</param>
        /// <param name="currentStrings">The dictionary of strings to query and add to</param>
        /// <param name="converterFunc">The converted to convert the key to a string value if not in the Dictionary</param>
        /// <returns></returns>
        public static string GetIdString(int key, Dictionary<int, string> currentStrings, Func<int, string> converterFunc) {
            WrapErr.ChkParam(currentStrings, "currentStrings", 51000);
            WrapErr.ChkParam(converterFunc, "converterFunc", 51001);
            return WrapErr.ToErrorReportException(51002, () => {
                if (currentStrings.Keys.Contains(key)) {
                    return currentStrings[key];
                }

                // Do another wrap level to isolate the user defined conversion failure
                string ret =  WrapErr.ToErrorReportException(51003, "Error in Calling Id to String Converter Method", () => {
                    return converterFunc.Invoke(key);
                });
                currentStrings.Add(key, ret);
                return ret;
            });
        }


        /// <summary>Register a transition for a state</summary>
        /// <param name="type">string of transition type</param>
        /// <typeparam name="TMsgId">Event id</typeparam>
        /// <param name="msgId">The event message id</param>
        /// <param name="transition">Transition object</param>
        /// <param name="store">Transition store</param>
        public static void RegisterTransition<TMsgId>(string type, TMsgId msgId, ISpStateTransition<TMsgId> transition, Dictionary<int, ISpStateTransition<TMsgId>> store) where TMsgId : struct {
            //WrapErr.ChkParam(eventId, "msgId", 51004);
            WrapErr.ChkParam(transition, "transition", 51005);
            WrapErr.ChkParam(store, "store", 51006);

            WrapErr.ChkTrue(typeof(TMsgId).IsEnum, 9999, () => string.Format("Transition type {0} must be Enum", msgId.GetType().Name));
            WrapErr.ChkTrue(typeof(TMsgId).GetEnumUnderlyingType() == typeof(Int32), 9999, 
                () => string.Format("Transition type enum {0} must be derived from int", msgId.GetType().Name));

            int tmp = Convert.ToInt32(msgId);
            // 51007 - failure of conversion number

            // Duplicate transitions on same Event is a no no.
            WrapErr.ChkFalse(store.Keys.Contains(tmp), 51008,
                () => { return String.Format("Already Contain a '{0}' Transition for Id:{1}", type, tmp); });
            store.Add(tmp, transition);
        }


        /// <summary>
        /// Get a clone of the transition object from the store or null if not found
        /// </summary>
        /// <param name="store">The store to search</param>
        /// <param name="eventMsg">The message to insert in the transition object</param>
        /// <returns>The transition object from the store or null if not found</returns>
        public static ISpStateTransition<TMsgId> GetTransitionCloneFromStore<TMsgId>(Dictionary<int, ISpStateTransition<TMsgId>> store, ISpEventMessage eventMsg) where TMsgId : struct {
            WrapErr.ChkParam(store, "store", 51009);
            WrapErr.ChkParam(eventMsg, "eventMsg", 51010);

            return WrapErr.ToErrorReportException(51011, () => {
                if (store.Keys.Contains(eventMsg.EventId)) {
                    // Clone Transition object from store since its pointers get reset later
                    ISpStateTransition<TMsgId> tr = (ISpStateTransition<TMsgId>)store[eventMsg.EventId].Clone();

                    if (tr.ReturnMessage == null) {
                        tr.ReturnMessage = eventMsg;
                    }
                    else {
                        //Log.Info("SpTools", "GetTransitionCloneFromStore", "Found a transition AND - Held msg - transfering GUID");
                    }

                    tr.ReturnMessage.Uid = eventMsg.Uid;
                    return tr;
                }
                return null;
            });
        }

    }
}
