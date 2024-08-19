using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dorkbots.ServiceLocatorTools;

public class TutorialCanvasManager : MonoBehaviour {

    [SerializeField] private List<GameObject> tutorialPages;
    [SerializeField] private GameObject prevButton;
    [SerializeField] private GameObject nextButton;

    public int currentPage = 0; // Set this to 0 to start at the first page when the tutorial is opened

    private IGameManager gameManager;

    // Start is called before the first frame update
    void Start() {
        gameManager = ServiceLocator.Resolve<IGameManager>();
    }

    // Update is called once per frame
    void Update() {
        if (currentPage == 0) {
            prevButton.SetActive(false);
            nextButton.SetActive(true);

            // Reset page 0 to active if it's not already
            for (int i = 1; i < tutorialPages.Count; i++) {
                tutorialPages[i].SetActive(false);
            }
            tutorialPages[0].SetActive(true);
        } else if (currentPage == tutorialPages.Count - 1) {
            nextButton.SetActive(false);
            prevButton.SetActive(true);
        } else {
            prevButton.SetActive(true);
            nextButton.SetActive(true);
        }
    }

    public void NextPage() {
        if (currentPage < tutorialPages.Count - 1) {
            tutorialPages[currentPage].SetActive(false);
            currentPage++;
            tutorialPages[currentPage].SetActive(true);
        }
    }

    public void PreviousPage() {
        if (currentPage > 0) {
            tutorialPages[currentPage].SetActive(false);
            currentPage--;
            tutorialPages[currentPage].SetActive(true);
        }
    }

    public void CloseTutorial() {
        tutorialPages[currentPage].SetActive(false);
        currentPage = 0;
        gameManager.UpdateGameState(GameState.StartMenu);
    }
}
