using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ChunkTrigger : MonoBehaviour
{
    [Header("Точка привязки (мир)")]
    [SerializeField] private Vector2 anchorPoint;

    [Header("Префаб чанка")]
    [SerializeField] private GameObject chunkPrefab;

    [Header("Размер чанка")]
    [SerializeField] private float chunkSize = 16f;

    [Header("Куда строить относительно anchorPoint")]
    [SerializeField] private Vector2Int[] neighborOffsets;

    private bool isUsed = false;
    public void Setup(Vector2 anchor)
    {
        anchorPoint = anchor;
        transform.position = anchor;

        var col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;

        isUsed = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isUsed) return;
        if (!other.CompareTag("Player")) return;

        isUsed = true;
        SpawnNeighbors();
    }

    private void SpawnNeighbors()
    {
        int baseX = Mathf.RoundToInt(anchorPoint.x / chunkSize);
        int baseY = Mathf.RoundToInt(anchorPoint.y / chunkSize);
        Debug.Log($"[{name}] anchor={anchorPoint} chunkSize={chunkSize} base=({baseX},{baseY})");

        foreach (var off in neighborOffsets)
        {
            Vector2 newTL = ComputeTopLeft(baseX + off.x, baseY + off.y);
            Debug.Log($"[{name}] off={off} -> newTL={newTL}");

            if (ChunkExistsAt(newTL))
            {
                Debug.Log($"[{name}] skip — already exists");
                continue;
            }

            GameObject go = Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity);
            Chunk chunk = go.GetComponent<Chunk>();
            chunk.Setup(newTL);
        }
    }

    private Vector2 ComputeTopLeft(int gx, int gy)
    {
        return new Vector2(gx * chunkSize, gy * chunkSize);
    }
    private bool ChunkExistsAt(Vector2 topLeft)
    {
        Vector2 center = new Vector2(topLeft.x + chunkSize / 2f, topLeft.y - chunkSize / 2f);
        Collider2D hit = Physics2D.OverlapPoint(center);
        return hit != null && hit.GetComponentInParent<Chunk>() != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(4f, 4f, 0f));
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(anchorPoint, 0.3f);
    }
}