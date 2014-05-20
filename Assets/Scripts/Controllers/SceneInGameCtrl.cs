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
        GameManager.Instance.PauseEvent += (bool isOnPause) => { if (isOnPause)Bind(); else UnBind(); };
    }

    void DisplayPause()
    {
        print("HIT PRESSED");
        
        Pause.SetActive(GameManager.Instance.IsInPause);
        RootInGame.SetActive(!GameManager.Instance.IsInPause);
    }

    void Bind()
    {
        GameManager.Instance.Action1Pressed += PauseAction;
    }

    void UnBind()
    {
        GameManager.Instance.Action1Pressed -= PauseAction;
    }

    void PauseAction()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            Debug.Log("Je rentre");
            GameObject objectHit = hit.collider.gameObject;
            if (objectHit.name.Equals("Return"))
            {
                Pause.SetActive(false);
                RootInGame.SetActive(true);
            }
            else if (objectHit.name.Equals("BackMainMenu"))
            {
                UnBind();
                Application.LoadLevel("MainScene");
            }
        }
        else
        {
            Debug.Log("Je rentre pas ");
        }
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
