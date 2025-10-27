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
            gameObject.SetActive(false);
            profileUi.gameObject.SetActive(true);
        }
        else
        {
            // 실패해도 창은 닫지 않고 다시 시도할 수 있게 함
            SetButtonsInteractable(true);
        }
    }

    private void SetButtonsInteractable(bool interactable)
    {
        saveButton.interactable = interactable;
        nicknameInputField.interactable = interactable;
    }
}
