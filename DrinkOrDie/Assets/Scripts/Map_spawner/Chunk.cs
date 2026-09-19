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
    {        initialized = true;
        topLeft = topLeftWorld;
        transform.position = new Vector3(topLeft.x + size / 2f, topLeft.y - size / 2f, 0f);
        var col = GetComponent<BoxCollider2D>();
        col.size = new Vector2(size, size);
        //col.isTrigger = false;
        Vector2 tr = new Vector2(topLeft.x + size, topLeft.y);
        Vector2 bl = new Vector2(topLeft.x, topLeft.y - size);
        Vector2 br = new Vector2(topLeft.x + size, topLeft.y - size);
        Debug.Log($"[{name}] topLeft={topLeft} size={size} | TL={topLeft} TR={tr} BL={bl} BR={br} | refs: TL={triggerTL?.name} TR={triggerTR?.name} BL={triggerBL?.name} BR={triggerBR?.name}");
        triggerTL.Setup(topLeft);
        triggerTR.Setup(tr);
        triggerBL.Setup(bl);
        triggerBR.Setup(br);
    }
}