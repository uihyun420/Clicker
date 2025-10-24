using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;

public class LogInUI : MonoBehaviour
{
    public GameObject loginPanel;

    [SerializeField] private Button anonyButton;
    [SerializeField] private Button logInButton;
    [SerializeField] private Button signUpButton;

    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;

    [SerializeField] private TextMeshProUGUI errorText;


    public Button profileButton;
    public TextMeshProUGUI profileText;

    private async Task Start()
    {
        SetButtonsInteractable(false);

        await UniTask.WaitUntil(() => AuthManager.Instance != null && AuthManager.Instance.IsInitialized);

        anonyButton.onClick.AddListener(() => OnAnonyButtonClicked().Forget());
        logInButton.onClick.AddListener(() => OnlogInButtonClicked().Forget());
        signUpButton.onClick.AddListener(() => OnSignUpButtonClicked().Forget());
        profileButton.onClick.AddListener(() => { AuthManager.Instance.SignOut(); UpdateUI().Forget(); });
        SetButtonsInteractable(true);

        UpdateUI().Forget();
    }

    private async UniTaskVoid OnAnonyButtonClicked()
    {
        string emailText = emailInput.text;
        string passwordText = passwordInput.text;
        SetButtonsInteractable(false);

        var (success, error) = await AuthManager.Instance.SingInAnonymouslyAsync();
        if (success)
        {
        }
        else
        {
            ShowError(error);
        }
        SetButtonsInteractable(true);
        UpdateUI().Forget();
    }

    public async UniTaskVoid UpdateUI()
    {
        if( AuthManager.Instance == null && !AuthManager.Instance.IsInitialized)
        {
            return;
        }
        bool isLoggedIn = AuthManager.Instance.IsLoggedIn;
        loginPanel.SetActive(!isLoggedIn);

        if(isLoggedIn)
        {
            string userId = AuthManager.Instance.UserId;
            profileText.text = userId;
        }
        else
        {
            profileText.text = string.Empty;
        }

        errorText.text = string.Empty;
    }

    private async UniTaskVoid OnlogInButtonClicked()
    {
        string emailText = emailInput.text;
        string passwordText = passwordInput.text;

        SetButtonsInteractable(false);
        AuthManager.Instance.SignOut();
        var (success, error) = await AuthManager.Instance.SignInWithEmailAsync(emailText, passwordText);
        if(success)
        {
        }
        else
        {
            ShowError(error);
        }
        SetButtonsInteractable(true);
        UpdateUI().Forget();

    }
    private async UniTaskVoid OnSignUpButtonClicked()
    {
        string emailText = emailInput.text;
        string passwordText = passwordInput.text;
        SetButtonsInteractable(false);

        var (success, error) = await AuthManager.Instance.CreateUserWithEmailAsync(emailText, passwordText);
        if (success)
        {
        }
        else
        {
            ShowError(error);
        }
        SetButtonsInteractable(true);
        UpdateUI().Forget();
    }

    private void ShowError(string message)
    {
        errorText.text= message;
        errorText.color = Color.red;
    }

    private void SetButtonsInteractable(bool b )
    {
        logInButton.interactable = b;
        signUpButton.interactable= b;
        anonyButton.interactable= b;
    }
}
