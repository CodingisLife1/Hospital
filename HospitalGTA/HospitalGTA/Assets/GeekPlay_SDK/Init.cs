using System;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngine;

public enum Platform 
{
    Editor,
    Yandex, 
    VK,
    GameArter
}

public class Init : MonoBehaviour
{
    public bool soundOn = true; //Звук включен?
    public PlayerData playerData;

    [Header("Publisher Settings")]
    [Space(50)]
    public Platform platform; //Платформа
    [SerializeField] private GameObject gameArterPrefab; //Префаб площадки GameArter
    public GameObject leaderboardBtn; //КНОПКА, ОТКРЫВАЮЩАЯ ЛИДЕРБОРД
    [SerializeField] private string colorDebug; //Цвет Дебага

    [Header("Not touch")]
    public static Init Instance;
    string rewardTag; //Тэг награды
    private bool adOpen; //Реклама открыта?
    string purchasedTag; //Тэг покупки
    public bool wasLoad; //Игра загружалась?
    bool canShowAd = true; //Можно ли проигрывать рекламу на вк?
    string developerNameYandex = "GeeKid%20-%20школа%20программирования";

    [Header("Localization")]
    public string language;

    [Header("Mobile")]
    public bool mobile; //Устройство игрока мобильное?


    //ВЫ МЕНЯЕТЕ
    public void OnRewarded() //ВОЗНАГРАЖДЕНИЕ ПОСЛЕ ПРОСМОТРА РЕКЛАМЫ
    {
        if (rewardTag == "Coins")
        {
        	playerData.money += 100; //ПРИСУЖДАЕМ НАГРАДУ
        }
        Debug.Log($"<color=yellow>REWARD:</color> {rewardTag}");
        Save();
        StartCoroutine(RewardLoad());
    }



