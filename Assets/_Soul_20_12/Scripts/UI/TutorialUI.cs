using API.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : BaseUIMenu
{
    public static TutorialUI Ins;

    public Text tutorialText;

    private void Awake()
    {
        Ins = this;
    }

    public void SetDetail( string text)
    {
        tutorialText.text = text;
    }
}
