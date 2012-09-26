using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChkUtils;
using SpStateMachine.Interfaces;

namespace SpStateMachine.Core {

    /// <summary>
    /// Collection of tools useful for factoring out common functionality
    /// </summary>
    /// <author>Michael Roop</author>
    /// <copyright>July 2012 Michael Roop Used by permission</copyright> 
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


        /// <summary>
        /// Register a state transition for an event
        /// </summary>
        /// <param name="eventId">The id converter of the event type</param>
        /// <param name="transition">The transition object</param>
        public static void RegisterTransition(string type, ISpToInt eventId, ISpStateTransition transition, Dictionary<int, ISpStateTransition> store) {
            WrapErr.ChkParam(eventId, "eventId", 51004);
            WrapErr.ChkParam(transition, "transition", 51005);
            WrapErr.ChkParam(store, "store", 51006);

            // Wrap the id converter separately
            int tmp = WrapErr.ToErrorReportException(51007,
                () => { return String.Format("Error on Event Id Converter for '{0}' Event Type", type); },
                () => { return eventId.ToInt(); });

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
        public static ISpStateTransition GetTransitionCloneFromStore(Dictionary<int, ISpStateTransition> store, ISpEventMessage eventMsg) {
            WrapErr.ChkParam(store, "store", 51009);
            WrapErr.ChkParam(eventMsg, "eventMsg", 51010);

            return WrapErr.ToErrorReportException(51011, () => {
                if (store.Keys.Contains(eventMsg.EventId)) {
                    // Clone Transition object from store since its pointers get reset later
                    ISpStateTransition tr = (ISpStateTransition)store[eventMsg.EventId].Clone();

                    if (tr.ReturnMessage == null) {
                        tr.ReturnMessage = eventMsg;
                    }
                    else {
                        //Log.Info("SpTools", "GetTransitionCloneFromStore", "Found a transition AND - Held msg - transfering GUID");
                    }

                    // TODO - Look at transfering the GUID here
                    tr.ReturnMessage.Uid = eventMsg.Uid;


                    //tr.ReturnMessage = eventMsg;
                    return tr;
                }
                return null;
            });
        }




    }
}
