using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField]
    AudioSource stairwalk;
    [SerializeField]
    Animator fade;
    private void LateUpdate() {
        if (!stairwalk.isPlaying) {
            fade.Play("FadeIn");
            Destroy(this.gameObject);
        }
    }
}
