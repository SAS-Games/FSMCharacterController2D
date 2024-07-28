using SAS.Utilities.TagSystem;
using UnityEngine;
using System.Collections;


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
        if (!PlayerPrefs.HasKey(inkJSON.name))
        {
            PlayerPrefs.SetInt(inkJSON.name, 1);
            _dialogueHandler.EnterDialogueMode(inkJSON, null);
        }
    }
}