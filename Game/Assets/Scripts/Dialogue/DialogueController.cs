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
    int wordNo;
    
    string previousSmallText;
    float previousHeight;

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

            // If couldn't find file or not doing dialogue
            if (dr.IsEmpty() || LevelController.levelController.editor)
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
                        // Split on words to deal with overflows
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

            // If there are words left to print
            if (currentTextArr.Count > 0 && !overflowed && !done)
            {
                // If the current word is empty
                if (currentTextArr[wordNo].Length == 0)
                {
                    wordNo++;

                    // If the current word was the last
                    if (wordNo >= currentTextArr.Count)
                    {
                        done = true;
                    }
                    // Otherwise check the next word fits
                    else
                    {
                        previousHeight = smallText.preferredHeight;
                        previousSmallText = smallText.text;
                        smallText.text += currentTextArr[wordNo];
                        float newHeight = smallText.preferredHeight;


                        if (smallText.preferredHeight > textHeight)
                        {
                            overflowed = true;
                        }
                        // If the word overflows on to the next line, go to the next line
                        else if (previousHeight != newHeight)
                        {
                            // If it's not going to a new line anyway
                            smallText.text = previousSmallText + "k";
                            if (smallText.preferredHeight != newHeight)
                            {                                
                                previousSmallText += Environment.NewLine;
                            }     
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
                        string letter = currentTextArr[wordNo].Substring(0, 1);
                        currentTextArr[wordNo] = currentTextArr[wordNo].Substring(1);
                        smallText.text += letter;
                    }
                }
            }
        }         
    }

    // Dialogue entries are split on spaces, so they need to be put back
    void PutSpacesBack()
    {
        for (int i = 0; i < currentTextArr.Count; i++)
        {
            //currentTextArr[i] = currentTextArr[i] + ".";
            currentTextArr[i] = currentTextArr[i] + " ";
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
        currentTextArr = new List<string>(dr.dialogueEntries[entryNo].text.Split(' '));
        PutSpacesBack();
        smallText.text = "";
        nameText.text = dr.dialogueEntries[entryNo].character;

        entryNo++;
        timer = 0;
        wordNo = 0;
        previousSmallText = "";
    }

    string GetPath()
    {
        return fileName;
    }
}
