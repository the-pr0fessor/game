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
        Encryption.EncryptString(FormatToXML(), GetEncName());
        Debug.Log("Done");
    }

    string GetTextName()
    {
        return sourceFolder + @"\" + fileName + ".txt";
    }

    string GetEncName()
    {
        return Application.streamingAssetsPath + @"/Scripts/" + fileName + ".bytes";
    }

    // Make the XML file from the notation
    string FormatToXML()
    {
        string xml = File.ReadAllText(GetTextName());

        xml = "<DialogueRoot>" + Environment.NewLine + "<DialogueEntries>" + Environment.NewLine + xml + Environment.NewLine + "</DialogueEntries>" + Environment.NewLine + "</DialogueRoot>";

        xml = xml.Replace("[", "<DialogueEntry>" + Environment.NewLine + "<character>");
        xml = xml.Replace(" | ", "</character>" + Environment.NewLine + "<text>");
        xml = xml.Replace("]", "</text>" + Environment.NewLine + "</DialogueEntry>");

        return xml;
    }
}

