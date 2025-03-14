using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName; 
    [TextArea(3, 10)]
    public string text;
    public RuntimeAnimatorController characterAnimation; 
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] lines; 
}
