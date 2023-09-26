using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        QuestManager.instance.hospital.SetActive(true);

        Destroy(gameObject);
    }
}
