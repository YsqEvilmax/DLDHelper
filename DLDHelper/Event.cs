using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLDHelper
{
    public abstract class Event
    {
        public Event(object e = null)
        {
            this.m_EventArgs = e;
        }

        public void Run()
        {
            if(OnListening != null)
            {
                OnListening(m_EventArgs);
                if (OnHandling != null)
                {
                    OnHandling(m_EventArgs);
                }
            }
        }
        private object m_EventArgs;

        public Action<object> OnListening;
        public Action<object> OnHandling;
    }
}
