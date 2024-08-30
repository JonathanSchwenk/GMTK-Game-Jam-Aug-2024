using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;

public class UsePowerupManager : MonoBehaviour {

    [SerializeField] private GameObject ogPowerupButton;
    [SerializeField] private GameObject timeButton;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject destroyButton;
    [SerializeField] private GameObject enlargeButton;
    [SerializeField] private GameObject shrinkButton;
    [SerializeField] private GameObject closeMenuButton;

    private GameObject[] fanButtons;
    private float animationDuration = 0.5f;  // Duration of the animation
    private float spreadDistance = 100f;  // How far the buttons spread out
    private Vector2[] directions;   // Directions for the buttons to fan out

    private IAudioManager audioManager;
    private IInventoryManager inventoryManager;
    private IPlayingCanvasManager playingCanvasManager;
    private IGamePieceManager gamePieceManager;

    // Start is called before the first frame update
    void Start() {
        audioManager = ServiceLocator.Resolve<IAudioManager>();
        inventoryManager = ServiceLocator.Resolve<IInventoryManager>();
        playingCanvasManager = ServiceLocator.Resolve<IPlayingCanvasManager>();
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();

        InitializeButtons();
    }

    private void InitializeButtons() {
        // Create a list of buttons
        fanButtons = new GameObject[] { closeMenuButton, shrinkButton, enlargeButton, destroyButton, skipButton, timeButton };

        directions = new Vector2[fanButtons.Length];

        // Initialize buttons to start at the original button's position and be small
        foreach (GameObject button in fanButtons) {
            button.transform.position = ogPowerupButton.transform.position;
            button.transform.localScale = Vector3.zero;  // Start with a scale of zero (invisible)
        }

        // Define the angle range for the semi-circle (in radians)
        float startAngle = -Mathf.PI / 3.5f;  // -90 degrees (bottom)
        float endAngle = Mathf.PI / 3.5f;     // 90 degrees (top)

        // Calculate the angle step based on the number of buttons
        float angleStep = (endAngle - startAngle) / (directions.Length - 1);

        float radius = 1.6f; // Radius of the semi-circle

        for (int i = 0; i < directions.Length; i++) {
            float angle = startAngle + i * angleStep; // Calculate the angle for each button
            directions[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius; // Convert angle to a direction vector
        }
    }

    // Update is called once per frame
    void Update() {
        DisplayPowerupInventoryValues();
    }

    private void DisplayPowerupInventoryValues() {
        if (inventoryManager != null) {
            timeButton.GetComponentInChildren<TextMeshProUGUI>().text = inventoryManager.extraTime.ToString();
            skipButton.GetComponentInChildren<TextMeshProUGUI>().text = inventoryManager.skips.ToString();
            destroyButton.GetComponentInChildren<TextMeshProUGUI>().text = inventoryManager.destroys.ToString();
            enlargeButton.GetComponentInChildren<TextMeshProUGUI>().text = inventoryManager.extraEnlarges.ToString();
            shrinkButton.GetComponentInChildren<TextMeshProUGUI>().text = inventoryManager.extraShrinks.ToString();
        }
    }

    private IEnumerator FanOutButton(GameObject button, Vector3 startScale, Vector3 endScale, Vector3 startPosition, Vector3 endPosition) {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration) {
            button.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationDuration);
            button.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / animationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        button.transform.position = endPosition;
        button.transform.localScale = endScale;
    }

    // Opens the powerup menu
    public void PowerUpButton() {
        audioManager.PlaySFX("UIClick_General");

        // Stop time to allow you to pick something
        playingCanvasManager.countdownTimerIncrement = 0f;
        

        // Open the powerup menu
        for (int i = 0; i < fanButtons.Length; i++) {
            StartCoroutine(FanOutButton(fanButtons[i], Vector3.zero, Vector3.one, ogPowerupButton.transform.position, ogPowerupButton.transform.position + (Vector3)directions[i] * spreadDistance));
        }
    }

    // Close menu
    public void CloseMenuButton() {
        audioManager.PlaySFX("UIClick_General");

        CloseAction(true);
    }

    private void CloseAction(bool shouldToggleCountdown) {
        // Start time again
        if (shouldToggleCountdown) {
            playingCanvasManager.countdownTimerIncrement = 0.1f;
        }

        // Close the powerup menu
        for (int i = 0; i < fanButtons.Length; i++) {
            StartCoroutine(FanOutButton(fanButtons[i], Vector3.one, Vector3.zero, fanButtons[i].transform.position, ogPowerupButton.transform.position));
            fanButtons[i].transform.localScale = Vector3.zero;  // Start with a scale of zero (invisible)
        }
    }

    // Time
    public void TimeButton() {
        // Use time powerup
        if (inventoryManager.extraTime > 0) {
            inventoryManager.extraTime--;
            inventoryManager.SaveInventory();
            playingCanvasManager.countdownTimer += 5f;
        }
    }

    // Skip
    public void SkipButton() {
        // Use skip powerup
        if (inventoryManager.skips > 0) {
            inventoryManager.skips--;
            inventoryManager.SaveInventory();

            // Stop the timers coroutine
            playingCanvasManager.StopCountdown();

            // Reset the active game piece
            gamePieceManager.activeGamePiece.gameObject.GetComponent<MeshCollider>().isTrigger = false;
            Destroy(gamePieceManager.activeGamePiece.gameObject.GetComponent<Rigidbody>());
            Destroy(gamePieceManager.activeGamePiece.gameObject);
            gamePieceManager.activeGamePiece = null;

            CloseAction(true);
        }
    }

    // Destroy
    public void DestroyButton() {
        // Use destroy powerup
        if (inventoryManager.destroys > 0) {
            inventoryManager.destroys--;
            inventoryManager.SaveInventory();

            CloseAction(false);

            // Set icon of OG button to destroy icon
            ogPowerupButton.transform.GetChild(1).gameObject.SetActive(false);
            ogPowerupButton.transform.GetChild(2).gameObject.SetActive(true);

            gamePieceManager.activelyDestroying = true;

            // Also want to disable other buttons 
        }
    }

    // Enlarge
    public void EnlargeButton() {
        // Use enlarge powerup
        if (inventoryManager.extraEnlarges > 0) {
            inventoryManager.extraEnlarges--;
            inventoryManager.SaveInventory();
            gamePieceManager.enlargeRemaining += 1;
            gamePieceManager.curEnlargeValue = gamePieceManager.enlargeRemaining;
        }
    }

    // Shrink
    public void ShrinkButton() {
        // Use shrink powerup
        if (inventoryManager.extraShrinks > 0) {
            inventoryManager.extraShrinks--;
            inventoryManager.SaveInventory();
            gamePieceManager.shrinkRemaining += 1;
            gamePieceManager.curShrinkValue = gamePieceManager.shrinkRemaining;
        }
    }
}
