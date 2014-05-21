﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Field : MonoBehaviour
{
	//public Dice diceSelected = null;
	
	public List<Square> ListSquares = new List<Square>();
	public List<GameObject> ListSquaresGo = new List<GameObject>();
	public List<Monster> ListMonsters = new List<Monster>();
	
	void Start () {
		for(int i = 0; i < 10; i++)
		{
			for(int j = 0; j < 10; j++)
			{
				Square s = new Square(i,j);
				ListSquares.Add(s);
			}
		}

		Generate();

		GameManager.Instance.currentTeamTurn = Monster.Team.A;
		GameManager.Instance.LaunchLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Generate()
	{
		int squareCounter = 0;
		foreach(var square in ListSquares)
		{
			Vector3 pos = new Vector3(square.PositionX, 0.0f, square.PositionZ);
			
			GameObject squareGo = (GameObject) Instantiate(Resources.Load("Prefabs/Prefab_Square"), pos, Quaternion.identity);
            squareGo.transform.parent = this.transform.parent;
			squareGo.name = "Square_" + squareCounter++;
			ListSquaresGo.Add(squareGo);
		}
	}

	public void CheckEnnemiesPosition()
	{
		Monster currentMonster = ListMonsters.Where(lm => (lm.whichTeam.Equals(GameManager.Instance.currenTeamTurn) && (lm.isSelected == true))).FirstOrDefault();

		if(currentMonster != null)
		{
			List<Monster> localEnnemies = ListMonsters.Where(lm => (
																	!(lm.whichTeam.Equals(currentMonster.whichTeam)) 
																	&& ((lm.currentSquare.PositionX == currentMonster.currentSquare.PositionX + 1 && lm.currentSquare.PositionZ == currentMonster.currentSquare.PositionZ) 
																		|| (lm.currentSquare.PositionX == currentMonster.currentSquare.PositionX - 1 && lm.currentSquare.PositionZ == currentMonster.currentSquare.PositionZ) 
																		|| (lm.currentSquare.PositionX == currentMonster.currentSquare.PositionX && lm.currentSquare.PositionZ == currentMonster.currentSquare.PositionZ + 1) 
																		|| (lm.currentSquare.PositionX == currentMonster.currentSquare.PositionX && lm.currentSquare.PositionZ == currentMonster.currentSquare.PositionZ - 1) 
			    											))).ToList();
			if(localEnnemies.Count() == 1)
			{
				currentMonster.LaunchAttack(localEnnemies[0]);
			}
			else if(localEnnemies.Count() > 1)
			{
				int index = Random.Range(0, localEnnemies.Count());

				currentMonster.LaunchAttack(localEnnemies[index]);
			}

		}
	}

	/* Ca peut servir pour savoir si un combat doit s'enclencher. A adapter bien sur */
	/*public bool DicesHasNeighbor()
	{
		print ("HAS NEIGHBOR");
		foreach(var dice in ListDicesGo)
		{
			Vector3 pos = dice.transform.position;
			var list = ListDicesGo.Where(d => d.transform.position != pos).ToList();
			foreach(var d in list)
			{
				var dpos = d.transform.position;
				if(pos.x == dpos.x && (pos.z != (dpos.z+1) && pos.z != (dpos.z-1)))
					return false;
				else if(pos.z == dpos.z && ((pos.x+1) != dpos.x && (pos.x-1) != dpos.x))
					return false;
			}
		}
		return true;
	}*/
}
