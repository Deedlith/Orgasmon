using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

	string _monsterSelected; // monster currently selected by player
	GameObject _currentMonsterGO;
	Monster _currentMonster;

    float duration= 0.9f; // duration of movement in seconds
    bool moving= false; // flag to indicate it's moving

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
            if (objectHit != null && objectHit.name.Contains("Square") && _currentMonsterGO != null)
			{
				print ("SQUARE SELECTED : " + objectHit.name);
                StartCoroutine(MoveMonster(_currentMonsterGO, objectHit));
                _currentMonster = null;
                _currentMonsterGO = null;
                _monsterSelected = null;
				//MoveMonster(_currentMonster, _currentMonsterGO);
			}
            else if (objectHit.name.Contains("A"))
			{
				print ("MONSTER SELECTED : " + objectHit.name);

				_monsterSelected = objectHit.name;
				_currentMonsterGO = GameObject.Find(_monsterSelected);
				_currentMonster = Field.Instance.GetMonsterFromGo(_currentMonsterGO);

				if(_currentMonster != null) 
				{
					Genetic.Instance.DisplayPattern(_currentMonster.listMovements);
					Moves ();
				}
				else print("Error Select");
			}
		}
	}



    IEnumerator MoveMonster(GameObject monsterGO,GameObject end)
    {
        if (moving) yield return new WaitForSeconds(0) ; 

        float distanceX = end.transform.position.x - monsterGO.transform.position.x;
        distanceX = Mathf.Sqrt(distanceX);

        float distanceZ = end.transform.position.z - monsterGO.transform.position.z;
        distanceZ = Mathf.Sqrt(distanceZ);

        //print("monsterGO " + monsterGO.transform.position + " end " + end.transform.position);

        Vector3 initPos = monsterGO.transform.position;
        if (distanceX > distanceZ)
        {
            moving = true; 
            var curPos = initPos;
            var newPos = new Vector3(end.transform.position.x, 0, monsterGO.transform.position.z); 
            for (float t = 0f; t < 1; )
            {
                t += Time.deltaTime / duration;
                monsterGO.transform.position = Vector3.Lerp(curPos, newPos, t);
                yield return new WaitForSeconds(0);
            }
            //print("INTER CHEMIN " + monsterGO.transform.position + " initPos " + initPos + " end " + end.transform.position);
            monsterGO.transform.position = curPos = new Vector3(end.transform.position.x, 0, initPos.z);
            newPos = new Vector3(end.transform.position.x, 0, end.transform.position.z); 

            for (float t = 0f; t < 1; )
            {
                t += Time.deltaTime / duration;
                monsterGO.transform.position = Vector3.Lerp(curPos, newPos, t); 
                yield return new WaitForSeconds(0); 
            }
        }
        else
        {
            moving = true;
            var curPos = initPos;
            var newPos = new Vector3(monsterGO.transform.position.x, 0, end.transform.position.z); 
            for (float t = 0f; t < 1; )
            {
                t += Time.deltaTime / duration;
                monsterGO.transform.position = Vector3.Lerp(curPos, newPos, t); 
                yield return new WaitForSeconds(0); 
            }
            //print("INTER CHEMIN " + monsterGO.transform.position + " initPos " + initPos + " end " + end.transform.position);
            curPos = new Vector3(initPos.x, 0, end.transform.position.z); 
            newPos = new Vector3(end.transform.position.x, 0, end.transform.position.z); 

            for (float t = 0f; t < 1; )
            {
                t += Time.deltaTime / duration; 
                monsterGO.transform.position = Vector3.Lerp(curPos, newPos, t); 
                yield return new WaitForSeconds(0); 
            }
        }
        moving = false;
    }

	public class Move
	{
		public Square currentSquare;
		public List<Move> child;
	}

	public void Moves()
	{
		int index = 0;
		Move currentMove = new Move();

		if(_currentMonster != null)
		{
			currentMove.currentSquare = _currentMonster.currentSquare;
			currentMove.child = new List<Move>();

			if(_currentMonster.listMovements[index] == Movement.Vertical)
			{
				Square sf = Field.Instance.ListSquares.Where(ls => ((ls.PositionX == currentMove.currentSquare.PositionX) && (ls.PositionZ == currentMove.currentSquare.PositionZ + 1))).FirstOrDefault();
				Square sb = Field.Instance.ListSquares.Where(ls => ((ls.PositionX == currentMove.currentSquare.PositionX) && (ls.PositionZ == currentMove.currentSquare.PositionZ - 1))).FirstOrDefault();

				if(sf != null) 
				{
					Move tempMove = new Move();
					tempMove.currentSquare = sf;
					tempMove.child = new List<Move>();
					currentMove.child.Add(tempMove);
				}

				if(sb != null)
				{
					Move tempMove = new Move();
					tempMove.currentSquare = sb;
					tempMove.child = new List<Move>();
					currentMove.child.Add(tempMove);
				}
			}
			else if(_currentMonster.listMovements[index] == Movement.Horizontal)
			{
				Square sr = Field.Instance.ListSquares.Where(ls => ((ls.PositionX == currentMove.currentSquare.PositionX + 1) && (ls.PositionZ == currentMove.currentSquare.PositionZ))).FirstOrDefault();
				Square sl = Field.Instance.ListSquares.Where(ls => ((ls.PositionX == currentMove.currentSquare.PositionX - 1) && (ls.PositionZ == currentMove.currentSquare.PositionZ))).FirstOrDefault();
				
				if(sr != null) 
				{
					Move tempMove = new Move();
					tempMove.currentSquare = sr;
					tempMove.child = new List<Move>();
					currentMove.child.Add(tempMove);
				}
				
				if(sl != null)
				{
					Move tempMove = new Move();
					tempMove.currentSquare = sl;
					tempMove.child = new List<Move>();
					currentMove.child.Add(tempMove);
				}
			}

			SearchMove(index, _currentMonster.listMovements.Count(), currentMove.child);

			List<Move> ListMoves = new List<Move>();
			ListMoves.Add(currentMove);

			DisplayClear();

			DisplayMove(ListMoves);
		}
	}

	public void DisplayClear()
	{
		for(int x = 0; x < 10; x++)
		{
			for(int z = 0; z < 10; z++)
			{
				GameObject square = Field.Instance.ListSquaresGo.Where(ls => ((ls.gameObject.transform.position.x == x) && (ls.gameObject.transform.position.z == z))).FirstOrDefault();
				
				if(square != null)
				{
					square.renderer.material.color = new Color(255, 255, 255,1);
				}
			}
		}
	}

	public void DisplayMove(List<Move> moves)
	{
		if(moves != null && moves.Count() > 0)
		{
			foreach(Move move in moves)
			{
				GameObject square = Field.Instance.ListSquaresGo.Where(ls => ((ls.gameObject.transform.position.x == move.currentSquare.PositionX) && (ls.gameObject.transform.position.z == move.currentSquare.PositionZ))).FirstOrDefault();
				
				if(square != null)
				{
					square.renderer.material.color = new Color(128,0,0,1);
				}
				
				DisplayMove(move.child);
			}
		}
	}

	public void SearchMove(int index, int counterMove, List<Move> possibilityMoves)
	{
		index += 1;

		if(index < counterMove)
		{
			foreach(Move move in possibilityMoves)
			{								
				if(_currentMonster.listMovements[index] == Movement.Vertical)
				{
					Square sf = Field.Instance.ListSquares.Where(ls => ((ls.PositionX == move.currentSquare.PositionX) && (ls.PositionZ == move.currentSquare.PositionZ + 1))).FirstOrDefault();
					Square sb = Field.Instance.ListSquares.Where(ls => ((ls.PositionX == move.currentSquare.PositionX) && (ls.PositionZ == move.currentSquare.PositionZ - 1))).FirstOrDefault();
					
					if(sf != null) 
					{
						Move tempMove = new Move();
						tempMove.currentSquare = sf;
						tempMove.child = new List<Move>();
						move.child.Add(tempMove);
					}
					
					if(sb != null)
					{
						Move tempMove = new Move();
						tempMove.currentSquare = sb;
						tempMove.child = new List<Move>();
						move.child.Add(tempMove);
					}
				}
				else if(_currentMonster.listMovements[index] == Movement.Horizontal)
				{
					Square sr = Field.Instance.ListSquares.Where(ls => ((ls.PositionX == move.currentSquare.PositionX + 1) && (ls.PositionZ == move.currentSquare.PositionZ))).FirstOrDefault();
					Square sl = Field.Instance.ListSquares.Where(ls => ((ls.PositionX == move.currentSquare.PositionX - 1) && (ls.PositionZ == move.currentSquare.PositionZ))).FirstOrDefault();
					
					if(sr != null) 
					{
						Move tempMove = new Move();
						tempMove.currentSquare = sr;
						tempMove.child = new List<Move>();
						move.child.Add(tempMove);
					}
					
					if(sl != null)
					{
						Move tempMove = new Move();
						tempMove.currentSquare = sl;
						tempMove.child = new List<Move>();
						move.child.Add(tempMove);
					}
				}

				SearchMove(index, _currentMonster.listMovements.Count(), move.child);
			}
		}			
	}

	/*void MoveMonster(Monster monster, GameObject monsterGO)
	{
		monster.listMovements.Add (Movement.Horizontal);
		monster.listMovements.Add (Movement.Vertical);
		monster.listMovements.Add (Movement.Horizontal);
		monster.listMovements.Add (Movement.Vertical);
		foreach (Movement movement in monster.listMovements)
		{
			// Pas possible d'enchainer les mouvements avec une coroutine TTT_TTT
			StartCoroutine(MoveMonsterOneSquare(0.5f, monsterGO, movement));		
		}
	}*/

	/*IEnumerator MoveMonsterOneSquare(float delayTime, GameObject monsterGO, Movement movement)
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
	}*/

	bool CanMove(int x, int z)
	{
		return true;
	}
}
