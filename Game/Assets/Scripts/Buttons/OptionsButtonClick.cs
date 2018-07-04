using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButtonClick : ButtonClick {

    protected override void OnClick()
    {
        ic.ShowOptions();
    }
}
