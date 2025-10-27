using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.Text;

public class LogInUI : MonoBehaviour
{
    public GameObject loginPanel;

    [SerializeField] private ProfileUi profileUi;
    [SerializeField] private NickNameSetUi nickNameSetUi;

    [SerializeField] private Button anonyButton;
    [SerializeField] private Button logInButton;
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private Button recordButton;
    [SerializeField] private Button readerBoardButton;

    [SerializeField] private Button GameOverRecordButton;
    [SerializeField] private Button GameOverReaderBoardButton;

    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private ScoreRecordUi scoreRecordUi;

    public Button profileButton;
    public TextMeshProUGUI profileText;

    private async Task Start()
    {
        SetButtonsInteractable(false);

        await UniTask.WaitUntil(() => AuthManager.Instance != null && AuthManager.Instance.IsInitialized);

        anonyButton.onClick.AddListener(() => OnAnonyButtonClicked().Forget());
        logInButton.onClick.AddListener(() => OnlogInButtonClicked().Forget());
        signUpButton.onClick.AddListener(() => OnSignUpButtonClicked().Forget());
        profileButton.onClick.AddListener(() => OnProfileButtonClicked().Forget());
        closeButton.onClick.AddListener(() => OnCloseButtonClicked());
        recordButton.onClick.AddListener(() => OnRecordButtonClicked().Forget());
        GameOverRecordButton.onClick.AddListener(() => OnRecordButtonClicked().Forget());
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
        SetButtonsInteractable(true);
        UpdateUI().Forget();
    }

    public async UniTaskVoid UpdateUI()
    {
        if (AuthManager.Instance == null || !AuthManager.Instance.IsInitialized)
        {
            return;
        }

        bool isLoggedIn = AuthManager.Instance.IsLoggedIn;
        loginPanel.SetActive(!isLoggedIn);

        if (isLoggedIn)
        {
            // 캐시된 프로필이 없으면 로드 시도
            if (ProfileManager.Instance.CachedProfile == null)
            {
                var (profile, error) = await ProfileManager.Instance.LoadProfileAsync("");
                if (profile == null)
                {
                    var sb = new StringBuilder();
                    sb.Clear();
                    sb.Append("닉네임 없음");
                    profileText.text = sb.ToString();
                    return;
                }
            }

            // 이제 캐시된 프로필이 있는지 확인하고 닉네임 표시
            if (ProfileManager.Instance.CachedProfile != null && !string.IsNullOrEmpty(ProfileManager.Instance.CachedProfile.nickname))
            {
                var sb = new StringBuilder();
                sb.Clear();
                sb.Append(ProfileManager.Instance.CachedProfile.nickname);
                profileText.text = sb.ToString();
            }
            else
            {
                var sb = new StringBuilder();
                sb.Clear();
                sb.Append("닉네임 미설정");
                profileText.text = sb.ToString();
            }
        }
        else
        {
            var sb = new StringBuilder();
            sb.Clear();
            sb.Append("닉네임 없음");
            profileText.text = sb.ToString();
        }
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
            await ProfileManager.Instance.LoadProfileAsync("");

            if(ProfileManager.Instance.CachedProfile == null || string.IsNullOrEmpty(ProfileManager.Instance.CachedProfile.nickname))
            {
                nickNameSetUi.gameObject.SetActive(true);
            }
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
            nickNameSetUi.gameObject.SetActive(true);
        }
        SetButtonsInteractable(true);
        UpdateUI().Forget();
    }

    private void SetButtonsInteractable(bool b )
    {
        logInButton.interactable = b;
        signUpButton.interactable= b;
        anonyButton.interactable= b;
    }

    private async UniTaskVoid OnProfileButtonClicked()
    {
        await profileUi.SetTextNickNameText();

        if(AuthManager.Instance.IsLoggedIn)
        {
            profileUi.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("로그인이 되지 않았습니다.");
        }
    }

    private void OnCloseButtonClicked()
    {
        loginPanel.gameObject.SetActive(true);
    }

    private async UniTaskVoid OnRecordButtonClicked()
    {        
        scoreRecordUi.gameObject.SetActive(true);   
    }
    
}
