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
    public float fadeTime;
    
    //Canvas fullCanvas;
    protected Canvas smallCanvas;
    protected Text smallText;
    protected Text nameText;

    protected int entryNo;
    protected int wordNo;
    protected string previousSmallText;
    protected float previousHeight;
    protected List<string> currentTextArr;
    protected float timer;
    protected bool done;
    protected bool overflowed;
    protected DialogueRoot dr;
    protected bool noMoreDialogue;
    Image dialogueCanvasImage;
    float alpha;
    bool starting;
    Color nameTextColour;
    bool emptyDialogue;

    protected virtual void Awake()
    {
        //fullCanvas = GameObject.FindGameObjectWithTag("Full Dialogue Canvas").GetComponent<Canvas>();
        smallCanvas = GameObject.FindGameObjectWithTag("Small Dialogue Canvas").GetComponent<Canvas>();
        //smallText = smallCanvas.GetComponentInChildren<Text>();
        smallText = GameObject.FindGameObjectWithTag("Dialogue Text").GetComponent<Text>();
        nameText = GameObject.FindGameObjectWithTag("Name Text").GetComponent<Text>();
        dialogueCanvasImage = GameObject.FindGameObjectWithTag("Dialogue Panel").GetComponent<Image>();
    }

    protected virtual void Start()
    {
        emptyDialogue = false;
        if (fileName != "")
        {
            // Convert the scripts if in editor
            if (Application.isEditor)
            {
                DialogueConverter.Convert();                
            }

            dr = new DialogueRoot();
            //dr = DialogueRoot.LoadFromObject(path);
            //dr = DialogueRoot.Load(GetPath());
            dr = DialogueRoot.LoadFromResources(fileName);

            // If couldn't find file or not doing dialogue
            if (dr.IsEmpty() || LevelController.levelController.editor)
            {
                emptyDialogue = true;
                Finish();
            }
            else
            {
                //smallCanvas.enabled = true;
                entryNo = 0;
                timer = 0;
                currentTextArr = new List<string>();
                done = false;
                overflowed = false;
                //noMoreDialogue = false;

                GetNextDialogue();

                textHeight = smallText.rectTransform.rect.size.y;

                noMoreDialogue = true;
                smallCanvas.enabled = true;
                alpha = 0;
                starting = false;
                dialogueCanvasImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                nameTextColour = nameText.color;
                nameText.color = new Color(nameTextColour.r, nameTextColour.g, nameTextColour.b, 0.0f);                
            }   
        }
        else
        {
            Finish();
            emptyDialogue = true;
        }
    }

    protected virtual void Update()
    {
        if (!noMoreDialogue && !emptyDialogue)
        {
            // Move to next on click
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // If the current dialgoue entry is finished
                if (done)
                {          
                    // If the current dialogue entry was the last one, finish
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
                            smallText.text = previousSmallText + "k"; // Doesn't work with spaces...
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


        // Fade in
        if (starting)
        {
            timer += Time.deltaTime;

            if (alpha <= (218.0f / 255.0f))
            {
                alpha += Time.deltaTime / fadeTime;
                dialogueCanvasImage.color = new Color(1.0f, 1.0f, 1.0f, alpha);
                nameText.color = new Color(nameTextColour.r, nameTextColour.g, nameTextColour.b, alpha);
            }
            else
            {
                dialogueCanvasImage.color = new Color(1.0f, 1.0f, 1.0f, (218.0f / 255.0f));
                nameText.color = new Color(nameTextColour.r, nameTextColour.g, nameTextColour.b, (218.0f / 255.0f));
                noMoreDialogue = false;
                starting = false;
            }
        }
    }

    // Start the fade in
    public void StartShowingDialogue()
    {
        starting = true;        
    }

    // Dialogue entries are split on spaces, so they need to be put back
    protected void PutSpacesBack()
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

    public virtual void GetNextDialogue()
    {
        currentTextArr = new List<string>(dr.dialogueEntries[entryNo].text.Split(' '));       
        string character = dr.dialogueEntries[entryNo].character;
        entryNo++;

        PutSpacesBack();
        smallText.text = "";
        nameText.text = character;
        done = false;            
        timer = 0;
        wordNo = 0;
        previousSmallText = "";           
    }

    string GetPath()
    {
        return fileName;
    }
}
