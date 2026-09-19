using UnityEngine;
using UnityEngine.EventSystems;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance { get; private set; }

    [SerializeField] private BuildingData[] buildings;

    private BuildingData selectedBuilding;
    private GameObject preview;

    public bool isBuildingPhase;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!isBuildingPhase) return;

        if (preview != null)
            preview.transform.position = GetMouseWorldPosition();

        HandleMouse();
    }

    public void StartBuildPhase()
    {
        isBuildingPhase = true;
    }

    public void EndBuildPhase()
    {
        isBuildingPhase = false;
        CancelBuilding();
    }

    public void SelectBuilding(BuildingData building)
    {
        selectedBuilding = building;
        CreatePreview();
    }

    private void HandleMouse()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBuilding();
        }
        if (Input.GetMouseButtonDown(1))
        {
            EndBuildPhase();
        }
    }

    private void CreatePreview()
    {
        if (selectedBuilding == null) return;
        if (GameManager.Instance.PlayerExpCount < selectedBuilding.cost) return;
        if (preview != null) Destroy(preview);
        preview = Instantiate(selectedBuilding.prefab, GetMouseWorldPosition(), Quaternion.identity);
        foreach (var sr in preview.GetComponentsInChildren<SpriteRenderer>())
        {
            Color c = sr.color;
            c.a = 0.5f;
            sr.color = c;
        }
        foreach (var mb in preview.GetComponentsInChildren<MonoBehaviour>())
        {
            if (mb is Building) continue;
            mb.enabled = false;
        }
        foreach (var col in preview.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }
    }

    private void PlaceBuilding()
    {
        if (selectedBuilding == null)
            return;
        Vector3 worldPosition = GetMouseWorldPosition();
        GameObject buildingObject = Instantiate(
            selectedBuilding.prefab,
            worldPosition,
            Quaternion.identity
        );
        GameManager.Instance.AddExp(-selectedBuilding.cost);

        Building building = buildingObject.GetComponent<Building>();
        if (building != null)
            building.Setup(selectedBuilding);
        Destroy(preview);
        preview = null;
        selectedBuilding = null;
    }

    private void CancelBuilding()
    {
        selectedBuilding = null;
        if (preview != null)
        {
            Destroy(preview);
            preview = null;
        }
        Time.timeScale = 1f;
        GameObject.Find("BuildingPanel").SetActive(false);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 world = Camera.main.ScreenToWorldPoint(mouse);
        world.z = 0f;
        return world;
    }
}