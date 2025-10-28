using UnityEngine;
using UnityEngine.UI;

public class ReaderBoardUi : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button refreshButton;



    private void Start()
    {
        closeButton.onClick.AddListener(() => OnCloseButtonClicked()); 
    }

    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
