using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardFithQuest : MonoBehaviour
{
    [SerializeField] private Quest quest;
    [SerializeField] private int reward;

    private void OnTriggerEnter(Collider other)
    {
        quest.reward = reward;
        QuestManager.instance.reward_txt.text = quest.reward.ToString();
        
    }

}
