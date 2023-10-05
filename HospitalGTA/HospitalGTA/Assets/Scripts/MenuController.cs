using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    [SerializeField] private int[] carPrices;
    private int carNumber;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button buyCarButton;
    [SerializeField] private TextMeshProUGUI buyCar_txt;
    [SerializeField] private TextMeshProUGUI money_txt;
    [SerializeField] private GameObject[] specs;


    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    void Start()
    {
        Init.Instance.playerData.car_Number = 0;
        Init.Instance.playerData.boughtCars.Add(0);
        carNumber = 0;
    }

    private void OnEnable()
    {
        rightButton.onClick.AddListener(SwitchCarRight);
        leftButton.onClick.AddListener(SwitchCarLeft);
        buyCarButton.onClick.AddListener(BuyCar);
        money_txt.text = Init.Instance.playerData.money.ToString();
    }

    private void OnDisable()
    {
        rightButton.onClick.RemoveAllListeners();
        leftButton.onClick.RemoveAllListeners();
    }

    public void SwitchCarRight()
    {
        buyCarButton.onClick.RemoveAllListeners();
        buyCarButton.onClick.AddListener(BuyCar);
        //

        cars[carNumber].SetActive(false);
        specs[carNumber].SetActive(false);

        if (carNumber == cars.Length - 1)
        {
            carNumber = 0;
        }
        else
        {
            carNumber += 1;
        }

        cars[carNumber].SetActive(true);
        specs[carNumber].SetActive(true);

        //

        foreach (var item in Init.Instance.playerData.boughtCars)
        {
            if (item == carNumber)
            {
                if (carNumber == Init.Instance.playerData.car_Number)
                {
                    buyCar_txt.text = "Играть";
                }
                else
                {
                    buyCar_txt.text = "Выбрать";
                }
                return;
            }
        }

        buyCar_txt.text = carPrices[carNumber].ToString();
    }

    public void SwitchCarLeft()
    {
        buyCarButton.onClick.RemoveAllListeners();
        buyCarButton.onClick.AddListener(BuyCar);
        //

        cars[carNumber].SetActive(false);
        specs[carNumber].SetActive(false);

        if (carNumber == 0)
        {
            carNumber = cars.Length - 1;
        }
        else
        {
            carNumber -= 1;
        }

        cars[carNumber].SetActive(true);
        specs[carNumber].SetActive(true);

        //

        foreach (var item in Init.Instance.playerData.boughtCars)
        {
            if (item == carNumber)
            {
                if (carNumber == Init.Instance.playerData.car_Number)
                {
                    buyCar_txt.text = "Играть";
                }
                else
                {
                    buyCar_txt.text = "Выбрать";
                }
                return;
            }
        }

        buyCar_txt.text = carPrices[carNumber].ToString();

    }

    public void BuyCar()
    {
        if (carNumber == 0 && Init.Instance.playerData.car_Number == 0)
        {
            StartGame();
            return;
        }
        
        foreach (var item in Init.Instance.playerData.boughtCars)
        {
            if (item == carNumber)
            {
                if (carNumber == Init.Instance.playerData.car_Number)
                {
                    buyCar_txt.text = "Играть";
                    buyCarButton.onClick.RemoveAllListeners();
                    buyCarButton.onClick.AddListener(StartGame);
                }
                else
                {
                    Init.Instance.playerData.car_Number = carNumber;
                    buyCar_txt.text = "Играть";
                }
                return;
            }
        }

        if (Init.Instance.playerData.money >= carPrices[carNumber])
        {
            Init.Instance.playerData.money -= carPrices[carNumber];
            money_txt.text = Init.Instance.playerData.money.ToString();
            buyCar_txt.text = "Играть";
            Init.Instance.playerData.boughtCars.Add(carNumber);
            Init.Instance.playerData.car_Number = carNumber;
            buyCarButton.onClick.RemoveAllListeners();
            buyCarButton.onClick.AddListener(StartGame);
        }
        
        
    }
}
