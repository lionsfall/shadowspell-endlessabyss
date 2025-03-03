using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueNode
{
    [Tooltip("The name of the character speaking. Will be shown over the dialogue box.")]
    public string dialogueOwner;
    [Tooltip("The object that the camera will focus on when this dialogue is active.")]
    public GameObject dialogueFocus;
    [Tooltip("The time it takes for the camera to focus on the dialogueFocus object.")]
    public float focusTime = 1f;
    [Tooltip("Whether or not the camera should wait for the focus to complete before displaying the dialogue.")]
    public bool waitForFocusBeforeDialogue = false;
    [Tooltip("The text that will be displayed in the dialogue box.")]
    public string dialogueText;
    [Tooltip("The event that will be called when this dialogue starts.")]
    public UnityEvent onDialogueStart;
    [Tooltip("The event that will be called when this dialogue ends.")]
    public UnityEvent onDialogueEnd;
}
