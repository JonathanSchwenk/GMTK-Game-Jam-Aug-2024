using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
