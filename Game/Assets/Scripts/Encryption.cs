using System.IO;
using System;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;

public class Encryption  {

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
        catch (Exception e)
        {
            Debug.Log("Encryption failed: " + e.ToString());
        }
    }

    public static void EncryptString(string inputString, string outputFile)
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

            //FileStream fsIn = new FileStream(inputFile, FileMode.Open);
            Stream fsIn = GetStream(inputString);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
            {
                cs.WriteByte((byte)data);
            }

            fsIn.Close();
            cs.Close();
            fsCrypt.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Encryption failed: " + e.ToString());
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
