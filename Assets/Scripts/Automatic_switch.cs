using UnityEngine;

public class AutoSceneTransition : MonoBehaviour
{
    public float delay = 3f;
    public string nextSceneName = "Game";

    private void Start()
    {
        Invoke("LoadNextScene", delay);
    }

    private void LoadNextScene()
    {
        SceneManager.Instance.LoadScene(nextSceneName);
    }
}