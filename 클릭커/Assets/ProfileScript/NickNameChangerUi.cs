using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.Text;

public class NickNameChangerUi : MonoBehaviour
{
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI currnetNickNameText;
    [SerializeField] private ProfileUi profileUi;
    [SerializeField] private LogInUI loginUi;

    [SerializeField] private TextMeshProUGUI currentNickNameText;

    private void Start()
    {
        saveButton.onClick.AddListener(() => OnSaveButtonClicked().Forget());
        closeButton.onClick.AddListener(OnCloseButtonClicked);

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        nicknameInputField.text = string.Empty;
        SetCurrnetNickNameText();
    }

    private async UniTaskVoid OnSaveButtonClicked()
    {
        string newNickName = nicknameInputField.text.Trim();

        if (string.IsNullOrEmpty(newNickName))
        {
            return;
        }

        SetButtonsInteractable(false);

        // 프로필이 이미 존재하는지 확인 후 저장 또는 업데이트
        bool profileExists = await ProfileManager.Instance.ProfileExistAsync();

        if (profileExists)
        {
            // 기존 프로필의 닉네임만 업데이트
            await ProfileManager.Instance.UpdateNicknameAsync(newNickName);
        }
        else
        {
            // 새 프로필 생성
            await ProfileManager.Instance.SaveProfileAsync(newNickName);
        }

        // ProfileUi 업데이트
        if (profileUi != null)
        {
            profileUi.SetTextNickNameText().Forget();
        }

        if(loginUi != null)
        {
            loginUi.UpdateUI().Forget();
        }

        gameObject.SetActive(false);
        SetButtonsInteractable(true);
    }

    private void SetCurrnetNickNameText()
    {
        // AuthManager와 ProfileManager 널 체크
        if (AuthManager.Instance == null || ProfileManager.Instance == null || !AuthManager.Instance.IsLoggedIn)
        {
            currnetNickNameText.text = "";
            return;
        }

        // 캐시된 프로필이 있으면 해당 닉네임 표시
        if (ProfileManager.Instance.CachedProfile != null)
        {
            string currentNickname = ProfileManager.Instance.CachedProfile.nickname;

            if (string.IsNullOrEmpty(currentNickname))
            {
                currnetNickNameText.text = "닉네임 미설정";
                currentNickNameText.text = "닉네임 미설정";

            }
            else
            {
                currnetNickNameText.text = $"현재 닉네임: {currentNickname}";
                currentNickNameText.text = currentNickname;
            }
        }
    }
    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
    }

    private void SetButtonsInteractable(bool interactable)
    {
        saveButton.interactable = interactable;
        closeButton.interactable = interactable;
        nicknameInputField.interactable = interactable;
    }
}