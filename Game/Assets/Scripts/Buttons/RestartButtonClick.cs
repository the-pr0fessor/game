using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButtonClick : ButtonClick {

    protected override void OnClick()
    {
        ic.RestartLevel();
    }
}

