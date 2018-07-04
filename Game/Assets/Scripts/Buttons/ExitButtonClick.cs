using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButtonClick : ButtonClick {

    protected override void OnClick()
    {
        ic.ExitProgram();
    }
}
