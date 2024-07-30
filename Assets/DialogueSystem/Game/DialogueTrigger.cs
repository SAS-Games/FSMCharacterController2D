using SAS.Utilities.TagSystem;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{
    [Inject] private IDialogueHandler _dialogueHandler;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    private bool _triggerOncePerSession = true;

    private bool _triggered = false;

    private void Awake()
    {
        this.Initialize();
    }

    private void ShowDialogue()
    {
        if (_triggerOncePerSession && !_triggered)
        {
            _triggered = true;
            _dialogueHandler.EnterDialogueMode(inkJSON, null);
        }
    }
}