using UnityEngine;
using System.Collections;

public class MainSceneCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (FindObjectsOfType(typeof(ControllerManager)).Length == 0)
        {
            GameObject go = new GameObject();
            go.AddComponent<ControllerManager>();
            go.name = "InputManager";
        }

        GameManager.Instance.MenuEvent += (bool isOnMenu) => {if (isOnMenu)Bind(); else UnBind(); };
        GameManager.Instance.LaunchMenu();
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
            GameObject objectHit = hit.collider.gameObject;
            if (objectHit.name.Equals("Play"))
            {
                Application.LoadLevel("SceneInGame");
            }
            else if (objectHit.name.Equals("Quit"))
            {
            }
        }
    }
}
