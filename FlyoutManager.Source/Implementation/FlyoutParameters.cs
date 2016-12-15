using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bulldog.FlyoutManager
{
    /// <summary>
    /// A collection of parameters that are passed from objects requesting the Flyout to open to the IFlyout ViewModel that controls
    /// the specific Flyout.
    /// These parameters can also (optionally) be attached to FlyoutEventArgs when notifying subscribers of a change in the Flyout status.
    /// </summary>
    public class FlyoutParameters : IEnumerable
    {
        IDictionary<string, object> parameters;

        public FlyoutParameters()
        {
            parameters = new Dictionary<string, object>();
        }

        void AddParameter( string name, object payload )
        {
            parameters.Add( name, payload );
        }

        public object this[string key]
        {
            get { return parameters[key]; }
            set { parameters[key] = value; }
        }

        public bool ContainsKey(string key)
        {
            return parameters.ContainsKey( key );
        }

        public IEnumerator GetEnumerator()
        {
            return parameters.GetEnumerator();
        }
    }
}
