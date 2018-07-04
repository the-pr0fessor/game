using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButtonClick : ButtonClick {       
    protected override void OnClick()
    {
        ic.HideMenu();
    }
}
