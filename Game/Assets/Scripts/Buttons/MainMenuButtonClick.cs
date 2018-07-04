using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonClick : ButtonClick {

    protected override void OnClick()
    {
        ic.LoadMainMenu();
    }
}
