using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Firebase.Auth;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.Text;
using NUnit.Framework;

public class ScoreRecordUi : MonoBehaviour
{
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button closeButton;    

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject recordPrefab;
    [SerializeField] private Transform recordParent;

    [SerializeField] private MovingCircle movingCircle;
    private List<GameObject> recordItems = new List<GameObject>();


    private void Start()        
    {
        closeButton.onClick.AddListener(() => OnCloseButtonClicked());
        refreshButton.onClick.AddListener(() => RefreshHistory());

    }
    private void OnEnable()
    {
        SetScoreText();
        LoadScoreHistoryAsync().Forget();
    }

    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);    
    }

    private void SetScoreText()
    {
        if(movingCircle != null && movingCircle.finalScoreText != null)
        {
            scoreText.text = movingCircle.finalScoreText.text;
        }
        else
        {
            var sb = new StringBuilder();
            sb.Clear();
            sb.Append("점수 없음");
            scoreText.text = sb.ToString();
        }
    }

    private async UniTaskVoid LoadScoreHistoryAsync()
    {
        // null 체크 추가
        if (AuthManager.Instance == null)
        {
            Debug.LogError("[ScoreRecordUi] AuthManager.Instance가 null입니다.");
            return;
        }

        if (ScoreManager.Instance == null)
        {
            Debug.LogError("[ScoreRecordUi] ScoreManager.Instance가 null입니다.");
            return;
        }

        if(!AuthManager.Instance.IsLoggedIn)
        {
            Debug.Log("[ScoreRecordUi] 로그인이 필요합니다.");
            return;
        }

        try
        {
            // 기존 기록 아이템들 제거
            ClearRecordItems();

            List<ScoreData> history = await ScoreManager.Instance.LoadHistoryAsync(10);
            if(history.Count == 0)
            {
                Debug.Log("[ScoreRecordUi] 저장된 기록이 없습니다.");
                return;
            }
            foreach(var scoreData in history)
            {
                CreateRecordItem(scoreData);
            }

        }
        catch(System.Exception ex)
        {
            Debug.LogError($"[ScoreRecordUi] 히스토리 로드 실패: {ex.Message}");
        }
    }

    private void CreateRecordItem(ScoreData scoreData)
    {
        if (recordPrefab == null || recordParent == null)
        {
            return;
        }
        if (scoreData == null)
        {
            return;
        }
        try
        {
            var dateTime = scoreData.GetDateTime();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[ScoreRecordUi] DateTime 변환 실패: {ex.Message}");
            return;
        }
        GameObject recordItem = Instantiate(recordPrefab, recordParent);
        RecordScoreItem scoreItem = recordItem.GetComponent<RecordScoreItem>();
        if (scoreItem != null)
        {
            scoreItem.SetScoreData(scoreData);
        }
        else
        {
            Debug.LogError("[ScoreRecordUi] RecordScoreItem 컴포넌트를 찾을 수 없습니다.");
        }
        recordItems.Add(recordItem);
    }

    private void ClearRecordItems()
    {
        foreach (GameObject item in recordItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        recordItems.Clear();
    }

    private void RefreshHistory()
    {
        LoadScoreHistoryAsync().Forget();
    }
}
