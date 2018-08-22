using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionClick : MonoBehaviour {
    public int optionNo;

    DCDialgoue dCDialgoue;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        dCDialgoue = GameObject.FindGameObjectWithTag("Controllers").GetComponent<DCDialgoue>();
    }

    void OnClick()
    {
        dCDialgoue.ClickOption(optionNo);        
    }
}
