using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeleeSlash : MonoBehaviour
{
    // Complete each section marked TODO.

    [Header("Slash Settings")]
    public float lifetime = 0.25f;
    public int damage = 5;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private void Awake()
    {
        // Gets component
        Collider2D col = GetComponent<Collider2D>();

        // Checks if col is not null, then sets a trigger if true

        if (col != null)
        {
            col.isTrigger = true;
        }

    }

  

    private void Start()
    {
        // Remove the slash after its lifetime has passed.
        Destroy(gameObject, lifetime);
    }

    public void Initialize(int damageAmount)
    {
        // DMG

        damage = damageAmount;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Attempts to get enemy component
        EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();

        // Checks if its an enemy
        if (enemy != null)
        {
            // Makes enemy takedamage

            enemy.TakeDamage(damage);

            // Debug.Log

            if (showDebugLogs == true)
            {
                Debug.Log("Enemy took" + damage);
            }

        }
    }
}