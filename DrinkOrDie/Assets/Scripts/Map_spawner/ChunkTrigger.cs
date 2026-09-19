
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ChunkTrigger : MonoBehaviour
{
    [Header("Координаты текущего чанка")]
    [SerializeField] private Vector2 anchorPoint;

    [Header("Префаб чанка")]
    [SerializeField] private GameObject chunkPrefab;

    [Header("Размер чанка")]
    [SerializeField] private float chunkSize = 16f;

    [Header("Смещения соседей")]
    [SerializeField] private Vector2Int[] neighborOffsets;

    private bool isUsed;

    public void Setup(Vector2 triggerPosition, Vector2 chunkTopLeft)
    {
        transform.position = triggerPosition;
        anchorPoint = chunkTopLeft;

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

        foreach (var off in neighborOffsets)
        {
            int gx = baseX + off.x;
            int gy = baseY + off.y;

            Vector2 newTL = new Vector2(
                gx * chunkSize,
                gy * chunkSize
            );

            if (ChunkExistsAt(newTL))
                continue;

            GameObject go = Instantiate(
                chunkPrefab,
                Vector3.zero,
                Quaternion.identity
            );

            Chunk chunk = go.GetComponent<Chunk>();

            if (chunk != null)
            {
                chunk.Setup(newTL);
            }
        }
    }

    private bool ChunkExistsAt(Vector2 topLeft)
    {
        Vector2 center = new Vector2(
            topLeft.x + chunkSize / 2f,
            topLeft.y - chunkSize / 2f
        );

        Collider2D[] hits = Physics2D.OverlapPointAll(center);

        foreach (var hit in hits)
        {
            if (hit.GetComponentInParent<Chunk>() != null)
                return true;
        }

        return false;
    }
}