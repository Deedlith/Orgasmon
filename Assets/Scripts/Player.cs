using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

	string _monsterSelected; // monster currently selected by player

	void Awake()
	{

	}
	
	// Use this for initialization
	void Start () {
		GameManager.Instance.LevelEvent += (bool isOnLevel) => { if(isOnLevel == true) BindMove(); else UnbindMove(); };
	}
	
	void BindMove()
	{
		GameManager.Instance.Action1Pressed += MoveAction;
	}
	
	void UnbindMove()
	{
		GameManager.Instance.Action1Pressed -= MoveAction;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void MoveAction()
	{
		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// Sélectionner un monstre
		// Lui afficher son pattern de déplacement
		// Si un monstre est sélectionné et qu'il clique sur une des cases, le déplacer sur cette case
		// Je te laisse le code en dessous exemple
		if(Physics.Raycast(ray, out hit, 1000)) 
		{
			GameObject objectHit = hit.collider.gameObject;
			if(objectHit.name.Substring(0, 6).Equals("Square"))
			{
				print ("SQUARE SELECTED : " + objectHit.name);

				GameObject currentMonsterGO = GameObject.Find(_monsterSelected);
				Monster currentMonster = Field.Instance.GetMonsterFromGo(currentMonsterGO);
				MoveMonster(currentMonster, currentMonsterGO);
			}
			else if(objectHit.name.Substring(0, 5).Equals("TeamA"))
			{
				print ("MONSTER SELECTED : " + objectHit.name);
				StopAllCoroutines();
				_monsterSelected = objectHit.name;
			}
		}
	}

	void MoveMonster(Monster monster, GameObject monsterGO)
	{
		foreach (Movement movement in monster.listMovements)
		{
			StartCoroutine(MoveMonsterOneSquare(0.5f, monsterGO, movement));		
		}
	}

	IEnumerator MoveMonsterOneSquare(float delayTime, GameObject monsterGO, Movement movement)
	{
		print ("Moving monster...");
		
		Vector3 start_position = monsterGO.transform.position;
		Vector3 end_position = monsterGO.transform.position;

		if (movement == Movement.Vertical)
			end_position = new Vector3 (start_position.x, start_position.y, start_position.z++);

		if (movement == Movement.Horizontal)
			end_position = new Vector3 (start_position.x++, start_position.y, start_position.z);
		
		yield return new WaitForSeconds(delayTime);
		float startTime = Time.time; // Time.time contains current frame time, so remember starting point
		while (Time.time-startTime <= 1)
		{
			monsterGO.transform.position = Vector3.Lerp(end_position, start_position, Time.time-startTime); // lerp from A to B in one second
			yield return 1; // wait for next frame
		}
	}

	bool CanMove(int x, int z)
	{
		return true;
		/*if (onDice == false)
			return false;
		
		var nextSquare = Level.Instance._field.ListSquares.Where(s => s.PositionX == x && s.PositionZ == z).FirstOrDefault();
		
		if(nextSquare == null || !nextSquare.SquareIsExisting())
		{
			return false;
		}
		
		if(this.onDice.actualSquare != nextSquare)
			return false;
		
		return true;*/
	}
}
