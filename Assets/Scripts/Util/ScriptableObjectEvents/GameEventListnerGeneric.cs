using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Openverse.Events
{
    public abstract class GameEventListnerGeneric : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public GameEvent Event;
        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public abstract void OnEventRaised();
    }
}
