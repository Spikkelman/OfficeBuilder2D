using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro < Forgot to add this due to using TMP_Text
using UnityEngine.SceneManagement; // In case you want to load a new scene on success

public class LoginManager : MonoBehaviour
{
    // References to the UI elements; assign these in the Inspector.
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button registerButton;
    public TMP_Text errorText;

    void Start()
    {
        // Clear any error messages at startup.
        errorText.text = "";

        // Add listeners for the button clicks.
        loginButton.onClick.AddListener(HandleLogin);
        registerButton.onClick.AddListener(HandleRegister);
    }

    // Called when the login button is clicked.
    void HandleLogin()
    {
        // Get user input.
        string username = usernameInput.text;
        string password = passwordInput.text;

        // Simple input validation.
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            errorText.text = "Username or password cannot be empty.";
            return;
        }

        // Here you would normally call your back-end API to verify credentials.
        // For demonstration, we simulate a simple check:
        if (username == "test" && password == "Password123!")
        {
            // Login successful.
            errorText.text = "";
            Debug.Log("Login successful!");

            // Optionally, load the next scene.
            SceneManager.LoadScene("WorldEditScene");
        }
        else
        {
            // Display an error message.
            errorText.text = "Invalid username or password.";
        }
    }

    // Called when the register button is clicked.
    void HandleRegister()
    {
        // You can either load a registration scene or show a registration form.
        Debug.Log("Register button clicked.");

        // For example, to load a registration scene:
        // SceneManager.LoadScene("RegisterScene");
    }
}
