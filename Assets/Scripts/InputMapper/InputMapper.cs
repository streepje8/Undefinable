namespace Undefinable.Input {
    /// <summary>
    /// A Unity Engine Input Mapper that doesn't use the Unity Input Manager
    /// Resources: 
    /// [1] Finding out which key was pressed:
    ///     https://forum.unity.com/threads/find-out-which-key-was-pressed.385250/
    /// [2] Reflection for setting a value by string:
    ///     https://docs.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo.getvalue?view=net-6.0
    /// [3] Loading and saving files
    ///     https://docs.microsoft.com/en-us/dotnet/api/system.io.file?view=net-6.0
    /// [4] Persistant data path
    ///     https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
    /// ---------------------------------------------------------------------------------
    /// [Q] Why do we want to do this? 
    /// [A] Unity does not allow for editing the Input Manager directly from a game.
    ///     While it is a very useful system, as a result remappings are just not possible..? (Why..? Unity please..)
    ///     See resource [1] for more information.
    /// 
    /// [Q] Isn't it just easier to use the basic mapping tools?
    /// [A] While yes, it is definitely easier. 
    ///     However this should allow better user mapping without having to use the old input system.
    /// ---------------------------------------------------------------------------------
    /// (b ᵔ▽ᵔ)b <(Nico was here) ~
    /// 
    /// </summary>

    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEngine;
    using Undefinable.Data;

    //  Todo:
    //  - Look into joystick axis mapping, buttons work fine.

    //  Notes:
    //  - Reflection can be a bit funky.

    public class InputMapper : MonoBehaviour {
        //This is to filter out all relevant keycodes when we're pulling input
        //The range may be adjusted after some testing
        private static readonly KeyCode[] keyCodes = System.Enum.GetValues(typeof(KeyCode))
                                                     .Cast<KeyCode>()
                                                     //.Where(k => ((int)k <= (int)KeyCode.Mouse1))
                                                     .ToArray();

        [SerializeField] ControlScheme controls;
        [SerializeField] string configName = "controls.config";

        /// <summary>
        /// Remap the input to the given control scheme.
        /// </summary>
        /// <param name="target">Which action it should be saved to</param>
        public void Remap(string target) {
            StartCoroutine(FetchInput(target.ToLower()));
        }

        /// <summary>
        /// Save the current remap
        /// </summary>
        public void SaveRemap() {
            DataManagement.SaveData(configName, controls);
        }

        /// <summary>
        /// Load the current saved settings
        /// </summary>
        public void LoadRemap() {
            // Looks kinda funky but it works, definetly *not* fool proof though.
            controls = (ControlScheme)DataManagement.LoadData(configName, controls);
            //Update all UI Doodats
        }

        /// <summary>
        /// Figure out which key the user is pressing and apply it.
        /// </summary>
        /// <param name="target">Which action it should be saved to</param>
        /// <returns></returns>
        IEnumerator FetchInput(string target) {
            print("mapping..");
            bool mapping = true;

            while (mapping) {
                KeyCode value = keyCodes.FirstOrDefault(k => Input.GetKey(k));
                if (value != default) {
                    // This finds the input that needs to be replaced, and sets the new input.
                    // There probably is a better way, but this took a while to figure out :')
                    // The ToLower could be problematic, Maybe use an enum to lock the amount of values we can set.
                    // Could've probably used a dictionary instead? Not sure.
                    // The team wanted remapable buttons so here it is.
                    inputtype type = controls.inputs.Find(x => x.inputName.Equals(target.ToLower(),System.StringComparison.OrdinalIgnoreCase));
                    int oldPosition = controls.inputs.IndexOf(type);
                    controls.inputs.Remove(type);
                    type.inputValue = value;
                    controls.inputs.Insert(oldPosition, type);

                    mapping = false;
                }

                yield return null; //In order to give another code a chance to run, we do this here :)
            }
        }

    }
}