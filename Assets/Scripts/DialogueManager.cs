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
    public GameObject characterAnimatorObject;
    public AudioSource audioSource; 
    public AudioClip typingSound;   

    private Queue<DialogueLine> dialogueLines;
    private System.Action onDialogueComplete;

    private void Awake()
    {
        dialogueLines = new Queue<DialogueLine>();

        if (characterAnimatorObject != null)
        {
            characterAnimatorObject.SetActive(true);
        }

        DisableAllCharacters();
    }

    public void StartDialogue(Dialogue dialogue, System.Action onComplete = null)
    {
        dialogueBox.SetActive(true);
        Time.timeScale = 1f;
        DisableAllCharacters();

        dialogueLines.Clear();
        foreach (DialogueLine line in dialogue.lines)
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

        DialogueLine currentLine = dialogueLines.Dequeue();
        nameText.text = currentLine.characterName;
        dialogueText.text = "";

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (audioSource != null && typingSound != null)
        {
            audioSource.clip = typingSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        
        DisableAllCharacters();

        if (currentLine.characterAnimation != null)
        {
            bool characterFound = false;

            foreach (Transform child in characterAnimatorObject.transform)
            {
                Animator animator = child.GetComponent<Animator>();

                if (animator != null)
                {
                   
                    if (animator.runtimeAnimatorController == currentLine.characterAnimation)
                    {
                        child.gameObject.SetActive(true); 
                        animator.runtimeAnimatorController = currentLine.characterAnimation; 
                        StartCoroutine(PlayAnimationWithDelay(animator, "Idle")); 
                        characterFound = true;
                        break;
                    }
                }
            }

            if (!characterFound)
            {
                Debug.LogWarning("No matching character found for animation: " + currentLine.characterAnimation.name);
            }
        }

        StopAllCoroutines();
        StartCoroutine(TypeLine(currentLine.text));
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }


    private IEnumerator PlayAnimationWithDelay(Animator animator, string animationName)
    {
        yield return new WaitForEndOfFrame();
        animator.Play(animationName);
    }

    private void EndDialogue()
    {
        dialogueBox.SetActive(false);
        DisableAllCharacters();
        Time.timeScale = 1f;

        StopAllCoroutines();

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        onDialogueComplete?.Invoke();
    }

    private void DisableAllCharacters()
    {
        if (characterAnimatorObject != null)
        {
            foreach (Transform child in characterAnimatorObject.transform)
            {
                child.gameObject.SetActive(false);
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
