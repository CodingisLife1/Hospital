using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public QuestTrigger quest;

    private void OnTriggerEnter(Collider other)
    {
        if (quest.time >= 0)
        {
            quest.startTimer = false;
            QuestManager.instance.timer_txt.gameObject.SetActive(false);
            Destroy(gameObject);
            quest.CompleteMission();
        }
    }
}
