using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ReaderBoardItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nickNameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public async UniTask SetText(string nickName, int score)
    {
        nickNameText.text = nickName;
        scoreText.text = score.ToString();
    }
}
