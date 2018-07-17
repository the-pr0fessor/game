using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DCLevel : DialogueController {
    SceneIC ic;

    //public float fadeTime;
    
    //float alpha;

    //bool starting;

    protected override void Awake()
    {
        base.Awake();
        ic = GetComponent<SceneIC>();
    }

    protected override void Start()
    {
        ic.StartShowingDialogue();
        base.Start();        
        //noMoreDialogue = true;
        //smallCanvas.enabled = false;
        //alpha = 0;
        //starting = false;
        //smallCanvas.GetComponentInChildren<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    //protected override void Update()
    //{
    //    base.Update();

    //    if (starting)
    //    {
    //        timer += Time.deltaTime;

    //        if (alpha <= (218.0f / 255.0f))
    //        {
    //            alpha += Time.deltaTime / fadeTime;
    //            smallCanvas.GetComponentInChildren<Image>().color = new Color(1.0f, 1.0f, 1.0f, alpha);
    //        }
    //        else
    //        {
    //            smallCanvas.GetComponentInChildren<Image>().color = new Color(1.0f, 1.0f, 1.0f, (218.0f / 255.0f));
    //            noMoreDialogue = false;
    //            starting = false;
    //        }
    //    }
    //}

    //public void StartShowingDialogue()
    //{
    //    starting = true;
    //    smallCanvas.enabled = true;
    //}

    protected override void Finish()
    {
        base.Finish();
        ic.StopShowingDialogue();
    }
}
