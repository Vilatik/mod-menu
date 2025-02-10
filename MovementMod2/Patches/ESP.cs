using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ESP
{
    private List<GameObject> targetObjects = new List<GameObject>(); // List to hold target objects
    private Canvas canvas;
    private bool isEnabled = true; // Flag to enable/disable the ESP

    // Dictionary to store object names with their associated color
    private Dictionary<string, Color> objectNames = new Dictionary<string, Color>();

    public ESP()
    {
        // Initialize the canvas
        InitializeCanvas();
    }

    private void InitializeCanvas()
    {
        // Create the Canvas if it doesn't exist
        GameObject canvasObj = GameObject.Find("ESP_Canvas");
        if (canvasObj == null)
        {
            canvasObj = new GameObject("ESP_Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvasComponent = canvasObj.GetComponent<Canvas>();
            if (canvasComponent != null)
            {
                canvasComponent.renderMode = RenderMode.WorldSpace;
            }
            else
            {
            }
        }

        // Set the canvas reference after it's created
        canvas = canvasObj.GetComponent<Canvas>();
        if (canvas == null)
        {
        }
    }

    public void SetEnabled(bool enabled)
    {
        // Toggle the ESP on or off
        isEnabled = enabled;
    }

    public void UpdateObjects(List<string> objectNamesList)
    {
        // Clear the previous list of objects
        targetObjects.Clear();

        // Loop through each object name to track
        foreach (var objectName in objectNamesList)
        {
            var objects = GameObject.FindObjectsOfType<GameObject>()
                .Where(obj => obj.name.Contains(objectName) || obj.CompareTag(objectName))
                .ToList();

            targetObjects.AddRange(objects);
        }
    }

    public void RenderESP()
    {
        if (!isEnabled) return; // Skip rendering if ESP is disabled

        foreach (var target in targetObjects)
        {
            if (target != null)
            {
                // Get the world position of the target object
                Vector3 worldPos = target.transform.position;

                // Convert the world position to screen position
                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

                if (screenPos.z > 0) // Only draw if the object is visible on screen
                {
                    // Get the appropriate color for this object name
                    Color color = objectNames.ContainsKey(target.name) ? objectNames[target.name] : Color.white;

                    // Draw ESP text at the target's screen position
                    DrawText(screenPos, target.name, color);
                }
            }
        }
    }

    private void DrawText(Vector3 screenPos, string text, Color color)
    {
        // Create a UI Text element at the specified screen position
        GameObject textObj = new GameObject("TargetText");
        textObj.transform.SetParent(canvas.transform);

        RectTransform rectTransform = textObj.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(screenPos.x, screenPos.y);

        Text uiText = textObj.AddComponent<Text>();
        uiText.text = text;
        uiText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        uiText.color = color;
        uiText.fontSize = 20;
    }

    public void AddObjectName(string name, Color color)
    {
        // Add a new object name with its associated color
        if (!objectNames.ContainsKey(name))
        {
            objectNames[name] = color;
        }
    }

    public void RemoveObjectName(string name)
    {
        // Remove an object name
        if (objectNames.ContainsKey(name))
        {
            objectNames.Remove(name);
        }
    }

    public List<string> GetObjectNames()
    {
        // Return the list of object names
        return objectNames.Keys.ToList();
    }
}
