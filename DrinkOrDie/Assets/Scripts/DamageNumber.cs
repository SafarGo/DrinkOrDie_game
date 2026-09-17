using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float lifetime = 0.8f;

    private TMP_Text textComponent;
    private float timer;

    private void Awake()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
    }

    public void SetDamage(float damage)
    {
        textComponent.text = Mathf.RoundToInt(damage).ToString();
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}