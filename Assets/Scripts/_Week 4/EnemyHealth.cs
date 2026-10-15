using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EnemyHealth : MonoBehaviour
{
    // Complete each section marked TODO.

    [Header("Health")]
    public int maxHP = 3;
    public int currentHP;

    [Header("Rewards")]
    public int xpReward = 5;
    public int moneyReward = 1;
    public int killReward = 1;

    [Header("Reward References")]
    public PlayerExperience playerExperience;
    public PlayerMoney playerMoney;
    public PlayerKills playerKills;

    [Header("Coin Drops")]
    public GameObject coinPrefab;
    public int coinsToDrop = 1;

    [Range(0f, 1f)]
    public float coinDropChance = 0.5f;

    public float coinSpreadRadius = 0.5f;

    [Header("Hit Flash")]
    public SpriteRenderer spriteRenderer;
    public bool useHitFlash = true;
    public Color hitFlashColor = Color.red;
    public float flashDuration = 0.1f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip deathSound;

    [Header("Death Event")]
    public UnityEvent onDie;

    [Header("Debug")]
    public bool enableDebugKeys = true;
    public bool showDebugLogs = true;
    public Key debugDamageKey = Key.U;
    public int debugDamageAmount = 1;

    private Color originalColor;
    private bool isDead = false;
    private Coroutine flashRoutine;

    private void Awake()
    {
        // Checks SpriteRenderer is null


        if (spriteRenderer  == null) GetComponent<Renderer>();


        // Checks SpriteRenderer isn't null

        if (spriteRenderer != null) GetComponent<Renderer>();

        // Checks AudioSource is null

        if (audioSource == null) GetComponent<AudioSource>();

        // Checks PlayerExperience is null

       if (playerExperience == null) FindFirstObjectByType<PlayerExperience>();

 
        // Checks if playerMoney is null.

       if (playerMoney == null) FindFirstObjectByType<PlayerMoney>();


        // Checks if playerKills is null.
        

       if (playerKills == null) FindFirstObjectByType<PlayerKills>();
    }

    private void Start()
    {
        currentHP = maxHP;

        if (showDebugLogs == true)
        {
            Debug.Log("EnemyHealth (" + name + "): Started with HP " + currentHP + "/" + maxHP);
        }
    }

    private void Update()
    {
        HandleDebugInput();
    }

    private void HandleDebugInput()
    {
        if (enableDebugKeys == false)
        {
            return;
        }

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current[debugDamageKey].wasPressedThisFrame)
        {
            TakeDamage(debugDamageAmount);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead == true)
        {
            return;
        }

        // 

        currentHP -= amount;
        if (currentHP < 0)

        // 

        if (currentHP > 0) 
        { 
            currentHP = 0;
        }

        // Debug Log

        if (showDebugLogs == true)
        {
            Debug.Log("DMG Taken" + amount + "Current HP" + currentHP);
        }

        //

        PlayHitFeedback();

        // Die or not

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void PlayHitFeedback()
    {
        // Only start the hit flash if one is not already running.
        if (useHitFlash == true && spriteRenderer != null && flashRoutine == null)
        {
            flashRoutine = StartCoroutine(FlashOnHit());
        }

        // Hitsounds

        {
            if (audioSource != null && hitSound != null) { audioSource.Play(); }
        }
    }

    private IEnumerator FlashOnHit()
    {
        if (spriteRenderer == null)
        {
            flashRoutine = null;
            yield break;
        }

        // Changes spritecolour to hitflash.

        spriteRenderer.color = hitFlashColor;


        yield return new WaitForSeconds(flashDuration);

        //  Reverts Colour back to normal

        if (spriteRenderer != null) { spriteRenderer.color = originalColor; }


        // Allow the hit flash to be started again.
        flashRoutine = null;
    }

    private void Die()
    {
        // Checks if enemy is dead

        if (isDead == true)
        {
            return;
        }

        // Read.

        isDead = true;

        // Debug

        if (showDebugLogs == true)
        {
            Debug.Log("Enemy Died");
         }

        //

       AwardRewards();

        //

        DropCoins();

        // 

        PlayDeathSound();

        // Invoke any functions connected to the On Die event in the Inspector.
        // We will look at how UnityEvents work in more detail in later weeks.
        if (onDie != null)
        {
            onDie.Invoke();
        }

        // Remove the enemy from the game.
        Destroy(gameObject);
    }

    private void AwardRewards()
    {
        // Example: Check that the PlayerExperience reference exists
        // before calling one of its functions.
        if (playerExperience != null)
        {
            playerExperience.AddXP(xpReward);
        }

        // Gives money to player

        if (playerMoney != null)
        {
            playerMoney.AddMoney(xpReward);
        }

        // Awards player with kill

        if (playerKills  != null)
        {
            playerKills.AddKills(xpReward);
        }
    }

    private void DropCoins()
    {
        if (coinPrefab == null)
        {
            return;
        }

        if (coinsToDrop <= 0)
        {
            return;
        }

        // 
        float roll = Random.Range(0f, 1f);


        // Checks if enemy drops coins

             
        if (roll > 0)
        {
            if (showDebugLogs == true)
            {
                Debug.Log("EnemyHealth (" + name + "): Coin drop failed. Roll = " + roll);
            }

            return;
        }

        // Create the requested number of coins.
        for (int i = 0; i < coinsToDrop; i++)
        {
            Vector2 offset = Random.insideUnitCircle * coinSpreadRadius;

            Vector3 spawnPosition = transform.position + (Vector3)offset;

            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }

        if (showDebugLogs == true)
        {
            Debug.Log("EnemyHealth (" + name + "): Dropped " + coinsToDrop + " coin(s).");
        }
    }

    private void PlayDeathSound()
    {
        // Playing the proper sounds

        if (audioSource != null && deathSound != null) {GetComponent<AudioSource>().Play(); };
    }
}