using UnityEngine;

/// <summary>
/// Small script trying to avoid multiple players
/// </summary>
public class NoDuplication : MonoBehaviour {
    ActiveInstance[] _instances;
    void Awake() {
        _instances = FindObjectsOfType<ActiveInstance>();
        if (_instances.Length <= 0) {
            gameObject.AddComponent<ActiveInstance>();
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }
}
