using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;
using TMPro;
using System;
using UnityEngine.UI;

public class StartCanvasManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI bestScoreValue;
    [SerializeField] private TextMeshProUGUI bestChainValue;

    [SerializeField] private TutorialCanvasManager tutorialCanvasManager;
    [SerializeField] private GameObject sfxOn;
    [SerializeField] private GameObject sfxOff;
    [SerializeField] private GameObject musicOn;
    [SerializeField] private GameObject musicOff;

    [SerializeField] private Sprite leftArrow;
    [SerializeField] private Sprite rightArrow;
    [SerializeField] private Image missionButton;
    [SerializeField] private Image shopButton;


    private IGameManager gameManager;
    private IGamePieceManager gamePieceManager;
    private IAudioManager audioManager;
    private ISaveManager saveManager;
    private IMenuCanvasManager menuCanvasManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = ServiceLocator.Resolve<IGameManager>();
        gamePieceManager = ServiceLocator.Resolve<IGamePieceManager>();
        audioManager = ServiceLocator.Resolve<IAudioManager>();
        saveManager = ServiceLocator.Resolve<ISaveManager>();
        menuCanvasManager = ServiceLocator.Resolve<IMenuCanvasManager>();

        if (audioManager != null) {
            InitSoundAndMusic();
        }
    }

    // Update is called once per frame
    void Update()
    {
        bestScoreValue.text = Mathf.RoundToInt(saveManager.saveData.highScore).ToString();
        bestChainValue.text = Mathf.RoundToInt(saveManager.saveData.highChain).ToString();

        if (menuCanvasManager.menuCanvasState == MenuCanvasState.Missions) {
            missionButton.sprite = rightArrow;
        } else {
            missionButton.sprite = leftArrow;
        }
        if (menuCanvasManager.menuCanvasState == MenuCanvasState.Shop) {
            shopButton.sprite = leftArrow;
        } else {
            shopButton.sprite = rightArrow;
        }
    }

    public void OnStartButtonPressed() {
        // Sound and music
        audioManager.PlaySFX("UIClick_General");
        audioManager.PlaySFX("StartGame");
        audioManager.StopMusic("MenuMusic");
        audioManager.PlayMusic("GameplayMusic");

        gamePieceManager.InitRoundStats();
        gameManager.UpdateGameState(GameState.Playing);
    }

    public void OnTutorialButtonPressed() {
        audioManager.PlaySFX("UIClick_General");

        gamePieceManager.InitRoundStats();
        tutorialCanvasManager.currentPage = 0;
        gameManager.UpdateGameState(GameState.Tutorial);
    }

    public void OnExitButtonPressed() {
        audioManager.PlaySFX("UIClick_General");
        Application.Quit();
    }

    public void ToggleSFX() {
        audioManager.PlaySFX("UIClick_General");

        if (saveManager.saveData.SFXOn) {
            saveManager.saveData.SFXOn = false;

            sfxOn.SetActive(false);
            sfxOff.SetActive(true);
        } else {
            saveManager.saveData.SFXOn = true;

            sfxOn.SetActive(true);
            sfxOff.SetActive(false);
        }

        saveManager.Save();
    }

    public void ToggleMusic() {
        audioManager.PlaySFX("UIClick_General");

        // If already on, stop music, else, play music
        if (saveManager.saveData.musicOn) {
            saveManager.saveData.musicOn = false;

            audioManager.StopMusic("MenuMusic");
            audioManager.StopMusic("GameplayMusic");

            musicOn.SetActive(false);
            musicOff.SetActive(true);
        } else {
            saveManager.saveData.musicOn = true;

            audioManager.PlayMusic("MenuMusic");

            musicOn.SetActive(true);
            musicOff.SetActive(false);
        }

        saveManager.Save();
    }

    private void InitSoundAndMusic() {
        // music
        if (saveManager.saveData.musicOn) {
            audioManager.PlayMusic("MenuMusic");

            musicOn.SetActive(true);
            musicOff.SetActive(false);
        } else {
            audioManager.StopMusic("MenuMusic");
            audioManager.StopMusic("GameplayMusic");

            musicOn.SetActive(false);
            musicOff.SetActive(true);
        }

        // sfx
        if (saveManager.saveData.SFXOn) {
            sfxOn.SetActive(true);
            sfxOff.SetActive(false);
        } else {
            sfxOn.SetActive(false);
            sfxOff.SetActive(true);
        }
    }

    public void OnShopButtonPressed() {
        audioManager.PlaySFX("UIClick_General");

        if (menuCanvasManager.menuCanvasState == MenuCanvasState.Shop) {
            menuCanvasManager.UpdateCanvasState(MenuCanvasState.Start);
        } else {
            menuCanvasManager.UpdateCanvasState(MenuCanvasState.Shop);
        }
    }

    public void OnMissionsButtonPressed() {
        audioManager.PlaySFX("UIClick_General");

        if (menuCanvasManager.menuCanvasState == MenuCanvasState.Missions) {
            menuCanvasManager.UpdateCanvasState(MenuCanvasState.Start);
        } else {
            menuCanvasManager.UpdateCanvasState(MenuCanvasState.Missions);
        }
    }
}
