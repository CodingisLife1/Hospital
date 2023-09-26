using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public GameObject questPanel;
    public TextMeshProUGUI name_txt;
    public TextMeshProUGUI description_txt;
    public TextMeshProUGUI reward_txt;
    public TextMeshProUGUI rewardAmount_txt;
    public GameObject medKitSpawnPoints;
    public GameObject triggers;
    public GameObject winPanel;
    public GameObject finishPointsSecondQuest;
    public GameObject humanPoints;
    public TextMeshProUGUI timer_txt;
    public TextMeshProUGUI preTimer_txt;
    public Transform cam;
    public GameObject hospital;
    public SpawnQuest spawnQuest;
    public Transform rampPoint;
    public Transform containerPoint;
    public Transform fourthFinishSpawnPoint;
    public Transform fithFinishSpawnPoint;
    public Transform startPoint;
    public GameObject minimap;

    public static QuestManager instance;

    public Button start_button;
    public Button cancel_button;
    public Button claimRew_button;
    public Button home_button;

    private void Awake()
    {
        instance = this;
    }
}
