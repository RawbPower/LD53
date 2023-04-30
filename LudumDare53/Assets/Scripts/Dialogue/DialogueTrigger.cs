using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<Dialogue> dialogue;

    private void Start()
    {
        TriggerDialogue("Test");
    }

    public void TriggerDialogue (string dialogueKey)
    {
        DialogueManager manager = FindObjectOfType<DialogueManager>();
        if (manager)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue.Find(x => x.key == dialogueKey));
        }
    }
}
