using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Chunk : MonoBehaviour
{
    [SerializeField] private float size = 16f;
    [SerializeField] private ChunkTrigger triggerTL;
    [SerializeField] private ChunkTrigger triggerTR;
    [SerializeField] private ChunkTrigger triggerBL;
    [SerializeField] private ChunkTrigger triggerBR;

    private bool initialized = false;

    private Vector2 topLeft;

    private void Start()
    {
    }


    public void Setup(Vector2 topLeftWorld)
    {
        initialized = true;
        topLeft = topLeftWorld;
        transform.position = new Vector3(
            topLeft.x + size / 2f,
            topLeft.y - size / 2f,
            0f
        );

        var col = GetComponent<BoxCollider2D>();
        col.size = new Vector2(size, size);
        Vector2 tr = topLeft + new Vector2(size, 0);
        Vector2 bl = topLeft + new Vector2(0, -size);
        Vector2 br = topLeft + new Vector2(size, -size);
        triggerTL.Setup(topLeft, topLeft);
        triggerTR.Setup(tr, topLeft);
        triggerBL.Setup(bl, topLeft);
        triggerBR.Setup(br, topLeft);
    }
}