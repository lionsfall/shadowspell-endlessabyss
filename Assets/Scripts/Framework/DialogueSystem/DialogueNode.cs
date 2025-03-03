using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueNode
{
    [Header("Main Settings")]
    [Tooltip("The name of the character speaking. Will be shown over the dialogue box.")]
    public string dialogueOwner;
    [Tooltip("The text that will be displayed in the dialogue box.")]
    public string dialogueText;
    [Tooltip("The color of the text that will be displayed in the dialogue box.")]
    public Color textColor = Color.white;
    [Tooltip("The event that will be called when this dialogue starts.")]
    public UnityEvent onDialogueStart;
    [Tooltip("The event that will be called when this dialogue ends.")]
    public UnityEvent onDialogueEnd;
    [Header("Object Interaction")]
    [Tooltip("The object that the camera will focus on when this dialogue is active.")]
    public GameObject dialogueFocusObject;
    [Tooltip("The time it takes for the camera to focus on the dialogueFocus object.")]
    public float focusTime = 1f;
    [Tooltip("Whether or not the camera should wait for the focus to complete before displaying the dialogue.")]
    public bool waitForFocusBeforeDialogue = false;
    [Tooltip("The string that will be sent to the animator to trigger a speech animation, granted the focus object has an animator.")]
    public string speechAnimationTriggerString;
}