    //ВЫ ВЫЗЫВАЕТЕ
    public void ShowInterstitialAd() //МЕЖСТРАНИЧНАЯ РЕКЛАМА - ПОКАЗАТЬ
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>INTERSTITIAL SHOW</color>");
                break;
            case Platform.Yandex:
                Utils.AdInterstitial();
                break;
            case Platform.VK:
                if (canShowAd)
                {
                    canShowAd = false;
                    StartCoroutine(CanAdShow());
                    Utils.VK_Interstitial();
                }
                break;
        }
    }

    public void ShowRewardedAd(string idOrTag) //РЕКЛАМА С ВОЗНАГРАЖДЕНИЕМ - ПОКАЗАТЬ
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>REWARD SHOW</color>");
                rewardTag = idOrTag;
                OnRewarded();
                break;
            case Platform.Yandex:
                rewardTag = idOrTag;
                Utils.AdReward();
                break;
            case Platform.VK:
                    canShowAd = false;
                    StartCoroutine(CanAdShow());
                    rewardTag = idOrTag;
                    Utils.VK_Rewarded();
                break;
        }
    }

    public void OpenOtherGames() //ССЫЛКА НА ДРУГИЕ ИГРЫ
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>OPEN OTHER GAMES</color>");
                break;
            case Platform.Yandex:
                var domain = Utils.GetDomain();
                Application.OpenURL($"https://yandex.{domain}/games/developer?name=" + developerNameYandex);
                break;
            case Platform.VK:
            	rewardTag = "Group";
                Utils.VK_ToGroup();
                break;
        }
    }

    public void RateGameFunc() //ПРОСЬБА ОЦЕНИТЬ ИГРУ
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>REWIEV GAME</color>");
                break;
            case Platform.Yandex:
                Utils.RateGame();
                break;
        }
    }

    public void Save() //СОХРАНЕНИЕ
    {
        string jsonString = "";

        switch (platform)
        {
            case Platform.Editor:
            	jsonString = JsonUtility.ToJson(playerData);
            	PlayerPrefs.SetString("PlayerData", jsonString);
                Debug.Log($"<color={colorDebug}>SAVE</color>");
                break;
            case Platform.Yandex:
                if (wasLoad)
                {   
                    jsonString = JsonUtility.ToJson(playerData);
                    Utils.SaveExtern(jsonString);
                    Debug.Log("Save");
                }
                break;
            case Platform.VK:
                if (wasLoad)
                {   
                    jsonString = JsonUtility.ToJson(playerData);
                    Utils.VK_Save(jsonString);
                }
                break;
        }
    }

    public void Leaderboard(string leaderboardName, int value) //ЗАНЕСТИ В ЛИДЕРБОРД
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>SET LEADERBOARD:</color> {value}");
                break;
            case Platform.Yandex:
                Utils.SetToLeaderboard(value, leaderboardName);
                break;
            case Platform.VK:
                if (mobile)
                    Utils.VK_OpenLeaderboard(value);
                break;
        }
    }

    public void ToStarGame() //ДОБАВИТЬ В ИЗБРАННОЕ (ВК)
    {
    	switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>GAME TO STAR</color>");
                break;
            case Platform.Yandex:
                break;
            case Platform.VK:
                Utils.VK_Star();
                break;
        }
    }

    public void ShareGame() //ПОДЕЛИТЬСЯ ИГРОЙ (ВК)
    {
    	switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>SHARE</color>");
                break;
            case Platform.Yandex:
                break;
            case Platform.VK:
                Utils.VK_Share();
                break;
        }
    }

    public void InvitePlayers() //ПРИГЛАСИТЬ ИГРОКОВ (ВК)
    {
    	switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>INVITE</color>");
                break;
            case Platform.Yandex:
                break;
            case Platform.VK:
                Utils.VK_Invite();
                break;
        }
    }



    //ДЛЯ РАБОТЫ - ТРОГАТЬ НЕ НАДО
	protected void Awake()
    {
    	//Синглтон
    	if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //Game Arter
        if (platform != Platform.GameArter)
        {
            Destroy(gameArterPrefab);
        }
        else
        {
            gameArterPrefab.SetActive(true);
        }

        //Старт площадок
        switch (platform)
        {
            case Platform.Editor:
                ShowBannerAd();
                StartCoroutine(BannerVK());
                if (PlayerPrefs.HasKey("PlayerData"))
                {
                	string s = PlayerPrefs.GetString("PlayerData");
                	playerData = JsonUtility.FromJson<PlayerData>(s);
                }
                language = "tr"; //ВЫБРАТЬ ЯЗЫК ДЛЯ ТЕСТОВ. ru/en/tr\
                Localization();
                break;
            case Platform.Yandex:
                language = Utils.GetLang();
                if (wasLoad)
                    Utils.LoadExtern();
                Localization();
                ShowInterstitialAd();
                break;
            case Platform.VK:
                language = "ru";
                Localization();
                StartCoroutine(BannerVK());
                StartCoroutine(RewardLoad());
                StartCoroutine(InterLoad());
                if (wasLoad)
                    Utils.VK_Load();
                break;
        }
    }

    public void ItIsMobile()
    {
        mobile = true;
        leaderboardBtn.SetActive(true);
    }

    IEnumerator RewardLoad()
    {
    	yield return new WaitForSeconds(15);
    	switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>REWARD LOAD</color>");
                break;
            case Platform.VK:
                Utils.VK_AdRewardCheck();
                break;
        }
    }

    IEnumerator InterLoad()
    {
    	while (true)
    	{	
    		yield return new WaitForSeconds(15);
	    	switch (platform)
	        {
	            case Platform.Editor:
	                Debug.Log($"<color={colorDebug}>INTERSTITIAL LOAD</color>");
	                break;
	            case Platform.VK:
	                Utils.VK_AdInterCheck();
	                break;
	        }
    	}
    }


    IEnumerator BannerVK()
    {
    	yield return new WaitForSeconds(5);
    	ShowBannerAd();
    }

    public void ShowBannerAd() 
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>BANNER SHOW</color>");
                break;
            case Platform.VK:
                Utils.VK_Banner();
                break;
        }
    }

    public void StopMusAndGame()
    {
        adOpen = true;
        canShowAd = false;
        StartCoroutine(CanAdShow());
        AudioListener.volume = 0;
        Time.timeScale = 0;
    }

    public void ResumeMusAndGame()
    {
        adOpen = false;
        AudioListener.volume = 1;
        Time.timeScale = 1;
        if (!soundOn)
            AudioListener.volume = 0;
    }

    public void Localization()
    {

    }

    public void SetPlayerData(string value)
    {
        playerData = JsonUtility.FromJson<PlayerData>(value);
    }

    public void Load()
    {
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>LOAD</color>");
                break;
            case Platform.Yandex:
                if (wasLoad)
                {   
                    Debug.Log("LOAD");
                    Utils.LoadExtern();
                }
                break;
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        Silence(!hasFocus);
    }

    void OnApplicationPause(bool isPaused)
    {
        Silence(isPaused);
    }

    private void Silence(bool silence)
    {
        AudioListener.volume = silence ? 0 : 1;
        Time.timeScale = silence ? 0 : 1;

        if (adOpen)
        {
            Time.timeScale = 0;
            AudioListener.volume = 0;
        }
        if (!soundOn)
        {
            AudioListener.volume = 0;
        }
    }
    IEnumerator CanAdShow()
    {
        yield return new WaitForSeconds(60);
        canShowAd = true;
    }



    

    //В ДОРАБОТКЕ
    public void RealBuyItem(string idOrTag) //открыть окно покупки
    {
        switch (platform)
        {
            case Platform.Editor:
                purchasedTag = idOrTag;
                OnPurchasedItem();
                Debug.Log($"<color={colorDebug}>PURCHASE: </color> {purchasedTag}");
                break;
            case Platform.Yandex:
                purchasedTag = idOrTag;
                Utils.BuyItem(idOrTag);
                break;
            case Platform.VK:
                purchasedTag = idOrTag;
                Debug.Log($"<color={colorDebug}>PURCHASE VK</color>");
                break;
        }
    }

    public void CheckBuysOnStart(string idOrTag) //проверить покупки на старте
    {
        Utils.CheckBuyItem(idOrTag);
    }

    private void OnPurchasedItem() //начислить покупку (при удачной оплате)
    {
        if (purchasedTag == "purchasedID")
        {
            
        }
    }

    public void SetPurchasedItem() //начислить уже купленные предметы на старте
    {
        if (purchasedTag == "purchasedID")
        {
            
        }
    }

    public void LeaderboardBtn(int value) //Для кнопки лидерборда в VK
    {
    	//value = playerData.Level;
        switch (platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>SET LEADERBOARD:</color> {value}");
                break;
            case Platform.Yandex:
                break;
            case Platform.VK:
                Utils.VK_OpenLeaderboard(value);
                break;
        }
    }
}
