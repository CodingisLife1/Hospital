using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Minimap minimap;

    private void OnEnable()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            if (i == Init.Instance.playerData.car_Number)
            {
                cars[i].SetActive(true);
                minimap.player = cars[i].transform;
                vcam.Follow = cars[i].transform;
                vcam.LookAt = cars[i].transform;
                
            }
            else
            {
                Destroy(cars[i]);
            }
        }

        QuestManager.instance.home_button.onClick.RemoveAllListeners();
        QuestManager.instance.home_button.onClick.AddListener(ReturnHome);
    }


    private void ReturnHome()
    {
        cars[Init.Instance.playerData.car_Number].transform.position = QuestManager.instance.startPoint.position;
        cars[Init.Instance.playerData.car_Number].SetActive(false);
        SceneManager.LoadScene("Menu");
        Init.Instance.ShowInterstitialAd();
    }

}
