using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private UpgradeCardUI[] cards;
    [SerializeField] private UpgradeData[] upgrades;
    [SerializeField] private UpgradeManager upgradeManager;

    public void Show()
    {
        gameObject.SetActive(true);
        List<UpgradeData> available =
            new List<UpgradeData>(upgrades);

        for (int i = 0; i < cards.Length; i++)
        {
            if (available.Count == 0)
                break;
            int randomIndex = Random.Range(0, available.Count);
            UpgradeData upgrade = available[randomIndex];
            cards[i].Setup(upgrade, this);
            available.RemoveAt(randomIndex);
        }
    }

    public void SelectUpgrade(UpgradeData upgrade)
    {
        upgradeManager.ApplyUpgrade(upgrade);
        gameObject.SetActive(false);
    }
}
