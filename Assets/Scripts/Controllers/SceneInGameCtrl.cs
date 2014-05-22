using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInGameCtrl : MonoBehaviour
{

    public GameObject pause;
    public GameObject rootInGame;
    public GameObject initGame;
    GameObject monsterGo = null;
    Monster monster;
    static int nbMonsterSelected = 0;
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

    public void InitGame()
    {
        initGame.SetActive(true);
        rootInGame.SetActive(false);
        Field.Instance.CreateMonsters();
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
        GameManager.Instance.Action2Pressed += InitGameAction;
    }

    void UnBindInit()
    {
        GameManager.Instance.Action1Pressed -= InitGameAction;
        GameManager.Instance.Action2Pressed -= InitGameAction;
    }


    void InitGameAction()
    {
        RaycastHit hit = new RaycastHit();
        GameObject objectHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            objectHit = hit.collider.gameObject;
            if (objectHit.name.Equals("PlayGame"))
            {
                initGame.SetActive(false);
                rootInGame.SetActive(true);
                UnBindInit();
                Field.Instance.Generate();
            }
            else if (objectHit.name.Equals("Regenerate") && nbMonsterSelected == 1 && monster!= null)
            {
                nbMonsterSelected = 0;
                Field.Instance.RegenerateMonster(monster,monsterGo.transform.parent.gameObject);
            }
            else if (objectHit.name.Equals("Fusion") && nbMonsterSelected == 2)
            {
                nbMonsterSelected = 0;
                Field.Instance.Fusion();
            }

            else if (objectHit.name.Contains("Monster_A"))
            {
                monsterGo = objectHit.gameObject;
                monsterGo.transform.parent.transform.FindChild("InfosMonsters").gameObject.SetActive(true);
                Field.Instance.dicoMonsterGOMonster.TryGetValue(monsterGo, out monster);
                if (Input.GetMouseButtonDown(0))
                {
                    if (nbMonsterSelected < 2)
                    {
                        monsterGo.transform.renderer.material.color = Color.red;
                        monster.isSelected = true;
                        nbMonsterSelected++;
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    monsterGo.transform.renderer.material.color = Color.white;
                    monster.isSelected = false;
                    nbMonsterSelected--;
                    monsterGo = null;
                }
            }
        }
        else
        {
            if (monsterGo != null)
            {
                monsterGo.transform.parent.transform.FindChild("InfosMonsters").gameObject.SetActive(false);
            }
        }
    }

    void PauseAction()
    {
        RaycastHit hit = new RaycastHit();
        GameObject objectHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            objectHit = hit.collider.gameObject;
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
