using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
    }
    
    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
