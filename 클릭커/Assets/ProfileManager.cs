using System.Linq.Expressions;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Firebase.Database;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    private static ProfileManager instance;
    public static ProfileManager Instance => instance;

    private DatabaseReference databaseRef;
    private DatabaseReference usersRef;

    private UserProfile cachedProfile;
    public UserProfile CachedProfile => cachedProfile;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {

        }
    }

    private async UniTaskVoid Start()
    {
        await FireBaseInitializer.Instance.WaitForInitializationAsync();

        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        usersRef = databaseRef.Child("users");

        Debug.Log("[Profile] ProfileManager �ʱ�ȭ �Ϸ�");
    }

    public async UniTask<(bool success, string error)> SaveProfileAsync(string nickname)
    {
        if (!AuthManager.Instance.IsLoggedIn)
        {
            return (false, "[Profile] �α��� X");
        }

        string userId = AuthManager.Instance.UserId;
        string email = AuthManager.Instance.CurrentUser.Email ?? "�͸�";

        try
        {
            Debug.Log($"[Profile] ������ ���� �õ� {nickname}");

            UserProfile profile = new UserProfile(nickname, email);
            string json = profile.ToJson();

            await usersRef.Child(userId).SetRawJsonValueAsync(json).AsUniTask();

            cachedProfile = profile;

            Debug.Log($"[Profile] ������ ���� {nickname}");
            return (true, null);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Profile] ������ ���� {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async UniTask<(UserProfile profile, string error)> LoadProfileAsync(string nickname)
    {
        if (!AuthManager.Instance.IsLoggedIn)
        {
            return (null, "�α��� X");
        }

        string userId = AuthManager.Instance.UserId;

        try
        {
            Debug.Log($"[Profile] ������ �ε� �õ� {nickname}");
            DataSnapshot snapshot = await usersRef.Child(userId).GetValueAsync().AsUniTask();

            if (!snapshot.Exists)
            {
                Debug.Log($"[Profile] ������ ����");
                return (null, "������ ����");
            }

            string json = snapshot.GetRawJsonValue();
            cachedProfile = UserProfile.FromJson(json);

            Debug.Log($"[Profile] ������ ���� {nickname}");
            return (cachedProfile, null);
        }

        catch (System.Exception ex)
        {
            Debug.LogError($"[Profile] ������ ���� {ex.Message}");
            return (null, ex.Message);
        }
    }

    public async UniTask<(bool success, string error)> UpdateNicknameAsync(string newNickname)
    {
        if (!AuthManager.Instance.IsLoggedIn)
        {
            return (false, "�α��� X");
        }

        string userId = AuthManager.Instance.UserId;

        try
        {
            Debug.Log($"[Profile] �г��� ���� �õ� {newNickname}");

            await usersRef.Child(userId).Child("nickname").SetValueAsync(newNickname).AsUniTask();

            cachedProfile.nickname = newNickname;

            Debug.Log($"[Profile] �г��� ���� ���� {cachedProfile.nickname}");
            return (true, null);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Profile] �г��� ���� ���� {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async UniTask<bool> ProfileExistAsync()
    {
        if (!AuthManager.Instance.IsLoggedIn)
        {
            return false;
        }

        string userId = AuthManager.Instance.UserId;

        try
        {
            DataSnapshot snapshot = await usersRef.Child(userId).GetValueAsync().AsUniTask();
            return snapshot.Exists;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Profile] ������ Ȯ�� ����: {ex.Message}");
            return false;
        }
    }
}
