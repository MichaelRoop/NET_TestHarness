//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SpStateMachine.Interfaces;
//using SpStateMachine.Converters;

//namespace TestCases.SpStateMachineTests.TestImplementations {

//    /// <summary>
//    /// Implementation of ISpIdConverter to convert from int the 
//    /// enum id strings
//    /// </summary>
//    public class MyIdConverter : ISpIdConverter {

//        #region Static Singleton Support

//        // Just used as a shortcut. Should use DI

//        private static MyIdConverter instance = null;

//        public static MyIdConverter Instance {
//            get {
//                if (MyIdConverter.instance == null) {
//                    MyIdConverter.instance = new MyIdConverter();
//                }
//                return MyIdConverter.instance;
//            }
//        }

//        #endregion

//        #region ISpIdConverter Members

//        /// <summary>
//        /// Convert the type id to string
//        /// </summary>
//        /// <param name="id">The id to convert to string</param>
//        /// <returns></returns>
//        public string StateId(int id) {
//            return SpConverter.IntToEnum<MyStateID>(id).ToString();
//        }


//        /// <summary>
//        /// Convert the event id to string
//        /// </summary>
//        /// <param name="id">The id to convert to string</param>
//        /// <returns></returns>
//        public string EventId(int id) {
//            return SpConverter.IntToEnum<MyEventType>(id).ToString();
//        }


//        /// <summary>
//        /// Convert the message id to string
//        /// </summary>
//        /// <param name="id">The message id to convert to string</param>
//        /// <returns></returns>
//        public string MsgTypeId(int id) {
//            return SpConverter.IntToEnum<MyMsgType>(id).ToString();
//        }

//        #endregion

//    }
//}
