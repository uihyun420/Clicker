using System;
using UnityEngine;

[Serializable]
public class ScoreData
{
    public int score;
    public long timestamp;

    public ScoreData()
    {

    }

    public ScoreData(int score, long timestamp)
    {
        this.score = score;
        this.timestamp = timestamp;
    }

    public DateTime GetDateTime()
    {
        try
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).LocalDateTime;
        }
        catch (ArgumentOutOfRangeException)
        {
            // 잘못된 timestamp 값인 경우 현재 시간 반환
            Debug.LogWarning($"잘못된 timestamp 값: {timestamp}. 현재 시간을 사용합니다.");
            return DateTime.Now;
        }
    }

    public string GetDateString()
    {
        return GetDateTime().ToString("yyyy-MM-dd HH:mm:ss");
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static ScoreData FromJson(string json)
    {
        return JsonUtility.FromJson<ScoreData>(json);
    }
}
