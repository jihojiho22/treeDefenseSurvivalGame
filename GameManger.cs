using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
     public static GameManager instance;

    [System.Serializable]
    public class CoinData
    {
        public int copper;
        public int silver;
        public int gold;
        public int mineral;
    }

    public CoinData coinData = new CoinData();
    private GameObject coinPanel;
    private TextMeshProUGUI coinCopperText;
    private TextMeshProUGUI coinSilverText;
    private TextMeshProUGUI coinGoldText;
    private TextMeshProUGUI mineralText;
    public GameObject gameOverPanel;
    public GameObject upgradePanel;
    public GameObject profilePanel;
    public GameObject settingPanel;
    public GameObject mineralPanel;
    public GameObject pickaxe;

    public bool isGameOver = false;
    public static int SelectedCharacterIndex { get; set; }

    public GameObject playerCharacter;
    public Vector3 defaultSpawnPosition = Vector3.zero;
    public static bool isPaused = false;
    public Weapon weapon;
    private string previousSceneName;

    void Awake()
{
    if (instance == null)
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        previousSceneName = SceneManager.GetActiveScene().name;
    }
    else if (instance != this)
    {
        Destroy(gameObject);
    }
}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    isPaused = false;
    SetupUI();
    if (playerCharacter != null)
    {
        Vector3 spawnPosition = GetSpawnPosition(scene.name, previousSceneName);
        playerCharacter.transform.position = spawnPosition;
    }
    previousSceneName = scene.name;
}
    public GameObject SpawnOrGetCharacter(GameObject characterPrefab, Vector3 spawnPosition)
    {
        if (playerCharacter == null)
        {
            playerCharacter = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
            DontDestroyOnLoad(playerCharacter);
        }
        else
        {
            playerCharacter.transform.position = spawnPosition;
        }
        return playerCharacter;
    }

    private Vector3 GetSpawnPosition(string currentSceneName, string previousSceneName)
{
    switch (currentSceneName)
    {
        case "GameStartScene":
            return defaultSpawnPosition;
        case "Map0":
            return new Vector3(7, -1, 0);
        case "Map1":
            if (previousSceneName == "Map0")
                return new Vector3(-7, 0, 0);
            else if (previousSceneName == "Map2")
                return new Vector3(7, 0, 0);
            break;
        case "Map2": 
            if (previousSceneName == "GameStartScene")
                return new Vector3(-7, 0, 0);
            else if (previousSceneName == "Map3")
                return new Vector3(7, 0, 0);
            else if (previousSceneName == "SlimeCave")
                return new Vector3(3, -2, 0);
                break;
        case "Map3":
            if (previousSceneName == "Map2")
                return new Vector3(-7, 0, 0);
                break;
        case "SlimeCave":
            if (previousSceneName == "Map2")
            return new Vector3(-7, 0, 0);
            break;
    }
    return defaultSpawnPosition;
}
    public GameObject FindWeaponByTag(string tagName)
{
    GameObject weapon = GameObject.FindGameObjectWithTag(tagName);
    if (weapon != null)
    {
        this.weapon = weapon.GetComponent<Weapon>();
        return weapon;
    }
    Debug.LogWarning("Weapon with tag " + tagName + " not found.");
    return null;
}

    void SetupUI()
    {
        coinPanel = GameObject.Find("CoinPanel");
        
        if (coinPanel != null)
        {
            coinCopperText = coinPanel.transform.Find("CopperText")?.GetComponent<TextMeshProUGUI>();
            coinSilverText = coinPanel.transform.Find("SilverText")?.GetComponent<TextMeshProUGUI>();
            coinGoldText = coinPanel.transform.Find("GoldText")?.GetComponent<TextMeshProUGUI>();
            mineralText = mineralPanel.transform.Find("mineralText")?.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            coinPanel = new GameObject("CoinPanel");
            coinCopperText = CreateTextElement(coinPanel, "CopperText");
            coinSilverText = CreateTextElement(coinPanel, "SilverText");
            coinGoldText = CreateTextElement(coinPanel, "GoldText");
            mineralText = CreateTextElement(mineralPanel, "mineralText");
        }

         UpdateCoinUI();
         FindWeaponByTag("Weapon");
    }

    private TextMeshProUGUI CreateTextElement(GameObject parent, string name)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform, false);
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.alignment = TextAlignmentOptions.Center;
        return text;
    }

    public void UpdateCoinUI()
    {
        if (coinCopperText != null) coinCopperText.text = coinData.copper.ToString();
        if (coinSilverText != null) coinSilverText.text = coinData.silver.ToString();
        if (coinGoldText != null) coinGoldText.text = coinData.gold.ToString();
        if (mineralText != null) mineralText.text = coinData.mineral.ToString();
    }

    public void SetGameOver()
{
    isGameOver = true;
    isPaused = true;
    Invoke("ShowGameOverPanel", 1f);
    UpgradeManager.Instance.setDamageToDefault();
}

  

    void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
    public void ShowPickaxe(Vector3 position)
{
    if (pickaxe != null)
    {
        Vector3 offset = new Vector3(0.5f, 0.5f, 0); 
        pickaxe.SetActive(true);
        pickaxe.transform.position = position + offset;
        Debug.Log("Pickaxe shown at position: " + (position + offset));
    }
    else
    {
        Debug.LogError("Pickaxe is null in GameManager");
    }
}

    public void HidePickaxe()
    {
        if (pickaxe != null)
        {
            pickaxe.SetActive(false);
        }
    }

   public void ShowMineralPanel()
    {
        if (mineralPanel != null)
        {
            mineralPanel.SetActive(true);

        }
    }

    public void ShowProfilePanel()
    {
        if (profilePanel != null)
        {
            profilePanel.SetActive(true);
            isPaused = true;
        }
    }

    public void CloseProfilePanel()
    {
        
            profilePanel.SetActive(false);
            isPaused = false;
        
    }
     public void ShowSettingPanel()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(true);
            isPaused = true;
        }
    }

    public void CloseSettingPanel()
    {
        
            settingPanel.SetActive(false);
            isPaused = false;
        
    }
    public void ShowUpgradePanel()
    {
        if (gameOverPanel != null)
        {
            upgradePanel.SetActive(true);
        }
    }

    public void CloseUpgradePanel()
    {
        if (gameOverPanel != null)
        {
            upgradePanel.SetActive(false);
            isPaused = false;
        }
    }

    public void increaseCopperCoin()
    {
        coinData.copper++;
        UpdateCoinUI();
    }

    public void increaseSilverCoin()
    {
        coinData.silver++;
        UpdateCoinUI();
    }

    public void increaseGoldCoin()
    {
        coinData.gold++;
        UpdateCoinUI();
    }
    public void increaseMineral()
    {
        coinData.mineral++;
        UpdateCoinUI();
    }
    
}