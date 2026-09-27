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
    public AudioSource WaveAudio;
    public GameObject PausePanel;
    public GameObject BossPrefab;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        Time.timeScale = 1f;
    }

    void Update()
    {
        ExpText.text = "Exp: " + PlayerExpCount + "/" + ExpToNewxWave;
        WaveText.text = "Wave: " + WaveCount;
        HPText.text = "HP: " + Mathf.CeilToInt(GameObject.Find("Player").GetComponent<PlayerController>().Hp);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausePanel.activeSelf)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
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
                //Spawn boss
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
        WaveAudio.Play();
        UpgradePanel.GetComponent<UpgradePanel>().Show();
    }

    void ShowBuildingPanel()
    {
        GameObject.Find("ConstructionManager").GetComponent<ConstructionManager>().StartBuildPhase();
        Time.timeScale = 0f;
        WaveAudio.Play();
        BuildingPanel.SetActive(true);
    }

    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        PausePanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
    }
}
