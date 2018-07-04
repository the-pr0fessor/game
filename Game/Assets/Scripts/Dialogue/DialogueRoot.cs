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
        return LoadFromText(DecryptFile(GetPath(name)));
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


    // Borrowed from https://www.codeproject.com/articles/26085/file-encryption-and-decryption-in-c
    public static void EncryptFile(string inputFile, string outputFile)
    {
        try
        {
            string password = @"p2nSu6Fd"; // Your Key Here
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = UE.GetBytes(password);

            string cryptFile = outputFile;
            FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

            RijndaelManaged RMCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
            {
                cs.WriteByte((byte)data);
            }
            
            fsIn.Close();
            cs.Close();
            fsCrypt.Close();
        }
        catch
        {
            Debug.Log("Encryption failed");
        }
    }

    public static string DecryptFile(string inputFile)
    {
        string password = @"p2nSu6Fd"; // Your Key Here
        string output = "";
        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] key = UE.GetBytes(password);

        FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

        RijndaelManaged RMCrypto = new RijndaelManaged();

        CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(cs);
        output = sr.ReadToEnd();

        sr.Close();
        cs.Close();
        fsCrypt.Close();

        return output;
    }
}