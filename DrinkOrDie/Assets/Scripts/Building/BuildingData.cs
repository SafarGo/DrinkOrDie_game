using UnityEngine;

[CreateAssetMenu(fileName = "Building_", menuName = "Game/Building")]
public class BuildingData : ScriptableObject
{
    public BuildingType type;

    [Header("UI")]
    public Sprite icon;
    public string title;

    [Header("Строительство")]
    public GameObject prefab;
    public int cost;
}