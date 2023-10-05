using UnityEngine;

[CreateAssetMenu(fileName = "QuestParams", menuName = "ScriptableObjects/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    [Multiline(2)] public string description;
    public int reward;
    public int questNumber;

}
