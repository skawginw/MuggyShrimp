using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string characterName;
    [TextArea(3, 10)]
    public string[] lines;
    public RuntimeAnimatorController characterAnimation; 
}
