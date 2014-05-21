using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Field : MonoBehaviour
{
	//public Dice diceSelected = null;
	
	public List<Square> ListSquares = new List<Square>();
	public List<GameObject> ListSquaresGo = new List<GameObject>();
	public List<Monster> ListMonsters = new List<Monster>();
    public List<GameObject> ListMonstersGo = new List<GameObject>();
	
	void Start () {
		for(int i = 0; i < 10; i++)
		{
			for(int j = 0; j < 10; j++)
			{
				Square s = new Square(i,j);
				ListSquares.Add(s);
			}
		}

        for (int i = 0; i < 5; i++)
        {
            Monster m = new Monster();
            m.level = i + 1;
            m.pv = 2 * m.level;
            // Attack Pattern
            m.listAttackPatterns = new List<AttackPattern>();
            AttackPattern attack = new AttackPattern();
            attack.atk = Attack.Arms;
            attack.power = m.level * 2;
            m.listAttackPatterns.Add(attack);
            // Defense Pattern
            m.listDefensePatterns = new List<DefensePattern>();
            DefensePattern defense = new DefensePattern();
            defense.def = Shield.Arms;
            defense.power = m.level * 2;
            m.listDefensePatterns.Add(defense);
            // Movement Pattern
            m.listMovements = new List<Movement>();
            m.listMovements.Add(Movement.Vertical);
            m.speed = m.level * 3;
            m.whicTeam = Team.A;
            ListMonsters.Add(m);

            m = new Monster();
            m.level = i + 1;
            // Attack Pattern
            m.listAttackPatterns = new List<AttackPattern>();
            attack = new AttackPattern();
            attack.atk = Attack.Arms;
            attack.power = m.level * 2;
            m.listAttackPatterns.Add(attack);
            // Defense Pattern
            m.listDefensePatterns = new List<DefensePattern>();
            defense = new DefensePattern();
            defense.def = Shield.Arms;
            defense.power = m.level * 2;
            m.listDefensePatterns.Add(defense);
            // Movement Pattern
            m.listMovements = new List<Movement>();
            m.listMovements.Add(Movement.Vertical);
            m.pv = 2 * m.level;
            m.speed = m.level * 3;
            m.whicTeam = Team.B;
            ListMonsters.Add(m);
        }

		Generate();

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

        int monsterCounter = 0;
        foreach (var monster in ListMonsters)
        {
            //Récupère le square min en z pour la Team A et le max pour la Team B et le x current
            Square s;
            if (monster.whicTeam == Team.A)
            {
                s = ListSquares.Where(p => p.PositionX == monsterCounter && p.PositionZ == ListSquares.Min(z => z.PositionZ)).FirstOrDefault();
            }
            else
            {
                s = ListSquares.Where(p => p.PositionX == monsterCounter && p.PositionZ == ListSquares.Max(z => z.PositionZ)).FirstOrDefault();
            }
            
            Vector3 pos = new Vector3(s.PositionX, 0.0f, s.PositionZ);

            GameObject monsterGo = (GameObject)Instantiate(Resources.Load("Prefabs/Prefab_Monster"), pos, Quaternion.identity);
            monsterGo.transform.parent = this.transform.parent;
            monsterGo.name = "Team" + monster.whicTeam.ToString() + "_" + monsterCounter++;
            monster.currentSquare = s;
            monster.isSelected = false;
            ListMonstersGo.Add(monsterGo);
        }

        print(ListMonstersGo[0].transform.position);
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
