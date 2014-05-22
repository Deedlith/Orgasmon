using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInGameCtrl : MonoBehaviour
{

    public GameObject pause;
    public GameObject rootInGame;
    public GameObject initGame;

    // Update is called once per frame


    void Start()
    {
        GameManager.Instance.PausePressed += () => { DisplayPause(); };
        GameManager.Instance.PauseEvent += (bool isOnPause) => { if (isOnPause)BindPause(); else UnBindPause(); };

        GameManager.Instance.MenuEvent += (bool isOnMenu) => { if (isOnMenu)BindInit(); else UnBindInit(); };
        GameManager.Instance.LaunchMenu();
    }

    void DisplayPause()
    {
        pause.SetActive(GameManager.Instance.IsInPause);
        rootInGame.SetActive(!GameManager.Instance.IsInPause);
    }

    public void InitGame(List<GameObject> list)
    {
        initGame.SetActive(true);
        rootInGame.SetActive(false);
        int posX = -4, posY = 2, posZ = 4;
        foreach (GameObject m in list)
        {
            m.transform.position = new Vector3(posX, posY, posZ);
            posX += 2;
        }
    }

    void BindPause()
    {
        GameManager.Instance.Action1Pressed += PauseAction;
    }

    void UnBindPause()
    {
        GameManager.Instance.Action1Pressed -= PauseAction;
    }


    void BindInit()
    {
        GameManager.Instance.Action1Pressed += InitGameAction;
    }

    void UnBindInit()
    {
        GameManager.Instance.Action1Pressed -= InitGameAction;
    }


    void InitGameAction()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            GameObject objectHit = hit.collider.gameObject;
            if (objectHit.name.Equals("Play2"))
            {
                initGame.SetActive(false);
                rootInGame.SetActive(true);
                UnBindInit();
            }
        }
    }

    void PauseAction()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            GameObject objectHit = hit.collider.gameObject;
            if (objectHit.name.Equals("Return"))
            {
                pause.SetActive(false);
                rootInGame.SetActive(true);
            }
            else if (objectHit.name.Equals("BackMainMenu"))
            {
                UnBindPause();
                Application.LoadLevel("MainScene");
            }
        }
    }
}
