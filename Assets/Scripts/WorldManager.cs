using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    // --- UI References for World Creation ---
    [Header("World Creation Panel")]
    public TMP_InputField worldNameInput;
    public Button createWorldButton;
    public TMP_Text creationFeedbackText;

    // --- UI References for World Overview ---
    [Header("World Overview Panel")]
    // Parent object (e.g., Content panel of a ScrollView) where world buttons will be added
    public Transform overviewContentPanel;
    // Prefab for a single world overview entry (must contain a Button and a TMP_Text for the world name)
    public GameObject worldButtonPrefab;

    // --- Data Model: List of worlds owned by the user ---
    private List<WorldData> userWorlds = new List<WorldData>();

    [System.Serializable]
    public class WorldData
    {
        public string worldName;
        // In a full implementation you could add additional fields (unique ID, associated 2D object data, etc.)
    }

    void Start()
    {
        // Clear any feedback text at startup
        creationFeedbackText.text = "";

        // Hook up the button for creating a new world
        createWorldButton.onClick.AddListener(CreateWorld);

        // Populate the overview list (if there are pre‑existing worlds)
        PopulateOverview();
    }

    // --- World Creation ---
    void CreateWorld()
    {
        string nameInput = worldNameInput.text.Trim();

        // Validation: world name must be between 1 and 25 characters
        if (string.IsNullOrEmpty(nameInput))
        {
            creationFeedbackText.text = "World name cannot be empty.";
            return;
        }
        if (nameInput.Length < 1 || nameInput.Length > 25)
        {
            creationFeedbackText.text = "World name must be between 1 and 25 characters.";
            return;
        }
        // Check for duplicate world name
        foreach (var world in userWorlds)
        {
            if (world.worldName == nameInput)
            {
                creationFeedbackText.text = "A world with this name already exists.";
                return;
            }
        }
        // Check maximum number of worlds (5)
        if (userWorlds.Count >= 5)
        {
            creationFeedbackText.text = "You cannot create more than 5 worlds.";
            return;
        }

        // If validations pass, create a new world and add it to the user's list
        WorldData newWorld = new WorldData
        {
            worldName = nameInput
        };

        userWorlds.Add(newWorld);
        creationFeedbackText.text = "World created successfully!";

        // Optionally, clear the input field
        worldNameInput.text = "";

        // Refresh the overview list to show the new world
        PopulateOverview();
    }

    // --- Populate the World Overview List ---
    void PopulateOverview()
    {
        // Clear current overview list
        foreach (Transform child in overviewContentPanel)
        {
            Destroy(child.gameObject);
        }

        // Create a button for each world
        foreach (var world in userWorlds)
        {
            GameObject newButton = Instantiate(worldButtonPrefab, overviewContentPanel);
            // Set the button text to the world's name (using TMP_Text)
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = world.worldName;
            }

            // Hook up the button click to load/view the world
            Button btn = newButton.GetComponent<Button>();
            if (btn != null)
            {
                // Use a local copy of world to avoid closure issues
                WorldData currentWorld = world;
                btn.onClick.AddListener(() => OnWorldSelected(currentWorld));
            }

            // Delete button
            Transform deleteBtnTransform = newButton.transform.Find("DeleteButton");
            if(deleteBtnTransform != null)
            {
                 Button deleteBtn = deleteBtnTransform.GetComponent<Button>();
                deleteBtn.onClick.AddListener(() => DeleteWorld(world));
            }
        }
    }

    // --- When a World is Selected for Viewing ---
    void OnWorldSelected(WorldData world)
    {
        Debug.Log("Viewing world: " + world.worldName);
        // Here you would pass the world data (via a static manager or similar)
        // and load the scene for viewing/editing that world.
        // For example:
        // SelectedWorldData.worldName = world.worldName;
        // SceneManager.LoadScene("WorldEditScene");
    }

    // --- Delete a World (and its associated 2D objects) ---
    // This method can be called from a delete button attached to a world entry.
    public void DeleteWorld(WorldData worldToDelete)
    {
        userWorlds.Remove(worldToDelete);
        Debug.Log("Deleted world: " + worldToDelete.worldName);
        // In a complete implementation, you would also remove associated 2D objects for that world.

        // Refresh the overview list
        PopulateOverview();
    }
}