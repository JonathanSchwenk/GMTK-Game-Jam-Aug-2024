using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;
using UnityEngine.UI;

public class UsePowerupManager : MonoBehaviour, IPowerupManager {

    [SerializeField] private GameObject ogPowerupButton;
    [SerializeField] private GameObject timeButton;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject destroyButton;
    [SerializeField] private GameObject enlargeButton;
    [SerializeField] private GameObject shrinkButton;
    [SerializeField] private GameObject closeMenuButton;

    private GameObject[] fanButtons;
    private float animationDuration = 0.5f;  // Duration of the animation
    private float spreadDistance = 350f;  // How far the buttons spread out
    private Vector2[] directions;   // Directions for the buttons to fan out

    private IAudioManager audioManager;
    private IInventoryManager inventoryManager;
    private IPlayingCanvasManager playingCanvasManager;
    private IGamePieceManager gamePieceManager;

    public GameObject globalOgPowerupButton { get { return ogPowerupButton; } }

    // TODO: If you have no powerups then the button should be disabled visually

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
        ChangeButtonColor();
    }

    private void ChangeButtonColor() {
        if (inventoryManager.extraTime == 0) {
            timeButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.buttonOffColor;
        } else {
            timeButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.buttonOnColor;
        }

        if (inventoryManager.skips == 0) {
            skipButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.buttonOffColor;
        } else {
            skipButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.buttonOnColor;
        }

        if (inventoryManager.destroys == 0) {
            destroyButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.buttonOffColor;
        } else {
            destroyButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.buttonOnColor;
        }

        if (inventoryManager.extraEnlarges == 0) {
            enlargeButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.buttonOffColor;
        } else {
            enlargeButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.buttonOnColor;
        }

        if (inventoryManager.extraShrinks == 0) {
            shrinkButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.buttonOffColor;
        } else {
            shrinkButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.buttonOnColor;
        }
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

    private IEnumerator FanOutButton(RectTransform buttonRect, Vector3 startScale, Vector3 endScale, Vector2 startPosition, Vector2 endPosition) {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration) {
            // Lerp the anchoredPosition for UI elements
            buttonRect.anchoredPosition = Vector2.Lerp(startPosition, endPosition, elapsedTime / animationDuration);
            buttonRect.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / animationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position and scale are set properly at the end of the animation
        buttonRect.anchoredPosition = endPosition;
        buttonRect.localScale = endScale;
    }

    // Opens the powerup menu
    public void PowerUpButton() {
        // Don't want to do anything if we are actively destroying
        if (gamePieceManager.activelyDestroying) {
            return;
        }

        audioManager.PlaySFX("UIClick_General");

        // Stop time to allow you to pick something
        playingCanvasManager.countdownTimerIncrement = 0f;

        // Open the powerup menu
        for (int i = 0; i < fanButtons.Length; i++) {
            // Calculate start and end positions using RectTransform.anchoredPosition for UI elements
            RectTransform buttonRectTransform = fanButtons[i].GetComponent<RectTransform>();
            RectTransform ogButtonRectTransform = ogPowerupButton.GetComponent<RectTransform>();

            // Start at the original button's position
            Vector2 startPos = ogButtonRectTransform.anchoredPosition;
            // End position is based on the fan directions and spread distance
            Vector2 endPos = ogButtonRectTransform.anchoredPosition + directions[i] * spreadDistance;

            // Start coroutine to fan out the button
            StartCoroutine(FanOutButton(buttonRectTransform, Vector3.zero, Vector3.one, startPos, endPos));
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
            // Get the RectTransform of each button
            RectTransform buttonRect = fanButtons[i].GetComponent<RectTransform>();
            RectTransform ogButtonRect = ogPowerupButton.GetComponent<RectTransform>();

            // Start coroutine to reverse the fan out (i.e., fan in)
            StartCoroutine(FanOutButton(buttonRect, Vector3.one, Vector3.zero, buttonRect.anchoredPosition, ogButtonRect.anchoredPosition));

            // Immediately set the scale to zero after the coroutine to make the buttons invisible
            fanButtons[i].transform.localScale = Vector3.zero;
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
        if (inventoryManager.destroys > 0 && gamePieceManager.score > 0) {
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
