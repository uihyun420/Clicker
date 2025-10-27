using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.Text;
using System;

public class RecordScoreItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void SetScoreData(ScoreData scoreData)
    {
        DateTime dateTime = scoreData.GetDateTime();
        string formattedDate = $"{dateTime.Year} - {dateTime.Month:00} - {dateTime.Day:00} {dateTime.Hour:00}:{dateTime.Minute:00}:{dateTime.Second:00}";
        scoreText.text = $"{scoreData.score}Á¡ - {formattedDate}";
    }

    public void SetScoreData(int score)
    {
        DateTime now = DateTime.Now;
        string formattedDate = $"{now.Year} - {now.Month:00} - {now.Day:00} {now.Hour:00}:{now.Minute:00}:{now.Second:00}";
        scoreText.text = $"{score}Á¡ - {formattedDate}";
    }
}
