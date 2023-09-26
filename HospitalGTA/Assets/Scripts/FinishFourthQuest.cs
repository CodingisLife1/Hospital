using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishFourthQuest : MonoBehaviour
{
    public QuestTrigger quest;

    private void OnTriggerEnter(Collider other)
    {
        quest.CompleteMission();
        quest.car.transform.position = QuestManager.instance.startPoint.position;

        Destroy(gameObject);
    }
}
