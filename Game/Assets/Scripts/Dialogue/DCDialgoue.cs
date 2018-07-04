using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCDialgoue : DialogueController {

    protected override void Finish()
    {
        base.Finish();
        LevelController.levelController.LoadNextLevel();
    }
}
