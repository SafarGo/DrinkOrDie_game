using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData Data { get; private set; }

    public void Setup(BuildingData data)
    {
        Data = data;
    }
}