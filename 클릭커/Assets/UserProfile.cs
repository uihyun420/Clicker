using System;
using UnityEngine;
[Serializable]
public class UserProfile
{
    public string nickname;
    public string email;
    public long createdAt;

    public UserProfile()
    {

    }
    public UserProfile(string nickname, string email)
    {
        this.nickname = nickname;
        this.email = email;       
        createdAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
    public string ToJson()
    {
        return JsonUtility.ToJson(this); // ���̽� ���Ϸ� �ٲ���
    }
    public static UserProfile FromJson(string json) // ���̽� ������ȭ
    {
        return JsonUtility.FromJson<UserProfile>(json); 
    }
}
