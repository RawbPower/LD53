using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;

    public float timeBetweenLetter;
    public float timeBetweenSentence;

    public event System.Action OnStartDialogue;
    public event System.Action OnEndDialogue;

    private bool isTalking;

    private Queue<string> sentences;
    private string speakerName;
    private string sentencePrefix;

    private float initialTimeBetweenLetter;
    private Color currentColor;

    // Start is called before the first frame update
    void Awake()
    {
        sentences = new Queue<string>();
        isTalking = false;
        initialTimeBetweenLetter = timeBetweenLetter;
        currentColor = dialogueText.color;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isTalking = true;

        sentences.Clear();

        speakerName = dialogue.name;

        OnStartDialogue?.Invoke();

        StartCoroutine(PlayMessage(dialogue.message));
    }

    IEnumerator PlayMessage(string message)
    {
        sentencePrefix = "<" + speakerName + "> ";
        dialogueText.text = sentencePrefix;
        int currentIndex = 0;
        while (message.Length > currentIndex)
        {
            char letter = message[currentIndex];
            string dialogueCode = GetDialogueCode(message, currentIndex);
            if (!string.IsNullOrEmpty(dialogueCode))
            {
                currentIndex += dialogueCode.Length;
                yield return ActivateCode(dialogueCode);
            }
            else
            {
                dialogueText.text += letter;
                currentIndex++;
                yield return new WaitForSeconds(timeBetweenLetter);
            }
        }
    }

    static string GetDialogueCode(string message, int currentIndex)
    {
        string code = "";
        string currentMessage = message.Remove(0, currentIndex);
        if (currentMessage.Length > 0 && currentMessage[0] == '[')
        {
            if (currentMessage.IndexOf(']') + 1 < currentMessage.Length)
            {
                code = currentMessage.Remove(currentMessage.IndexOf(']') + 1);
            }
            else
            {
                code = currentMessage;
            }
        }

        return code;

    }

    private IEnumerator ActivateCode(string code)
    {
        if (code.Equals("[n]"))
        {
            dialogueText.text += '\n';
            yield return new WaitForSeconds(timeBetweenSentence);
        }
        else if (code.Equals("[np]"))
        {
            dialogueText.text += '\n';
            dialogueText.text += sentencePrefix;
            yield return new WaitForSeconds(timeBetweenSentence);
        }
        else if (code.StartsWith("[t"))
        {
            if (code.Equals("[t]"))
            {
                timeBetweenLetter = initialTimeBetweenLetter;
            }
            else
            {
                string tblCode = code.Substring(3, code.Length - 4);
                timeBetweenLetter = float.Parse(tblCode);
            }
        }
        else if (code.StartsWith("[c"))
        {
            currentColor = GetColor(code);
            dialogueText.color = currentColor;
        }
    }

    static Color GetColor(string code)
    {
        string colCode = code.Substring(3, code.Length - 4);
        switch (colCode)
        {
            case "black":
                return Color.black;
            case "white":
                return Color.white;
            case "red":
                return Color.red;
            case "blue":
                return Color.blue;
            case "green":
                return Color.green;
            case "yellow":
                return Color.yellow;
            default:
                return Color.black;
        } 
    }

    void EndDialogue()
    {
        isTalking = false;
        OnEndDialogue?.Invoke();
 
    }
}
