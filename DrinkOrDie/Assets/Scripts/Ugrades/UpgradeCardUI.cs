using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Button button;

    public UpgradeData currentUpgrade;
    public UpgradePanel upgradePanel;

    public void Setup(UpgradeData data, UpgradePanel panel)
    {
        currentUpgrade = data;
        upgradePanel = panel;

        icon.sprite = data.icon;
        title.text = data.title;
        description.text = data.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        upgradePanel.SelectUpgrade(currentUpgrade);
        Time.timeScale = 1f;
    }
}