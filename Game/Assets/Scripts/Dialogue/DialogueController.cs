using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;



public class DialogueController : MonoBehaviour
{
    public float textSpeed;
    public float textHeight; // type stuff until text box overflows, this is the last valid height
    public string fileName;

    //Canvas fullCanvas;
    Canvas smallCanvas;
    Text smallText;
    Text nameText;

   
    int entryNo;
    int sentenceNo;
    
    string previousSmallText;

    List<string> currentTextArr;

    float timer;

    bool done;

    bool overflowed;

    DialogueRoot dr;

    bool noMoreDialogue;

    protected virtual void Awake()
    {
        //fullCanvas = GameObject.FindGameObjectWithTag("Full Dialogue Canvas").GetComponent<Canvas>();
        smallCanvas = GameObject.FindGameObjectWithTag("Small Dialogue Canvas").GetComponent<Canvas>();
        //smallText = smallCanvas.GetComponentInChildren<Text>();
        smallText = GameObject.FindGameObjectWithTag("Dialogue Text").GetComponent<Text>();
        nameText = GameObject.FindGameObjectWithTag("Name Text").GetComponent<Text>();
    }

    protected virtual void Start()
    {
        if (fileName != "")
        {
            dr = new DialogueRoot();
            //dr = DialogueRoot.LoadFromObject(path);
            //dr = DialogueRoot.Load(GetPath());
            dr = DialogueRoot.LoadFromResources(fileName);

            // If couldn't find file
            if (dr.IsEmpty())
            {
                Finish();
            }
            else
            {
                smallCanvas.enabled = true;
                entryNo = 0;
                timer = 0;
                currentTextArr = new List<string>();
                done = false;
                overflowed = false;
                noMoreDialogue = false;

                GetNextDialogue();

                textHeight = smallText.rectTransform.rect.size.y;
            }   
        }
        else
        {
            Finish();
        }
    }

    void Update()
    {
        if (!noMoreDialogue)
        {
            // Move to next on click
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // If the current dialgoue entry is finished
                if (done)
                {
                    done = false;

                    // If the current dialogue entry was the last one, finish up
                    if (entryNo >= dr.dialogueEntries.Count())
                    {                        
                        Finish();
                    }
                    // Otherwise get the next dialogue entry
                    else
                    {
                        // Split on sentences to deal with overflows
                        GetNextDialogue();
                    }
                }
                // If overflowed, go to next page for current dialogue entry
                else if (overflowed)
                {
                    overflowed = false;
                    smallText.text = "";
                    timer = 0;
                    previousSmallText = "";
                }
            }

            // If there are sentences left to print
            if (currentTextArr.Count > 0 && !overflowed && !done)
            {
                // If the current sentence is empty
                if (currentTextArr[sentenceNo].Length == 0)
                {
                    sentenceNo++;

                    // If the current sentence was the last
                    if (sentenceNo >= currentTextArr.Count - 1)
                    {
                        done = true;
                    }
                    // Otherwise check the next sentence fits
                    else
                    {
                        previousSmallText = smallText.text;
                        smallText.text += currentTextArr[sentenceNo];

                        if (smallText.preferredHeight > textHeight)
                        {
                            overflowed = true;
                        }

                        smallText.text = previousSmallText;
                    }

                }
                // Otherwise print the next character
                else
                {
                    timer += Time.deltaTime;
                    if (timer > textSpeed)
                    {
                        timer = 0;
                        string letter = currentTextArr[sentenceNo].Substring(0, 1);
                        currentTextArr[sentenceNo] = currentTextArr[sentenceNo].Substring(1);
                        smallText.text += letter;
                    }
                }
            }
        }         
    }

    // Dialogue entries are split on full stops, so they need to be put back
    void PutFullStopsBack()
    {
        for (int i = 0; i < currentTextArr.Count; i++)
        {
            currentTextArr[i] = currentTextArr[i] + ".";
        }
    }

    protected string GetName()
    {
        return "";
    }

    // Move on to the next thing, depends on the scene
    protected virtual void Finish()
    {
        noMoreDialogue = true;
        smallCanvas.enabled = false;
        done = true;
    }

    public bool Displaying()
    {
        return !noMoreDialogue;
    }

    public void GetNextDialogue()
    {
        currentTextArr = new List<string>(dr.dialogueEntries[entryNo].text.Split('.'));
        PutFullStopsBack();
        smallText.text = "";
        nameText.text = dr.dialogueEntries[entryNo].character;

        entryNo++;
        timer = 0;
        sentenceNo = 0;
        previousSmallText = "";
    }

    string GetPath()
    {
        return fileName;
    }
}
