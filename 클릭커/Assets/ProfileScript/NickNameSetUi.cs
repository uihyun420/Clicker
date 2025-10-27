using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.Text;

public class NickNameSetUi : MonoBehaviour
{
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private Button saveButton;
    [SerializeField] private ProfileUi profileUi;

    [SerializeField] private LogInUI loginUi;

    private void Start()
    {
        saveButton.onClick.AddListener(() => OnSaveButtonClicked().Forget());
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        nicknameInputField.text = string.Empty;
    }

    private async UniTaskVoid OnSaveButtonClicked()
    {
        string newNickName = nicknameInputField.text.Trim();

        if (string.IsNullOrEmpty(newNickName))
        {
            return;
        }

        SetButtonsInteractable(false);

        // 첫 프로필 생성 
        var (success, error) = await ProfileManager.Instance.SaveProfileAsync(newNickName);

        if (success)
        {
            Debug.Log($"[NickNameSet] 첫 프로필 생성 성공: {newNickName}");

            // ProfileUi 업데이트
            if (profileUi != null)
            {
                profileUi.SetTextNickNameText().Forget();
            }

            if (loginUi != null)
            {
                loginUi.UpdateUI().Forget();
            }
            gameObject.SetActive(false);
            profileUi.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError($"[NickNameSet] 프로필 생성 실패: {error}");
            SetButtonsInteractable(true);
        }
    }

    private void SetButtonsInteractable(bool interactable)
    {
        saveButton.interactable = interactable;
        nicknameInputField.interactable = interactable;
    }
}
