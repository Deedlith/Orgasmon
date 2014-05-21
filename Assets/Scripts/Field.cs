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
            m.whichTeam = Team.A;
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
            m.whichTeam = Team.B;
            ListMonsters.Add(m);
        }

		Generate();

		GameManager.Instance.currentTeamTurn = Team.A;
		GameManager.Instance.LaunchLevel();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var monster = ListMonsters.Where(m => m.isSelected == true).First();
            GameObject go = GetGoFromMonster(monster);
            monster.currentSquare = ListSquares.Where(s =>
                s.PositionX == Mathf.RoundToInt(go.transform.position.x)
                &&
                s.PositionZ == Mathf.RoundToInt(go.transform.position.z)).First();
            print(monster.currentSquare.PositionX);
            CheckEnnemiesPosition();
        }
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
            if (monster.whichTeam == Team.A)
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
            monsterGo.name = "Team" + monster.whichTeam.ToString() + "_" + monsterCounter++;
            monster.currentSquare = s;
            monster.isSelected = false;
            ListMonstersGo.Add(monsterGo);
        }

        var t = ListMonsters.ElementAt(8);
        print("TEAM : " + t.whichTeam.ToString());
        t.isSelected = true;
        print(ListMonstersGo[0].transform.position);
	}

	public void CheckEnnemiesPosition()
	{
		Monster currentMonster = ListMonsters.Where(lm => (lm.whichTeam.Equals(GameManager.Instance.currentTeamTurn) && (lm.isSelected == true))).FirstOrDefault();
        int damage = -1;

		if(currentMonster != null)
		{
            print("Current Monster");
			List<Monster> localEnnemies = ListMonsters.Where(lm => (
																	!(lm.whichTeam.Equals(currentMonster.whichTeam)) 
																	&& ((lm.currentSquare.PositionX == currentMonster.currentSquare.PositionX + 1 && lm.currentSquare.PositionZ == currentMonster.currentSquare.PositionZ) 
																		|| (lm.currentSquare.PositionX == currentMonster.currentSquare.PositionX - 1 && lm.currentSquare.PositionZ == currentMonster.currentSquare.PositionZ) 
																		|| (lm.currentSquare.PositionX == currentMonster.currentSquare.PositionX && lm.currentSquare.PositionZ == currentMonster.currentSquare.PositionZ + 1) 
																		|| (lm.currentSquare.PositionX == currentMonster.currentSquare.PositionX && lm.currentSquare.PositionZ == currentMonster.currentSquare.PositionZ - 1) 
			    											))).ToList();
            int index = 0;
            if (localEnnemies.Count() == 0)
                return;
			if(localEnnemies.Count() > 1)
			{
				index = Random.Range(0, localEnnemies.Count());

                damage = currentMonster.LaunchAttack(localEnnemies[index]);
			}

            damage = currentMonster.LaunchAttack(localEnnemies[index]);
            print("DAMAGE : " + damage + " PV LEFT : " + localEnnemies[index].pv);
            if (localEnnemies[index].pv <= 0)
            {
                var go = ListMonstersGo.Where(m =>
                    Mathf.RoundToInt(m.transform.position.x) == localEnnemies[index].currentSquare.PositionX
                    &&
                    Mathf.RoundToInt(m.transform.position.z) == localEnnemies[index].currentSquare.PositionZ).First();
                print(go.name + " IS DEAD !");
                Destroy(go);
                bool victory = CheckVictory();
                if (victory)
                    print("TEAM " + GameManager.Instance.currentTeamTurn.ToString() + " HAS WIN !");
            }

		}
	}

    public bool CheckVictory()
    {
        return (ListMonsters.Where(m => m.whichTeam == Team.A).Count() == 0 || ListMonsters.Where(m => m.whichTeam == Team.B).Count() == 0);
    }

    public Monster GetMonsterFromGo(GameObject go)
    {
        return ListMonsters.Where(m =>
                    m.currentSquare.PositionX == Mathf.RoundToInt(go.transform.position.x)
                    &&
                    m.currentSquare.PositionZ == Mathf.RoundToInt(go.transform.position.z)).First();
    }

    public GameObject GetGoFromMonster(Monster m)
    {

        GameObject goToReturn = ListMonstersGo.Where(go =>
                    (go.transform.position.x == m.currentSquare.PositionX)
                    &&
                    (go.transform.position.z == m.currentSquare.PositionZ)).First();
        return goToReturn;
    }
}
