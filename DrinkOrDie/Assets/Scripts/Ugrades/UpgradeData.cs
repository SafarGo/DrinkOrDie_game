using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade_", menuName = "Game/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public UpgradeType type;

    [Header("UI")]
    public Sprite icon;
    public string title;
    [TextArea(2, 4)]
    public string description;

    [Header("Значение улучшения")]
    public float value;
}