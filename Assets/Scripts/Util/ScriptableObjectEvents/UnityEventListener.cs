// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

namespace Openverse.Events
{
    public class UnityEventListener : GameEventListnerGeneric
    {
        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;

        public override void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}