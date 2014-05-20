using UnityEngine;
using System.Collections;

#region PUBLIC ENUM
public enum Action
{
	Up,
	Down,
	Left,
	Right,
	Action1,
	Action2,
	Pause
}
#endregion

#region EVENT HANDLER
public delegate void LevelEventHandler(bool isOnLevel);
public delegate void PauseEventHandler(bool isOnPause);
public delegate void MenuEventHandler(bool isOnMenu);
public delegate void GuiEventHandler(bool hasToDraw);
public delegate void ActionEventHandler();
#endregion

public class GameManager
{
	#region EVENT
	public event LevelEventHandler LevelEvent;
	public event PauseEventHandler PauseEvent;
	public event MenuEventHandler MenuEvent;
	public event GuiEventHandler GuiEvent;
	public event GuiEventHandler RefreshGuiEvent;
	public event GuiEventHandler GuiEndGame;

	public event ActionEventHandler Action1Pressed;
	public event ActionEventHandler Action2Pressed;
    public event ActionEventHandler PausePressed;
	#endregion
	
	#region PUBLIC PROPERTIES
	public bool IsInLevel { get; set; }
	public bool IsInMenu { get; set; }
	public bool IsInPause { get; set; }
	#endregion
	
	#region SINGLETON
	private static GameManager _instance = null;
	
	public static GameManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameManager();
			}
			
			return _instance;
		}
	}
	#endregion
	
	private bool _hasWin = false;
	
	#region PUBLIC METHODS
	public void LaunchLevel()
	{
		MonoBehaviour.print("LAUNCH LEVEL");
		this.IsInLevel = true;
		this.IsInMenu = false;
		this.IsInPause = false;
		LevelEvent(this.IsInLevel);
	}
	
	public void PauseLevel()
	{
		this.IsInLevel = false;
		this.IsInMenu = false;
		this.IsInPause = true;
		LevelEvent(this.IsInLevel);
	}
	
	public void ResumeLevel()
	{
		if(this.IsInLevel)
			return;
		
		this.IsInLevel = true;
		this.IsInMenu = false;
		this.IsInPause = false;
		LevelEvent(this.IsInLevel);
	}
	
	public void QuitLevel()
	{
		if(!this.IsInLevel)
			return;
		
		this.IsInLevel = false;
		this.IsInMenu = true;
		this.IsInPause = false;
		LevelEvent(this.IsInLevel);
	}
	
	public void EndGame(bool hasWin)
	{
		MonoBehaviour.print ("ENDGAME " + hasWin);
		this.IsInLevel = false;
		LevelEvent(this.IsInLevel);
		_hasWin = hasWin;
		GuiEndGame(true);
	}
	
	public void RefreshGui()
	{
		if(!this.IsInLevel && (this.IsInPause || this.IsInMenu))
			return;
		
		RefreshGuiEvent(true);
	}
	#endregion
	
	#region ACTION METHODS
	public void ActionHandler(Action which)
	{
        Debug.Log("COUCOU");
        switch(which)
		{
			/*case Action.Action1 :
				Action1Pressed();
				break;
			case Action.Action2 :
				Action2Pressed();
				break;*/
			case Action.Pause :
                this.IsInPause = true;
				PausePressed();
				break;
		}
	}
	
	#endregion
}
