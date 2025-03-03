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

        cinemachineBrainEvent.BlendFinishedEvent.AddListener((_, __) => OnCameraCut(cinemachineCamera, _));
    }


    private void OnDisable()
    {
        controls.Player.Interact.started -= Interact_performed;
        controls.Player.Interact.canceled -= Interact_canceled;
        controls.Player.Jump.started -= Interact_performed;
        controls.Player.Jump.canceled -= Interact_canceled;
        controls.Player.Disable();

        cinemachineBrainEvent.BlendFinishedEvent.RemoveListener((_, __) => OnCameraCut(cinemachineCamera, _));
    }
    private void OnCameraCut(ICinemachineCamera _, ICinemachineMixer __)
    {
        TriggerCameraCut();
    }

    private void TriggerCameraCut()
    {
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
                StartCoroutine(TypeText(dialogueChain.GetCurrentNode().dialogueText));
            }
        }
    }

    IEnumerator TypeText(string text)
    {
        DialogueNode node = dialogueChain.GetCurrentNode();

        if (node != null)
            node.onDialogueStart.Invoke();

        if(node.waitForFocusBeforeDialogue)
            yield return StartCoroutine(FocusCameraToFocusTarget());
        else 
            StartCoroutine(FocusCameraToFocusTarget());

        CinemachinePositionComposer posComposer = cinemachineCamera.GetComponent<CinemachinePositionComposer>();
        posComposer.Damping = Vector3.one * node.focusTime;

        foreach (char letter in text.ToCharArray())
        {
            textDisplay.text += letter;
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
        if (node.dialogueFocus != null)
        {
            cinemachineCamera.Target.TrackingTarget = node.dialogueFocus.transform;
            yield return new WaitUntil(() => cameraReachedPosTrigger);
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
