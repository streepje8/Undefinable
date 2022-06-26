using UnityEngine;
using UnityEngine.UI;
public class TextRandomboi : MonoBehaviour
{
    private Text text;
    void Start()
    {
        text = GetComponent<Text>();
        text.text = (Random.Range(0, 10)) > 5 ? "Undefinable" : "Undefined";
    }
}
