using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{
    public QuestTrigger quest;

    private void OnTriggerEnter(Collider other)
    {
        quest.medKitsCount += 1;
        quest.existMedkits.Remove(gameObject.transform);

        if (quest.medKitsCount == 6)
        {
            quest.medKitsCount = 0;
            quest.CompleteMission();
        }

        Destroy(gameObject);
    }
}
