using DG.Tweening;
using Dogabeey;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Unity.Cinemachine;
using System.Linq;

public class DialogueUI : MonoBehaviour
{
    public DialogueChain dialogueChain;
    public float textSpeed = 0.1f;
    public TMP_Text ownerDisplay;
    public TMP_Text textDisplay;
    public Button continueButton;
    public Image continueIndicator;

    private InputSystem_Actions controls;
    private bool canContinue = false;
    private bool spedUp = false;
    private bool holdingContinue = false;
    private CinemachineCamera cinemachineCamera;
    private CinemachineBrainEvents cinemachineBrainEvent;
    private bool cameraReachedPosTrigger = false;

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Interact.started += Interact_performed;
        controls.Player.Jump.started += Interact_performed;
        controls.Player.Interact.canceled += Interact_canceled;
        controls.Player.Jump.canceled += Interact_canceled;

    }


    private void OnDisable()
    {
        controls.Player.Interact.started -= Interact_performed;
        controls.Player.Interact.canceled -= Interact_canceled;
        controls.Player.Jump.started -= Interact_performed;
        controls.Player.Jump.canceled -= Interact_canceled;
        controls.Player.Disable();

    }

    private void TriggerCameraCut()
    {
        Debug.Log("Camera reached position");

        cameraReachedPosTrigger = true;
        DOVirtual.DelayedCall(0.1f, () => { cameraReachedPosTrigger = false; });
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        TryAdvance();
        holdingContinue = true;
    }
    private void Interact_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        holdingContinue = false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        controls = new InputSystem_Actions();

        cinemachineCamera = Camera.main.GetComponent<CinemachineCamera>();
        cinemachineBrainEvent = FindAnyObjectByType<CinemachineBrainEvents>();
    }

    private void Start()
    {
        DrawInit();
        if (dialogueChain.startOnAwake)
        {
            StartDialogue();
        }
    }
    // Update is called once per frame
    void Update()
    {
        DrawUpdate();
    }

    void DrawInit()
    {
        textDisplay.text = "";
        if (ownerDisplay) ownerDisplay.text = dialogueChain.GetCurrentNode().dialogueOwner;
        if (continueButton) continueButton.onClick.AddListener(TryAdvance);
    }
    void DrawUpdate()
    {
        continueIndicator.enabled = canContinue;
    }

    public void StartDialogue()
    {
        if (dialogueChain != null)
        {
            if(dialogueChain.active)
            {
                spedUp = false;
                canContinue = false;
                StartCoroutine(DialogueCoroutine(dialogueChain.GetCurrentNode().dialogueText));
            }
        }
    }

    IEnumerator DialogueCoroutine(string text)
    {
        DialogueNode node = dialogueChain.GetCurrentNode();

        if (node != null)
        {
            node.onDialogueStart.Invoke();

            if (node.waitForFocusBeforeDialogue)
                yield return StartCoroutine(FocusCameraToFocusTarget());
            else
                StartCoroutine(FocusCameraToFocusTarget());
        }
        // Set color of text
        textDisplay.color = node.textColor;

        // Trigger speech animation
        if (node.dialogueFocusObject != null)
        {
            Animator anim = node.dialogueFocusObject.GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.SetTrigger(node.speechAnimationTriggerString);
            }
        }

        for (int i = 0; i < text.Length; i++)
        { 
            if(i % 10 == 0 && node.dialogueFocusObject)
            {
                Animator anim = node.dialogueFocusObject.GetComponentInChildren<Animator>();
                anim.SetTrigger(node.speechAnimationTriggerString);
            }

            textDisplay.text += text[i];
            if((holdingContinue || spedUp))
            {
                yield return new WaitForSeconds(textSpeed/ 10);
            }
            else
            {
                yield return new WaitForSeconds(textSpeed);
                //SoundManager.Instance.Play(Const.SOUNDS.EFFECTS.TYPEWRITER);
            }
        }
        if (dialogueChain.GetCurrentNode().onDialogueEnd != null) dialogueChain.GetCurrentNode().onDialogueEnd.Invoke();
        canContinue = true;
    }
    private IEnumerator FocusCameraToFocusTarget()
    {
        DialogueNode node = dialogueChain.GetCurrentNode();
        if (node.dialogueFocusObject != null)
        {
            CinemachinePositionComposer posComposer = cinemachineCamera.GetComponent<CinemachinePositionComposer>();
            posComposer.Damping = Vector3.one * node.focusTime;
            cinemachineCamera.Target.TrackingTarget = node.dialogueFocusObject.transform;
            yield return new WaitForSeconds(node.focusTime);
        }

        yield break;
    }

    void TryAdvance()
    {
        if (canContinue)
        {
            dialogueChain.Advance();
            textDisplay.text = "";
            StartDialogue();
        }
        else
        {
            spedUp = true;
        }
    }
}
