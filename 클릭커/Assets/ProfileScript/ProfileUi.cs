using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Text;

public class ProfileUi : MonoBehaviour
{
    [SerializeField] private LogInUI loginUi;
    [SerializeField] private NickNameChangerUi nickNameChangerUi;

    [SerializeField] private Button nickNameEditButton;
    [SerializeField] private Button logOutButton;
    [SerializeField] private Button CloseButton;

    [SerializeField] private TextMeshProUGUI nickNameText;
    [SerializeField] private TextMeshProUGUI userIdText;
    private void Start()
    {
        logOutButton.onClick.AddListener(() => OnLogOutButtonClicked());
        nickNameEditButton.onClick.AddListener(() => OnNickNameEditButtonClicked());
        CloseButton.onClick.AddListener(() => OnCloseButtonClicked());

    }
    private void OnEnable()
    {
        SetTextNickNameText().Forget();
    }

    private void OnNickNameEditButtonClicked()
    {
        if (nickNameChangerUi != null)
        {
            nickNameChangerUi.gameObject.SetActive(true);
        }
    }

    private void OnLogOutButtonClicked()
    {
        AuthManager.Instance.SignOut();
        loginUi.UpdateUI().Forget();
        gameObject.SetActive(false);
    }

    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
    }

    public async UniTaskVoid SetTextNickNameText()
    {
        // AuthManager 초기화 대기
        await UniTask.WaitUntil(() => AuthManager.Instance != null && AuthManager.Instance.IsInitialized);

        var sb = new StringBuilder();
        if(!AuthManager.Instance.IsLoggedIn)
        {
            sb.Clear();
            sb.Append("로그인 필요");
            nickNameText.text = sb.ToString();
            return;
        }

        userIdText.text = AuthManager.Instance.UserId;

        if(ProfileManager.Instance.CachedProfile != null)
        {
            Debug.Log($"[ProfileUI] 캐시된 프로필 닉네임: '{ProfileManager.Instance.CachedProfile.nickname}'");
            nickNameText.text = ProfileManager.Instance.CachedProfile.nickname;
            return;
        }

        // 캐시된 프로필이 없으면 Firebase에서 로드
        var (profile, error) = await ProfileManager.Instance.LoadProfileAsync("");
        if (profile != null)
        {
            if (string.IsNullOrEmpty(profile.nickname))
            {
                nickNameText.text = "닉네임 미설정";
            }
            else
            {
                nickNameText.text = profile.nickname;
            }
        }
        else
        {
            sb.Clear();
            sb.Append("닉네임 없음");
            Debug.LogWarning($"프로필 로드 실패: {error}");
        }
    }
}
