using UnityEngine;
using Firebase;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem.iOS;

public class FireBaseInitializer : MonoBehaviour
{
    private static FireBaseInitializer instance;
    public static FireBaseInitializer Instance => instance;

    private bool isInitialized = false;
    private bool IsInitialized => isInitialized;

    private FirebaseApp firebaseApp;
    public FirebaseApp FirebaseApp => firebaseApp;


    private void Awake()
    {        
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeFireBaseAsync().Forget();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null; 
        }
    }

    private async UniTaskVoid InitializeFireBaseAsync()
    {
        Debug.Log("[FireBase] 초기화 시작");

        try
        {
            var status =  await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
            if(status == DependencyStatus.Available) // 초기화 성공 했을때 
            {
                firebaseApp = FirebaseApp.DefaultInstance;
                isInitialized = true;
                Debug.Log($"[FireBase] 초기화 성공 {firebaseApp.Name}");
            }
            else
            {
                Debug.Log($"[FireBase] 의존성 오류 : {status}");
                isInitialized = false;
            }
        }
        catch(System.Exception ex)
        {
            Debug.LogError($"[FireBase] 초기화 오류 : {ex.Message}");
            isInitialized = false;
        }

    }

    public async UniTask WaitForInitializationAsync()
    {
        await UniTask.WaitUntil(() => isInitialized);
    }

}
