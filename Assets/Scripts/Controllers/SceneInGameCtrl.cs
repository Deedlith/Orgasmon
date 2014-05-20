using UnityEngine;
using System.Collections;

public class SceneInGameCtrl : MonoBehaviour
{

    public GameObject Pause;
    public GameObject RootInGame;

    // Update is called once per frame


    void Start()
    {
        GameManager.Instance.PausePressed += () => { DisplayPause(); };
    }

    void DisplayPause()
    {
        print("HIT PRESSED");
        
        Pause.SetActive(GameManager.Instance.IsInPause);
        RootInGame.SetActive(!GameManager.Instance.IsInPause);
    }



    
    /*void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !Pause.activeSelf)
        {
            Pause.SetActive(true);
            RootInGame.SetActive(false);
        }
    }*/
}
