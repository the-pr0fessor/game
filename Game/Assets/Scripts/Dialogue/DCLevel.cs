using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCLevel : DialogueController {
    SceneIC ic;

    protected override void Awake()
    {
        base.Awake();
        ic = GetComponent<SceneIC>();
    }

    protected override void Start()
    {
        ic.StartShowingDialogue();
        base.Start();        
    }

    protected override void Finish()
    {
        base.Finish();
        ic.StopShowingDialogue();
    }
}
