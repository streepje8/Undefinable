using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public Animator fadeImage;

    private void Awake()
    {
        Instance = this;
    }

    public void FadeIn()
    {
        fadeImage?.Play("FadeIn");
    }

    public void FadeOut()
    {
        fadeImage?.Play("FadeOut");
    }
}
