using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int WaveCount = 1;
    public int PlayerExpCount = 0;
    public int ExpToNewxWave;
    public TMP_Text ExpText;
    public TMP_Text WaveText;
    public TMP_Text HPText;
    public GameObject UpgradePanel;
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
        HPText.text = "HP: " + GameObject.Find("Player").GetComponent<PlayerController>().Hp;
    }

    public void AddExp(int exp)
    {
        PlayerExpCount += exp;
        if (PlayerExpCount >= ExpToNewxWave)
        {
            WaveCount++;
            PlayerExpCount = 0;
            Time.timeScale = 0f;
            UpgradePanel.GetComponent<UpgradePanel>().Show();
        }
    }
}
