using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerKills : MonoBehaviour
{
    // Complete each section marked TODO.

    [Header("Kills")]
    public int currentKills = 0;

    [Header("Kills UI")]
    public TMP_Text killsText;
    public bool useKillsText = true;
    public string killsPrefix = "Kills: ";

    [Header("Debug")]
    public bool enableDebugKeys = true;
    public bool showDebugLogs = true;
    public int debugAddAmount = 1;
    public Key addKillKey = Key.B;
    public Key resetKillsKey = Key.V;

    [Header("Game Over Kills UI")]
    public TMP_Text gameOverKillsText;

    private void Start()
    {
        // Call the function that updates the kills UI.

        UpdateKillsUI();

        if (showDebugLogs == true)
        {
            Debug.Log("PlayerKills: Starting kills = " + currentKills);
        }
    }

    private void Update()
    {
        if (enableDebugKeys == true)
        {
            HandleDebugInput();
        }
    }

    private void HandleDebugInput()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        // If the Add Kill key is pressed,
        // call the function that adds kills.
        if (Keyboard.current[addKillKey].wasPressedThisFrame)
        {
            // Call AddKills and give it debugAddAmount.

            AddKills(debugAddAmount);
        }

        // If the Reset Kills key is pressed,
        // call the function that resets the player's kills.
        if (Keyboard.current[resetKillsKey].wasPressedThisFrame)
        {
            // Call ResetKills.

            ResetKills();
        }
    }

    public void AddKills(int amount)
    {
        if (amount < 0)
        {
            return;
        }

        // Add amount to the player's current kills.

        currentKills += amount;

        // Call the function that updates the kills UI.

        UpdateKillsUI();
    }

    public void ResetKills()
    {
        // Set the player's current kills back to 0.

        currentKills = 0;

        // Call the function that updates the kills UI.

        UpdateKillsUI();
    }

    public void DebugAddKill()
    {
        // Call AddKills and give it debugAddAmount.

        AddKills(debugAddAmount);
    }

    private void UpdateKillsUI()
    {
        if (useKillsText == false)
        {
            return;
        }

        // Update the kills text using killsPrefix
        // and the player's current kills.

      

        if (killsText == null)
        {
            return;
        }

        killsText.SetText(killsPrefix + currentKills);

        if (gameOverKillsText != null)
        {
            // Display the player's total kills
            // on the Game Over screen.

            gameOverKillsText.text = currentKills.ToString();

        }
    }
}