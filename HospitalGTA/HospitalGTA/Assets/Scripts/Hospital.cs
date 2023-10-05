using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hospital : MonoBehaviour
{
    public QuestTrigger quest;

    private void OnTriggerEnter(Collider other)
    {
        if (quest.time >= 0)
        {
            quest.startTimer = false;
            QuestManager.instance.timer_txt.gameObject.SetActive(false);
            gameObject.SetActive(false);
            quest.CompleteMission();
        }
    }
}
