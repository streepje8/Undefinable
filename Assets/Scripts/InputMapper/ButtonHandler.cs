namespace Undefinable.Input {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// An inbetween handler which allows for proper timing and updating of the UI
    /// </summary>

    public class ButtonHandler : MonoBehaviour {
        public UnityEvent remapProcess;
        [SerializeField] bool skipBelow, skipInput;
        public UnityEvent uiUpdate;

        // Start the action of remapping
        public void ExecuteAndUpdateOn() {
            remapProcess.Invoke();
            if (!skipBelow) {
                StartCoroutine(CheckMapping());
            }
        }

        // Update the UI after the remap process is done
        IEnumerator CheckMapping() {
            bool _remapping = true;
            while (_remapping) {
                if (Input.anyKeyDown || skipInput) {
                    _remapping = false;
                    yield return new WaitForSeconds(.5f);
                    uiUpdate.Invoke();
                }
                yield return null; //In order to give another code a chance to run, we do this here :)
            }
        }

    }
}