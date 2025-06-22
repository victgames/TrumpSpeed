using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearScene : MonoBehaviour
{
    // Startボタンに割り当てる
    public void OnNextButton()
    {
        SceneManager.LoadScene("StartScene"); // シーン名で遷移
    }
}
