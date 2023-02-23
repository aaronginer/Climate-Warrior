using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseScript : MonoBehaviour
{
    // Start is called before the first frame update
    static public PauseScript instance;

    public string backTo;
    public bool gamePaused = false;

    public GameObject menueContainer;
    public GameObject controlsLayout;
    public GameObject taskDescription;

    void Awake()
    {
        if (instance == null)
        {
            menueContainer.SetActive(gamePaused);
            taskDescription.SetActive(gamePaused);
            controlsLayout.SetActive(gamePaused);
            instance = this;
        }
    }

    public void ReturnToVillage()
    {
        SceneManager.LoadScene(backTo);
    }

    public void ShowContols()
    {
        menueContainer.SetActive(false);
        controlsLayout.SetActive(true);
        taskDescription.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;

            menueContainer.SetActive(gamePaused);
            controlsLayout.SetActive(false);
            taskDescription.SetActive(false);            
        }
    }
}
