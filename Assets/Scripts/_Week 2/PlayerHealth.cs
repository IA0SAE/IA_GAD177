using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class PlayerHealth : MonoBehaviour
{
    // Complete each section marked TODO.

    [Header("Health")]
    public int maxHP = 10;
    public int currentHP;

    [Header("Health UI")]
    public Slider healthSlider;
    public bool useHealthSlider = true;

    public TMP_Text healthText;
    public bool useHealthText = true;

    [Header("Hit Flash")]
    public SpriteRenderer spriteRenderer;
    public bool useHitFlash = true;
    public Color hitFlashColor = Color.red;
    public float flashDuration = 0.1f;

    [Header("Audio Feedback")]
    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip healSound;
    public AudioClip deathSound;

    [Header("Death Animation")]
    public Animator animator;
    public bool useDeathAnimation = true;
    public string deathAnimationName = "Death";
    public float deathDelay = 1.2f;

    [Header("Game Over")]
    public bool disableMovementOnDeath = true;
    public bool disableAttackOnDeath = true;

    [Header("References")]
    public PlayerMovement movement;
    public SingleMeleeAutoAttack meleeAutoAttack;
    public GameManager gameManager;

    [Header("Debug")]
    public bool enableDebugKeys = true;
    public bool showDebugLogs = true;

    public Key debugDamageKey = Key.Space;
    public int minimumDebugDamage = 1;
    public int maximumDebugDamage = 3;

    public Key debugHealKey = Key.H;
    public int minimumDebugHeal = 1;
    public int maximumDebugHeal = 3;

    public bool isDead { get; private set; }

    private Color originalColor;
    private Coroutine flashRoutine;

    private void Start()
    {
        

        currentHP = maxHP;


        UpdateHealthUI();

        if (movement == null)
        {
            movement = GetComponent<PlayerMovement>();
        }

        if (meleeAutoAttack == null)
        {
            meleeAutoAttack = GetComponent<SingleMeleeAutoAttack>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        SetupHealthUI();
        UpdateHealthUI();
    }

    private void Update()
    {
        HandleDebugInput();
    }

    private void HandleDebugInput()
    {
        if (enableDebugKeys == false || isDead == true)
        {
            return;
        }

        if (Keyboard.current == null)
        {
            return;
        }

        // If the debug damage key is pressed,
        // call the function that damages the player.
        if (Keyboard.current[debugDamageKey].wasPressedThisFrame)
        {
            DebugDamage();
        }

        // If the debug heal key is pressed,
        // call the function that heals the player.
        if (Keyboard.current[debugHealKey].wasPressedThisFrame)
        {
            DebugHeal();
        }
    }

    private void SetupHealthUI()
    {
        if (useHealthSlider == true && healthSlider != null)
        {
            healthSlider.minValue = 0;

        
            healthSlider.maxValue = maxHP;

          
            healthSlider.value = maxHP;
        }
    }

    private void UpdateHealthUI()
    {
        if (useHealthSlider == true && healthSlider != null)
        {
       
            healthSlider.maxValue = maxHP;

            
            healthSlider.value = currentHP;
        }

        if (useHealthText == true && healthText != null)
        {
            healthText.text = "HP: " + currentHP + "/" + maxHP;
        }
    }

    public void DebugDamage()
    {
        

        int minDebugDamage = 1;
        int maxDebugDamage = 2;
        
        int damageAmount = Random.Range(minDebugDamage, maxDebugDamage);



       

        Debug.Log("Random DMG Int" + damageAmount);

        TakeDamage(damageAmount);
    }

    public void DebugHeal()
    {
       

        int minDebugHeal = 1;
        int maxDebugHeal = 4;
            
        int healAmount = Random.Range(minDebugHeal, maxDebugHeal);



  


        Heal(healAmount);

        Debug.Log("Random Int:" +  healAmount);
    }

    public void TakeDamage(int amount)
    {
        if (isDead == true)
        {
            return;
        }

        

        currentHP -= amount;

      

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        

        Debug.Log("Damage Taken" + amount);
        Debug.Log("HP:" + currentHP);

        UpdateHealthUI();
        PlayDamageSound();

        if (useHitFlash == true)
        {
            StartHitFlash();
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead == true)
        {
            return;
        }

        

        currentHP += amount;

       

        if (currentHP < maxHP)
               currentHP = maxHP;
      
        

      Debug.Log("Amount Healed:" + amount);

        UpdateHealthUI();
        PlayHealSound();
    }

    private void PlayDamageSound()
    {
        if (audioSource == null)
        {
            return;
        }

        if (damageSound == null)
        {
            return;
        }

        audioSource.PlayOneShot(damageSound);

        if (showDebugLogs == true)
        {
            Debug.Log("PlayerHealth: Damage sound played.");
        }
    }

    private void PlayHealSound()
    {
        if (audioSource == null)
        {
            return;
        }

        if (healSound == null)
        {
            return;
        }

        audioSource.PlayOneShot(healSound);

        if (showDebugLogs == true)
        {
            Debug.Log("PlayerHealth: Heal sound played.");
        }
    }

    private void PlayDeathSound()
    {
        if (audioSource == null)
        {
            return;
        }

        if (deathSound == null)
        {
            return;
        }

        audioSource.PlayOneShot(deathSound);

        if (showDebugLogs == true)
        {
            Debug.Log("PlayerHealth: Death sound played.");
        }
    }

    private void StartHitFlash()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        // Only start the hit flash if one is not already running.
        if (flashRoutine == null)
        {
            flashRoutine = StartCoroutine(FlashOnHit());
        }
    }

    private IEnumerator FlashOnHit()
    {
        spriteRenderer.color = hitFlashColor;

        yield return new WaitForSeconds(flashDuration);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        // Allow the hit flash to be started again.
        flashRoutine = null;
    }

    private void Die()
    {
        if (isDead == true)
        {
            return;
        }

        isDead = true;

        if (showDebugLogs == true)
        {
            Debug.Log("PlayerHealth: Player died!");
        }

        PlayDeathSound();

        if (disableMovementOnDeath == true && movement != null)
        {
            movement.enabled = false;
        }

        if (disableAttackOnDeath == true && meleeAutoAttack != null)
        {
            meleeAutoAttack.attacksEnabled = false;
            meleeAutoAttack.enabled = false;
        }

        if (useDeathAnimation == true && animator != null)
        {
            animator.Play(deathAnimationName);

            if (showDebugLogs == true)
            {
                Debug.Log("PlayerHealth: Playing death animation: " + deathAnimationName);
            }
        }

        StartCoroutine(ShowGameOverAfterDelay());
    }

    private IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(deathDelay);

        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }
}