namespace Undefinable.Input {
    ///<summary>
    /// A scriptable object that can be used to store input settings.
    /// This should probably be a dictionary, but it works for now.
    ///</summary>

    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Controls", menuName = "ScriptableObjects/Input", order = 1)]
    public class ControlScheme : ScriptableObject {
        public List<inputtype> inputs = new List<inputtype>();
        public bool invertedX = false;
        public bool invertedY = false;
        public float sensivity = 3f;
    }
}