using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    [System.Serializable]
    public class DialogueCharacterPair
    {
        public Dialogue dialogue;
        public GameObject characterObject;
    }

    public List<DialogueCharacterPair> dialogueCharacterPairs;

    private Queue<string> dialogueLines;
    private System.Action onDialogueComplete;
    private GameObject currentCharacterObject = null;
    private void Awake()
    {
        dialogueLines = new Queue<string>();
        DisableAllCharacters(); 
    }

    public void StartDialogue(Dialogue dialogue, System.Action onComplete = null)
    {
        dialogueBox.SetActive(true);

        DisableAllCharacters();

        foreach (DialogueCharacterPair pair in dialogueCharacterPairs)
        {
            if (pair.dialogue == dialogue)
            {
                currentCharacterObject = pair.characterObject;
                if (currentCharacterObject != null)
                {
                    currentCharacterObject.SetActive(true);
                    Animator animator = currentCharacterObject.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.Play("Idle"); 
                    }
                }
                break;
            }
        }

        nameText.text = dialogue.characterName;
        dialogueLines.Clear();

        foreach (string line in dialogue.lines)
        {
            dialogueLines.Enqueue(line);
        }

        onDialogueComplete = onComplete;
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        string line = dialogueLines.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void EndDialogue()
    {
        dialogueBox.SetActive(false);
        DisableAllCharacters();
        onDialogueComplete?.Invoke();
    }

    private void DisableAllCharacters()
    {
        foreach (DialogueCharacterPair pair in dialogueCharacterPairs)
        {
            if (pair.characterObject != null)
            {
                pair.characterObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (dialogueBox.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (dialogueLines.Count == 0)
            {
                EndDialogue();
            }
            else
            {
                DisplayNextLine();
            }
        }
    }
}
