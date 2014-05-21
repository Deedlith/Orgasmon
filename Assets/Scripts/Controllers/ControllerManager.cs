using UnityEngine;
using System.Collections;

public class ControllerManager : MonoBehaviour
{
	public KeyCode _action1 = KeyCode.Mouse0;
	public KeyCode _action2 = KeyCode.Mouse1;
	public KeyCode _pause = KeyCode.P;
	
	delegate bool HasActionPressed();
	HasActionPressed _hasAction1Pressed;
	HasActionPressed _hasAction2Pressed;
	HasActionPressed _hasPausePressed;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
	// Use this for initialization
	void Start ()
	{
		_hasAction1Pressed = () => { if(Input.GetKeyDown(_action1)) return true; else return false; };
		_hasAction2Pressed = () => { if(Input.GetKeyDown(_action2)) return true; else return false; };
		_hasPausePressed = () => { if(Input.GetKeyDown(_pause)) return true; else return false; };
	}

	
	// Update is called once per frame
	void Update () {
		if(_hasAction1Pressed())
			GameManager.Instance.ActionHandler(Action.Action1);
		else if(_hasAction2Pressed())
			GameManager.Instance.ActionHandler(Action.Action2);
		else if(_hasPausePressed())
			GameManager.Instance.ActionHandler(Action.Pause);
	}
}
