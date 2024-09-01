using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Utilities {
    // Disables a GameObject after a specified delay
    public static IEnumerator DisableObjectAfterTime(GameObject obj, float delay) {
        yield return new WaitForSeconds(delay);

        Debug.Log("Disabling object: " + obj.name);

        obj.SetActive(false);
    }

    public static Vector2 GetCanvasPosition(Vector3 worldPosition, Camera camera, Canvas canvas) {
        // Convert 3D object position to screen space
        Vector3 screenPos = camera.WorldToScreenPoint(worldPosition);

        // Convert screen position to Canvas space
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            null, // Pass null because Canvas is in Screen Space - Overlay mode
            out Vector2 canvasPos
        );

        return canvasPos;
    }

    // Checks if you are hitting a UI element or canvas or not
    public static bool IsClickOverUI(GraphicRaycaster graphicRaycaster, EventSystem eventSystem) {
        PointerEventData pointerEventData = new PointerEventData(eventSystem) {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results) {
            if (result.gameObject.GetComponent<Graphic>() != null) {
                return true; // Hit a UI element
            }
        }

        return false; // No UI element was hit
    }
}
