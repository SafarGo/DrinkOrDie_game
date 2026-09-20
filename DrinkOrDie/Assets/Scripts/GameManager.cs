using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int WaveCount = 1;
    public int PlayerExpCount;
    public int ExpToNewxWave;
    public int PlayerLevel;

    public TMP_Text ExpText;
    public TMP_Text WaveText;
    public TMP_Text HPText;
    public GameObject UpgradePanel;
    public GameObject BuildingPanel;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Update()
    {
        ExpText.text = "Exp: " + PlayerExpCount + "/" + ExpToNewxWave;
        WaveText.text = "Wave: " + WaveCount;
        HPText.text = "HP: " + Mathf.CeilToInt(GameObject.Find("Player").GetComponent<PlayerController>().Hp);
    }

    public void AddExp(int exp)
    {
        PlayerExpCount += exp;
        if (PlayerExpCount >= ExpToNewxWave)
        {
            WaveCount++;
            foreach (var spawner in FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None))
            {
                spawner.spawnInterval *= 0.9f;
            }
            ShowBuildingPanel();
            ExpToNewxWave *= 4;
            if(WaveCount==6)
            {
                SceneManager.LoadScene("WinScene");
            }
        }
        bool buildingFase = GameObject.Find("ConstructionManager").GetComponent<ConstructionManager>().isBuildingPhase;
        if (PlayerExpCount%35 == 0 && !buildingFase)
        {
            ShowUpgradePanel();
        }
    }

    void ShowUpgradePanel()
    {
        Time.timeScale = 0f;
        UpgradePanel.GetComponent<UpgradePanel>().Show();
    }

    void ShowBuildingPanel()
    {
        GameObject.Find("ConstructionManager").GetComponent<ConstructionManager>().StartBuildPhase();
        Time.timeScale = 0f;
        BuildingPanel.SetActive(true);
    }
}
