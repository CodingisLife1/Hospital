using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class QuestTrigger : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Quest quest;
    public float time;
    public bool startTimer;
    private bool endTimer = true;
    private float preTime = 3f;
    private bool startPreTimer;
    public VehicleControl car;
    private Rigidbody carRB;
    private LineRenderer line;
    private bool secondQuest;
    private bool thirdQuest;
    private bool firstQuest;
    private bool fourthQuest;
    private float minCorners = 99999999;
    private NavMeshPath path;
    private NavMeshPath path1;
    public GameObject triggers;
    [SerializeField] private Transform icon;
    [SerializeField] private GameObject vfx;
    [SerializeField] private BoxCollider bc;
 
    [Header("First Quest")]
    public GameObject medKit;
    private List<Transform> medKitSpawnPoints;
    public int medKitsCount = 0;
    private bool[] emptyMedKitSpawnPoints;
    public Transform targetMedkit;
    public List<Transform> existMedkits = new List<Transform>();

    [Header("Second Quest")]
    [SerializeField] private GameObject finishPointPref;
    private List<Transform> finishSpawnPoints;
    private GameObject finishPoint;


    [Header("Third Quest")]
    [SerializeField] private GameObject humanPref;
    [SerializeField] private List<Transform> humanSpawnPoints;
    private GameObject human;

    [Header("Fourth Quest")]
    [SerializeField] private GameObject finishPref;    
    private GameObject finish;

    [Header("Fith Quest")]
    [SerializeField] private GameObject finishPref5Quest;    
    private GameObject finish5Quest;



    private void OnEnable()
    {
        QuestManager.instance.claimRew_button.onClick.RemoveAllListeners();
        QuestManager.instance.claimRew_button.onClick.AddListener(ClaimReward);
        car = GameObject.FindGameObjectWithTag("Player").GetComponent<VehicleControl>();
        carRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (vfx)
        {
            icon.LookAt(QuestManager.instance.cam);
        }

        if (firstQuest)
        {
            FindClosestObject();
            NavMesh.CalculatePath(car.transform.position, targetMedkit.position, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
        }

        if (secondQuest)
        {
            NavMesh.CalculatePath(car.transform.position, finishPoint.transform.position, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            
        }
        else if (thirdQuest)
        {
            if (human != null)
            {
                NavMesh.CalculatePath(car.transform.position, human.transform.position, NavMesh.AllAreas, path);
                line.positionCount = path.corners.Length;
                line.SetPositions(path.corners);
            }
            else
            {
                NavMesh.CalculatePath(car.transform.position, QuestManager.instance.hospital.transform.position, NavMesh.AllAreas, path1);
                Debug.Log(path1.corners.Length);
                line.positionCount = path1.corners.Length;
                line.SetPositions(path1.corners);
            }
            
        }
             

    }

    private void OnTriggerEnter(Collider other)
    {
        car.speed = 0;
        car.lastSpeed = -10;
        carRB.velocity = new Vector3(-5, -5, -5);
        car.activeControl = false;
        QuestManager.instance.start_button.onClick.RemoveAllListeners();
        QuestManager.instance.cancel_button.onClick.RemoveAllListeners();

        QuestManager.instance.name_txt.text = quest.questName;
        QuestManager.instance.description_txt.text = quest.description;
        QuestManager.instance.reward_txt.text = quest.reward.ToString();
        QuestManager.instance.questPanel.SetActive(true);

        QuestManager.instance.start_button.onClick.AddListener(StartQuest);
        QuestManager.instance.start_button.onClick.AddListener(() => this.gameObject.transform.parent = null);
        QuestManager.instance.start_button.onClick.AddListener(() => QuestManager.instance.triggers.SetActive(false));
        QuestManager.instance.start_button.onClick.AddListener(() => QuestManager.instance.questPanel.SetActive(false));

        QuestManager.instance.cancel_button.onClick.AddListener(() => QuestManager.instance.questPanel.SetActive(false));
        QuestManager.instance.cancel_button.onClick.AddListener(() => car.activeControl = true);


        path = new NavMeshPath();
        path1 = new NavMeshPath();
        
        line = car.GetComponent<LineRenderer>();
    }

    void StartQuest()
    {
        car.activeControl = true;
        Destroy(vfx);
        Destroy(bc);
        QuestManager.instance.home_button.gameObject.SetActive(false);
        switch (quest.questNumber)
        {
            case 1:
                medKitSpawnPoints = new List<Transform>();
                medKitsCount = 0;
                foreach (Transform child in QuestManager.instance.medKitSpawnPoints.transform)
                {
                    medKitSpawnPoints.Add(child);
                }

                emptyMedKitSpawnPoints = new bool[medKitSpawnPoints.Count];

                existMedkits.Clear();

                for (int i = 0; i < 6; i++)
                {
                    int r = Random.Range(0, medKitSpawnPoints.Count);
                    while (emptyMedKitSpawnPoints[r] != false)
                    {
                        r = Random.Range(0, medKitSpawnPoints.Count);
                    }
                    GameObject medkit = Instantiate(medKit, medKitSpawnPoints[r].transform.position, Quaternion.identity);
                    existMedkits.Add(medkit.transform);
                    medkit.GetComponent<MedKit>().quest = this;
                    emptyMedKitSpawnPoints[r] = true;
                }

                firstQuest = true;
                break;

            case 2:
                finishSpawnPoints = new List<Transform>();

                foreach  (Transform child in QuestManager.instance.finishPointsSecondQuest.transform)
                {
                    finishSpawnPoints.Add(child);
                }

                finishPoint = Instantiate(finishPointPref, finishSpawnPoints[Random.Range(0, finishSpawnPoints.Count)].transform.position, Quaternion.identity);
                finishPoint.GetComponent<FinishPoint>().quest = this;

                secondQuest = true;
                startPreTimer = true;
                QuestManager.instance.preTimer_txt.gameObject.SetActive(true);
                car.activeControl = false;
                preTime = 4f;
                time = 45f;
                Debug.Log(quest.questNumber);
                StartCoroutine(Timer());
                break;

            case 3:
                humanSpawnPoints = new List<Transform>();

                foreach (Transform child in QuestManager.instance.humanPoints.transform)
                {
                    humanSpawnPoints.Add(child);
                }

                human = Instantiate(humanPref, humanSpawnPoints[Random.Range(0, humanSpawnPoints.Count)].transform.position, Quaternion.identity);
                QuestManager.instance.hospital.GetComponent<Hospital>().quest = this;

                thirdQuest = true;
                startPreTimer = true;
                QuestManager.instance.preTimer_txt.gameObject.SetActive(true);
                car.activeControl = false;
                preTime = 4f;
                time = 60f;
                StartCoroutine(Timer());
                break;


            case 4:
                finish = Instantiate(finishPref, QuestManager.instance.fourthFinishSpawnPoint.position, Quaternion.identity);
                finish.GetComponent<FinishFourthQuest>().quest = this;
                car.transform.position = QuestManager.instance.rampPoint.position;
                Debug.Log(QuestManager.instance.rampPoint.position);
                Debug.Log(car.transform.position);
                car.transform.rotation = QuestManager.instance.rampPoint.rotation;
                QuestManager.instance.minimap.SetActive(false);
                startPreTimer = true;
                car.activeControl = false;
                preTime = 4f;
                time = 30f;
                fourthQuest = true;
                StartCoroutine(Timer());
                break;

            case 5:
                finish5Quest = Instantiate(finishPref5Quest, QuestManager.instance.fithFinishSpawnPoint.position, Quaternion.identity);
                finish5Quest.GetComponent<FinishFithQuest>().quest = this;
                car.transform.position = QuestManager.instance.containerPoint.position;
                car.transform.rotation = QuestManager.instance.containerPoint.rotation;
                Debug.Log(QuestManager.instance.containerPoint.position);
                Debug.Log(car.transform.position);
                QuestManager.instance.minimap.SetActive(false);
                startPreTimer = true;
                car.activeControl = false;
                preTime = 4f;
                time = 30f;
                fourthQuest = true; //òàêàÿ æå ïðîâåðêà êàê è â ÷åòâåðòîì êâåñòå
                StartCoroutine(Timer());
                break;

            default:
                break;
        }
    }

    private IEnumerator Timer()
    {
        while (endTimer)
        {

            //Ïîäãîòîâêà ê íà÷àëó ìèññèè
            if (startPreTimer)
            {
                preTime -= 1;
                if (preTime <= 0)
                {
                    QuestManager.instance.preTimer_txt.gameObject.SetActive(false);
                    QuestManager.instance.timer_txt.gameObject.SetActive(true);
                    startPreTimer = false;
                    startTimer = true;
                    car.activeControl = true;
                }
                QuestManager.instance.preTimer_txt.text = preTime.ToString("0");
            }

            //Íà÷àëî ìèññèè
            if (!fourthQuest)
            {
                if (startTimer)
                {
                    time -= 1;
                    if (time <= 0)
                    {
                        startTimer = false;
                        QuestManager.instance.triggers.SetActive(true);
                        gameObject.transform.parent = QuestManager.instance.triggers.transform;
                        if (quest.questNumber == 2)
                        {
                            secondQuest = false;
                            Destroy(finishPoint);
                        }
                        else if (quest.questNumber == 3)
                        {
                            Destroy(human);
                            QuestManager.instance.hospital.SetActive(false);
                            thirdQuest = false;
                        }
                        line.positionCount = 0;
                        QuestManager.instance.timer_txt.gameObject.SetActive(false);
                        endTimer = false;
                        Init.Instance.ShowInterstitialAd();
                    }
                    QuestManager.instance.timer_txt.text = time.ToString("0");
                }
            }

            if (!startPreTimer && fourthQuest)
            {
                endTimer = false;
                QuestManager.instance.timer_txt.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(1);
        }
        
        
    }


    public void CompleteMission()
    {
        foreach (var item in existMedkits)
        {
            Destroy(item);
        }
        existMedkits.Clear();
        QuestManager.instance.winPanel.SetActive(true);
        QuestManager.instance.rewardAmount_txt.text = "Респект +" + QuestManager.instance.reward_txt.text;
        QuestManager.instance.triggers.SetActive(true);
        gameObject.transform.parent = QuestManager.instance.triggers.transform;
        secondQuest = false;
        thirdQuest = false;
        firstQuest = false;
        fourthQuest = false;
        car.activeControl = false;
        QuestManager.instance.minimap.SetActive(true);

        line.positionCount = 0;
        QuestManager.instance.spawnQuest.Spawn();
    }

    private void ClaimReward()
    {
        QuestManager.instance.winPanel.SetActive(false);
        car.activeControl = true;
        Init.Instance.playerData.money += quest.reward;
        QuestManager.instance.home_button.gameObject.SetActive(true);
        Init.Instance.ShowInterstitialAd();
    }

    public void FindClosestObject()
    {
        minCorners = 9999999999;
        foreach (var item in existMedkits)
        {
            if (Vector3.Distance(car.transform.position, item.position) < minCorners)
            {
                minCorners = Vector3.Distance(car.transform.position, item.position);
                targetMedkit = item;
            }
            
        }
    }
}
