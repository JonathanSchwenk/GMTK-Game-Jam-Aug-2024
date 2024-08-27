using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;
using System;

public class GameOverCanvasManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI yourScoreValue;
    [SerializeField] private TextMeshProUGUI bestScoreValue;
    [SerializeField] private TextMeshProUGUI roundsMaxChainValue;
    [SerializeField] private TextMeshProUGUI bestMaxChainValue;

    [SerializeField] private GameObject orthoCamera;
    [SerializeField] private GameObject perspectiveCamera;
    [SerializeField] private GameObject cityCamera;

    [SerializeField] private GameObject cityView;
    [SerializeField] private GameObject defaultView;


    private float radius = 180f; // The radius of the circle
    private float speed = 0.25f;  // The speed of the movement
    private float angle = 0f; // The current angle of rotation
    public Vector3 targetPosition = new Vector3(100, 70, 60); // Target position (center of the circle)


    private IGamePieceManager gamePieceManager;
    private IGameManager gameManager;
    private IAudioManager audioManager;
    private ISaveManager saveManager;

    // Start is called before the first frame update
    void Start() {
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        gameManager = ServiceLocator.Resolve<IGameManager>();
        audioManager = ServiceLocator.Resolve<IAudioManager>();
        saveManager = ServiceLocator.Resolve<ISaveManager>();
    }

    // Update is called once per frame
    void Update() {
        yourScoreValue.text = Mathf.RoundToInt(gamePieceManager.score).ToString();
        bestScoreValue.text = Mathf.RoundToInt(saveManager.saveData.highScore).ToString();
        roundsMaxChainValue.text = Mathf.RoundToInt(gamePieceManager.curMaxChain).ToString();
        bestMaxChainValue.text = Mathf.RoundToInt(saveManager.saveData.highChain).ToString();

        if (gameManager.gameState == GameState.GameOver) {
            orthoCamera.SetActive(false);
            perspectiveCamera.SetActive(false);
            cityCamera.SetActive(true);

            // Rotate the camera around the target
            Rotate("right");
        }
    }

    public void OnReplayPressed() {
        // Resets the game state and starts a new round
        gamePieceManager.InitRoundStats();
        gameManager.UpdateGameState(GameState.Playing);

        audioManager.PlaySFX("UIClick_General");
        audioManager.PlaySFX("StartGame");
        audioManager.StopMusic("MenuMusic");
        audioManager.PlayMusic("GameplayMusic");
    }

    public void OnQuitPressed() {
        audioManager.PlaySFX("UIClick_General");
        // Quits the game
        Application.Quit();
    }

    public void OnMainMenuPressed() {
        audioManager.PlaySFX("UIClick_General");
        // Returns to the main menu after resetting
        gamePieceManager.InitRoundStats();
        gameManager.UpdateGameState(GameState.StartMenu);

        // Reset camera
        orthoCamera.SetActive(true);
        cityCamera.SetActive(false);
    }

    public void OnViewCityPressed() {
        audioManager.PlaySFX("UIClick_General");
        // Opens the city view
        cityView.SetActive(true);
        defaultView.SetActive(false);
        
        // Reset camera
        orthoCamera.SetActive(true);
        cityCamera.SetActive(false);
    }

    public void OnBackToDefaultPressed() {
        audioManager.PlaySFX("UIClick_General");
        // Opens the default view
        cityView.SetActive(false);
        defaultView.SetActive(true);
    }

    public void OnRotateLeftPressed() {
        audioManager.PlaySFX("UIClick_Scale");
        Rotate("left");
    }

    public void OnRotateRightPressed() {
        audioManager.PlaySFX("UIClick_Scale");
        Rotate("right");
    }

    private void Rotate(string direction) {
        print("Rotating " + direction);
        print(Time.timeScale);
        // Calculate the new angle based on the speed
        if (direction == "left") {
            angle -= speed * Time.deltaTime;
        } else if (direction == "right") {
            angle += speed * Time.deltaTime;
        }

        // Calculate the new position of the camera
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        // Update the camera's position
        cityCamera.transform.position = new Vector3(x, cityCamera.transform.position.y, z) + targetPosition;

        // Make the camera look at the target
        cityCamera.transform.LookAt(targetPosition);
    }
}
