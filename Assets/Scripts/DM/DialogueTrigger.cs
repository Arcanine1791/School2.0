using System.Collections;
using System.Collections.Generic;
//using System.Security.Policy;
using UnityEngine;
using UnityEditor.VersionControl;
using System;

public class DialogueTrigger : MonoBehaviour
{
    public MyMessage[] messages;
    public MyCharacter[] characters;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManger>().OpenDialogue(messages, characters);
    }
}

[System.Serializable]
public class MyMessage
{
    public int characterID;
    public string message;
}

[System.Serializable]
public class MyCharacter
{
    public string name;

}
