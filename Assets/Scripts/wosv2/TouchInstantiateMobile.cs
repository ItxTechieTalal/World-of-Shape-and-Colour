

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using MiniGame.FindAndMatch;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class TouchInstantiateMobile : MonoBehaviour
{
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] string requiredTag = "SpawnUI";
    [SerializeField] Canvas canvas; // Reference to the canvas
    private GameObject touchedObject;
    private GameObject currentPrefab;


    #region  LifeCycle Functions
    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            // Handle initial touch
            touchedObject = GetTouchedObject(touch.position);

            if (touchedObject == null)
            {
                Debug.Log("No object touched!");
                return;
            }

            if (touchedObject.tag != "Parent")
            {
                Debug.Log("Touched object has invalid tag: " + touchedObject.tag + ", GameObject: " + touchedObject.name);
                return;
            }

            InstantiatePrefabAtTouch(touch.position, touchedObject.name);
        }
        else if (touch.phase == TouchPhase.Moved && currentPrefab != null)
        {
            // Update the prefab position during touch move
            MovePrefabToTouch(touch.position);
        }
        else if (touch.phase == TouchPhase.Ended && currentPrefab != null)
        {
            // Finalize interaction when touch ends
            FinalizePrefabPosition(touch.position);
        }
    }
    #endregion

    #region Instantiate
    // Instantiate the prefab at the touch position
    private void InstantiatePrefabAtTouch(Vector2 touchPosition, string name)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            touchPosition,
            canvas.worldCamera,
            out localPoint
        );

        Vector3 worldPosition = canvas.transform.TransformPoint(localPoint);
        currentPrefab = Instantiate(prefabToSpawn, worldPosition, Quaternion.identity, canvas.transform);

        // Ensure the prefab has the necessary component (Wos2Item)
        Wos2Item currentPrefabS = currentPrefab.GetComponent<Wos2Item>();
        if (currentPrefabS == null)
        {
            Debug.LogError("Prefab does not have Wos2Item component!");
            return;
        }

        // Debug.LogError("Instantiated prefab with name: " + name);
        if (name == "0")
        {
            currentPrefabS.SetParentOnly(ItemManagerWorldV2.instance.parents[GamePlayWorldV2.instance.selectedParents[0]]);
            currentPrefab.name = "0";
        }
        else if (name == "1")
        {
            currentPrefabS.SetParentOnly(ItemManagerWorldV2.instance.parents[GamePlayWorldV2.instance.selectedParents[1]]);
            currentPrefab.name = "1";
        }
        else
        {
            currentPrefabS.SetParentOnly(ItemManagerWorldV2.instance.parents[GamePlayWorldV2.instance.selectedParents[2]]);
            currentPrefab.name = "2";
        }

        // Optionally, you can immediately set it up for dragging
        // currentPrefab.AddComponent<DragWorldV2>(); // Assuming you have DragWorldV2 that handles the movement
    }
    #endregion

    // Move the instantiated prefab as the touch moves
    private void MovePrefabToTouch(Vector2 touchPosition)
    {
        if (currentPrefab == null) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            touchPosition,
            canvas.worldCamera,
            out localPoint
        );

        Vector3 worldPosition = canvas.transform.TransformPoint(localPoint);
        currentPrefab.transform.position = worldPosition;  // Update prefab position to touch position
    }

    // Finalize the position when touch ends
    private void FinalizePrefabPosition(Vector2 touchPosition)
    {
        if (currentPrefab == null) return;

        // Convert the touch position to local space in the canvas
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            touchPosition,
            canvas.worldCamera,
            out localPoint
        );

        Vector3 worldPosition = canvas.transform.TransformPoint(localPoint);

        // You can check if the prefab is dropped in a valid zone

        // Optionally, finalize the drop logic based on the final position (for example, snapping to a target)
        // Debug.Log("Prefab dropped at: " + worldPosition);

        // Reset reference if needed
        currentPrefab.GetComponent<Image>().raycastTarget = false;
        currentPrefab.transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
        CheckDropZone(worldPosition);

        currentPrefab = null;
    }

    // Get the object that was touched at the beginning
    private GameObject GetTouchedObject(Vector2 touchPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = touchPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            return results[0].gameObject; // Return the first object under the touch
        }

        return null;
    }

    #region Check Drop Zone
    private void CheckDropZone(Vector3 worldPosition)
    {
        // Convert world position to screen space
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, worldPosition);

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        // List of raycast results (UI elements under the touch position)
        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();

        // Raycast all UI elements under the pointer
        raycaster.Raycast(pointerData, results);

        // Check if there are any results
        if (results.Count > 0)
        {
            // Get the first result
            GameObject hitObject = results[0].gameObject;

            // Check if the hit object is in the specified layer (e.g., "Child" layer)
            int layerMask = LayerMask.GetMask("Child");  // Replace "Child" with your desired layer
            if (hitObject.layer == LayerMask.NameToLayer("Child") && hitObject.name == currentPrefab.name)
            {
                Debug.Log("Prefab dropped on a valid child object: " + hitObject.transform.name);
                // Snap the prefab to the container's position
                currentPrefab.transform.position = hitObject.transform.position;
                Wos2Item item = hitObject.GetComponent<Wos2Item>();
                item.turnMyParentOn(currentPrefab.transform.GetChild(0).GetComponent<Image>().sprite);
                GamePlayWorldV2.instance.CheckIfAnyChildQuestionSolved();
                Destroy(currentPrefab);
            }
            else
            {
                Debug.Log("Prefab dropped outside of valid layer zone" + hitObject.transform.name + " " + hitObject.layer.ToString());
                Destroy(currentPrefab);
                // Optionally, reset the prefab to its original position
            }
        }
        else
        {
            Debug.Log("Prefab dropped outside of valid layer zone");
            Destroy(currentPrefab);
            // Optionally, reset the prefab to its original position
        }
    }

    // Check if the prefab is dropped on a valid drop zone (e.g., "Container" tag)
    // Check if the prefab is dropped on a valid drop zone (UI-based)
    #endregion
}

