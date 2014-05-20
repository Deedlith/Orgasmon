using UnityEngine;
using System.Collections;

public class SceneInGameCtrl : MonoBehaviour
{

    public GameObject Pause;
    public GameObject RootInGame;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !Pause.activeSelf)
        {
            Pause.SetActive(true);
            RootInGame.SetActive(false);
        }
    }
}
