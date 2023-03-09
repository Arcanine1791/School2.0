using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using TMPro;
using BNG;

public class DialogueManger : MonoBehaviour
{
    public TMP_Text characterName;
    public TMP_Text messageText;
    public RectTransform BgBox;

    MyCharacter[] currentCharacters;
    MyMessage[] currentMessages;
    int activeMessage = 0;
    int activeClip = 0;

    public AudioSource audioSource;
    public AudioClip[] clips;
    public void OpenDialogue(MyMessage[] messages, MyCharacter[] characters)
    {
        currentMessages = messages;
        currentCharacters = characters;
        activeMessage = 0;
        activeClip = 0;

        Debug.Log("Started" + messages.Length);
        DisplayMessage();
    }

    void DisplayMessage()
    {
        MyMessage DispMessage = currentMessages[activeMessage];
        messageText.text = DispMessage.message;

        MyCharacter DispCharacter = currentCharacters[DispMessage.characterID];
        characterName.text = DispCharacter.name;

        audioSource.PlayOneShot(clips[activeClip]);
    }

    public void nextMessage()
    {
        activeMessage++;
        activeClip++;
        if (activeMessage < currentMessages.Length)
            DisplayMessage();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K) || InputBridge.Instance.AButtonDown)
        {
            nextMessage();
        }
    }
}
