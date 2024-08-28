using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasManager : MonoBehaviour, IMenuCanvasManager {

    public MenuCanvasState menuCanvasState { get; set; }
    public Action<MenuCanvasState> OnMenuCanvasStateChanged { get; set; }

    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject missionsMenu;
    [SerializeField] private GameObject shopMenu;


    private float moveDuration = 1.0f; // Duration of the movement in seconds

    // Start is called before the first frame update
    void Start() {
        UpdateCanvasState(MenuCanvasState.Start);
    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateCanvasState(MenuCanvasState newState) {
        menuCanvasState = newState;

        // Switch statement that deals with each possible state 
        switch (newState) {
            case MenuCanvasState.Start:
                MoveMenus(MenuCanvasState.Start);
                break;
            case MenuCanvasState.Missions:
                MoveMenus(MenuCanvasState.Missions);
                break;
            case MenuCanvasState.Shop:
                MoveMenus(MenuCanvasState.Shop);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        // Null checker then calls the action for anthing subscribed to it
        OnMenuCanvasStateChanged?.Invoke(newState);
    }

    private void MoveMenus(MenuCanvasState menuCanvasState) {
        RectTransform missionsRect = missionsMenu.GetComponent<RectTransform>();
        RectTransform startRect = startMenu.GetComponent<RectTransform>();
        RectTransform shopRect = shopMenu.GetComponent<RectTransform>();

        if (menuCanvasState == MenuCanvasState.Start) {
            StartCoroutine(MoveObject(missionsRect, missionsRect.anchoredPosition, new Vector3(-1700, 0, 0), moveDuration));
            StartCoroutine(MoveObject(startRect, startRect.anchoredPosition, new Vector3(0, 0, 0), moveDuration));
            StartCoroutine(MoveObject(shopRect, shopRect.anchoredPosition, new Vector3(1700, 0, 0), moveDuration));
        } else if (menuCanvasState == MenuCanvasState.Missions) {
            StartCoroutine(MoveObject(missionsRect, missionsRect.anchoredPosition, new Vector3(0, 0, 0), moveDuration));
            StartCoroutine(MoveObject(startRect, startRect.anchoredPosition, new Vector3(1700, 0, 0), moveDuration));
            StartCoroutine(MoveObject(shopRect, shopRect.anchoredPosition, new Vector3(3400, 0, 0), moveDuration));
        } else if (menuCanvasState == MenuCanvasState.Shop) {
            StartCoroutine(MoveObject(missionsRect, missionsRect.anchoredPosition, new Vector3(-3400, 0, 0), moveDuration));
            StartCoroutine(MoveObject(startRect, startRect.anchoredPosition, new Vector3(-1700, 0, 0), moveDuration));
            StartCoroutine(MoveObject(shopRect, shopRect.anchoredPosition, new Vector3(0, 0, 0), moveDuration));
        }
    }

    private IEnumerator MoveObject(RectTransform rectTransform, Vector3 startPos, Vector3 endPos, float duration) {
        float elapsedTime = 0;

        while (elapsedTime < duration) {
            rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        rectTransform.anchoredPosition = endPos; // Ensure the final position is set
    }
}

public enum MenuCanvasState {
    Start,
    Missions,
    Shop
}
