using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpStateMachine.Interfaces {

    public interface IEventStore <T> {

        /// <summary>
        /// Add and event object to the store
        /// </summary>
        /// <param name="eventObject"></param>
        void Add(T eventObject);

        /// <summary>
        /// Pop the next event object from the store
        /// </summary>
        /// <returns></returns>
        T Get();


    }
}
