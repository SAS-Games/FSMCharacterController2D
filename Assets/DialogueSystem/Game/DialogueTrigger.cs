using SAS.Utilities.TagSystem;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{
    [Inject] private IDialogueHandler _dialogueHandler;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private void Awake()
    {
        this.Initialize();
    }

    private void ShowDialogue()
    {
        _dialogueHandler.EnterDialogueMode(inkJSON, null);
    }
}