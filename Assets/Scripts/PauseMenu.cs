using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool pause = false;
    [SerializeField] GameObject pauseMenu;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Put_pause();
        }
    }
    
    void Put_pause()
    {
        if (pause){
            Continue();
        } else {
            pause = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Continue()
    {
        pause = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
