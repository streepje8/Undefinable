using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Undefinable.Input;
using System.Text.RegularExpressions;

/// <summary>
/// This class simply shows the current state of the mappings
/// </summary>

public class UpdateMappingUI : MonoBehaviour {
    //Fetch data on load
    void Start() => Execute();

    [SerializeField] Text _text;
    [SerializeField] ControlScheme _cs;

    public void Execute() {
        //Make sure both are available to prevent errors.
        if (!_cs || !_text) return;

        //This is a bit funky, but it's a way to get the current mapping for the control scheme
        //There needs to be a thing that shows keycodes instead of numbers, I'm really just not sure how...
        string tmp = JsonUtility.ToJson(_cs, true);
        _text.text = Regex.Replace(tmp, "[^\\w\\.:]", " ").Replace("     ", "\n");
    }

}
