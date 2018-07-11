using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;

[XmlRoot("DialogueRoot")]
public class DialogueRoot
{
    [XmlArray("DialogueEntries"), XmlArrayItem("DialogueEntry")]
    public DialogueEntry[] dialogueEntries;

    //public void Save(string path)
    //{
    //    var serializer = new XmlSerializer(typeof(DialogueRoot));
    //    using (var stream = new FileStream(path, FileMode.Create))
    //    {
    //        serializer.Serialize(stream, this);
    //    }
    //}

    public static DialogueRoot Load(string path)
    {
        var serializer = new XmlSerializer(typeof(DialogueRoot));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as DialogueRoot;
        }
    }

    public static DialogueRoot LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(DialogueRoot));
        return serializer.Deserialize(new StringReader(text)) as DialogueRoot;
    }

    public bool IsEmpty()
    {
        if (dialogueEntries.Length == 0)
        {
            return true;
        }

        return false;
    }

    public static DialogueRoot LoadFromResources(string name)
    {
        return LoadFromText(Encryption.DecryptFile(GetPath(name)));
    }

    public static string GetPath(string name)
    {
        return Application.streamingAssetsPath + @"/Scripts/" + name + ".bytes";
    }

    static Stream GetStream(string s)
    {
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}