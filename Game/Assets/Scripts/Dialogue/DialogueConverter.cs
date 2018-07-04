using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DialogueConverter : MonoBehaviour {

    public string fileName;
    public string sourceFolder;
    
	void Start () {
        DialogueRoot.EncryptFile(GetXMLName(), GetEncName());
        Debug.Log(DialogueRoot.DecryptFile(GetEncName()));
    }

    string GetXMLName()
    {
        return sourceFolder + fileName + ".xml";
    }

    string GetEncName()
    {
        return Application.streamingAssetsPath + @"/Scripts/" + fileName + ".bytes";
    }
}

