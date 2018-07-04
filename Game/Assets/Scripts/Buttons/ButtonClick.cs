using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonClick : MonoBehaviour {

    protected SceneIC ic;

    void Start () {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        ic = GameObject.FindGameObjectWithTag("Controllers").GetComponent<SceneIC>();
    }

    protected virtual void OnClick()
    {

    }
}
