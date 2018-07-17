using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DialogueConverter : MonoBehaviour {

    public string sourceFolder;

    void Start()
    {
        // Cycle through each text file
        foreach (string fileName in Directory.GetFiles(sourceFolder, "*.txt"))
        {
            Encryption.EncryptString(FormatToXML(fileName), GetEncName(fileName));
        }

        Debug.Log("Done");
    }

    public static void Convert()
    {
        foreach (string fileName in Directory.GetFiles(@"C:\Users\James2\OneDrive\Documents\Game\Scripts\text", "*.txt"))
        {
            Encryption.EncryptString(FormatToXML(fileName), GetEncName(fileName));
        }
    }

    // Get the path to store the file in the app
    static string GetEncName(string fileName)
    {
        string name = Path.GetFileName(fileName);
        name = name.Substring(0, name.Length - 4);
        return Application.streamingAssetsPath + @"/Scripts/" + name + ".bytes";
    }

    // Make the XML file from the notation
    static string FormatToXML(string fileName)
    {
        string xml = File.ReadAllText(fileName);

        xml = "<DialogueRoot>" + Environment.NewLine + "<DialogueEntries>" + Environment.NewLine + xml + Environment.NewLine + "</DialogueEntries>" + Environment.NewLine + "</DialogueRoot>";
        xml = xml.Replace("[", "<DialogueEntry>" + Environment.NewLine + "<character>");
        xml = xml.Replace(" | ", "</character>" + Environment.NewLine + "<text>");
        xml = xml.Replace("]", "</text>" + Environment.NewLine + "</DialogueEntry>");

        return xml;
    }
}

