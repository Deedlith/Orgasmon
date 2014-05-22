using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Field : MonoBehaviour
{
    public SceneInGameCtrl sceneInGameCtrl;
    public GameObject parent;
    #region SINGLETON
    private static Field _instance = null;

    public static Field Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Field>().GetComponent<Field>();
            }

            return _instance;
        }
    }
    #endregion
	
	public List<Square> ListSquares = new List<Square>();
	public List<GameObject> ListSquaresGo = new List<GameObject>();
	public List<Monster> ListMonsters = new List<Monster>();
    public List<GameObject> ListMonstersGo = new List<GameObject>();
    public List<GameObject> ListMonstersGoPlayer = new List<GameObject>();
    public Dictionary<GameObject, Monster> dicoMonsterGOMonster = new Dictionary<GameObject, Monster>();
	
	void Start () {
		for(int i = 0; i < 10; i++)
		{
			for(int j = 0; j < 10; j++)
			{
				Square s = new Square(i,j);
				ListSquares.Add(s);
			}
		}

        /*for (int i = 0; i < 5; i++)
        {
            
            // Attack Pattern
            List<AttackPattern> listAttackPatterns = new List<AttackPattern>();
            AttackPattern attack = new AttackPattern();
            attack.atk = Attack.Arms;
            attack.power = (i + 1) * 3;
            listAttackPatterns.Add(attack);
            // Defense Pattern
            List<DefensePattern> listDefensePatterns = new List<DefensePattern>();
            DefensePattern defense = new DefensePattern();
            defense.def = Shield.Arms;
            defense.power = (i + 1) * 2;
            listDefensePatterns.Add(defense);
            // Movement Pattern
            List<Movement> listMovements = new List<Movement>();
            listMovements.Add(Movement.Vertical);
            Monster m = new Monster(Team.A, listAttackPatterns, listDefensePatterns, listMovements, (i + 1), ((i + 1) * 3), 2 * (i+1));
            ListMonsters.Add(m);



            // Attack Pattern
            listAttackPatterns = new List<AttackPattern>();
            attack = new AttackPattern();
            attack.atk = Attack.Arms;
            attack.power = (i + 1) * 3;
            listAttackPatterns.Add(attack);
            // Defense Pattern
            listDefensePatterns = new List<DefensePattern>();
            defense = new DefensePattern();
            defense.def = Shield.Arms;
            defense.power = (i + 1) * 2;
            listDefensePatterns.Add(defense);
            // Movement Pattern
            listMovements = new List<Movement>();
            listMovements.Add(Movement.Vertical);
            m = new Monster(Team.B, listAttackPatterns, listDefensePatterns, listMovements, (i + 1), ((i + 1) * 3), 2 * (i + 1));
            ListMonsters.Add(m);
        }*/

		//Generate();
        sceneInGameCtrl.InitGame();
		GameManager.Instance.currentTeamTurn = Team.A;
	}

    public void CreateMonsters()
    {
         int posX = -4, posY = 2, posZ = 4;
         for (int i = 0; i < 5; i++)
         {
             // Attack Pattern
             List<AttackPattern> listAttackPatterns = new List<AttackPattern>();
             AttackPattern attack = new AttackPattern();
             attack.atk = Attack.Arms;
             attack.power = (i + 1) * 3;
             listAttackPatterns.Add(attack);
             // Defense Pattern
             List<DefensePattern> listDefensePatterns = new List<DefensePattern>();
             DefensePattern defense = new DefensePattern();
             defense.def = Shield.Arms;
             defense.power = (i + 1) * 2;
             listDefensePatterns.Add(defense);
             // Movement Pattern
             List<Movement> listMovements = new List<Movement>();
             listMovements.Add(Movement.Vertical);
             Monster m = new Monster(Team.A, listAttackPatterns, listDefensePatterns, listMovements, (i + 1), ((i + 1) * 3), 2 * (i + 1));
             ListMonsters.Add(m);

             //Pour TeamA
             Vector3 pos = new Vector3(posX, posY, posZ);

             GameObject AllMonstersGo = (GameObject)Instantiate(Resources.Load("Prefabs/Prefab_MonsterInfos"), pos, Quaternion.identity);
             GameObject monsterGo = AllMonstersGo.transform.FindChild("Monster").gameObject;

             dicoMonsterGOMonster.Add(monsterGo, m);
             m.Infos(AllMonstersGo.transform.FindChild("InfosMonsters").gameObject.transform.GetComponent<TextMesh>());
             AllMonstersGo.name = "MonstersInfos_" + m.whichTeam.ToString() + i;
             AllMonstersGo.transform.parent = parent.transform;

             monsterGo.name = "Monster" + m.whichTeam.ToString() + "_" + i;
             m.isSelected = false;
             posX += 2;

             listAttackPatterns = new List<AttackPattern>();
             attack = new AttackPattern();
             attack.atk = Attack.Arms;
             attack.power = (i + 1) * 3;
             listAttackPatterns.Add(attack);
             // Defense Pattern
             listDefensePatterns = new List<DefensePattern>();
             defense = new DefensePattern();
             defense.def = Shield.Arms;
             defense.power = (i + 1) * 2;
             listDefensePatterns.Add(defense);
             // Movement Pattern
             listMovements = new List<Movement>();
             listMovements.Add(Movement.Vertical);
             m = new Monster(Team.B, listAttackPatterns, listDefensePatterns, listMovements, (i + 1), ((i + 1) * 3), 2 * (i + 1));
             ListMonsters.Add(m);


         }
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var monster = ListMonsters.Where(m => m.isSelected == true).First();
            GameObject go = ListMonstersGo.Where(g => g.transform.position.z != 0 && g.transform.position.z != 9).First(); 
            monster.currentSquare = ListSquares.Where(s =>
                s.PositionX == Mathf.RoundToInt(go.transform.position.x)
                &&
                s.PositionZ == Mathf.RoundToInt(go.transform.position.z)).First();
            print(monster.currentSquare.PositionX);
            CheckEnnemiesPosition();
        }
	}

    public void RegenerateMonster(Monster m, GameObject allMonster)
    {
        print(m.whichTeam + " - " + m.listAttackPatterns + " - " + m.listDefensePatterns + " - " + m.listMovements + " - " + m.level + " - " + m.speed + " - " + m.pv);
        Monster mon = new Monster(m.whichTeam,m.listAttackPatterns,m.listDefensePatterns,m.listMovements,m.level,m.speed,m.pv);
        mon.currentSquare = m.currentSquare;
        mon.isSelected = false;
        ListMonsters.Remove(m);
        ListMonsters.Add(mon);

        GameObject AllMonstersGo = (GameObject)Instantiate(Resources.Load("Prefabs/Prefab_MonsterInfos"), allMonster.transform.position, Quaternion.identity);
        GameObject monsterGo = AllMonstersGo.transform.FindChild("Monster").gameObject;
        mon.Infos(AllMonstersGo.transform.FindChild("InfosMonsters").gameObject.transform.GetComponent<TextMesh>());
        AllMonstersGo.name = allMonster.name;
        AllMonstersGo.transform.parent = allMonster.transform.parent;

        monsterGo.name = allMonster.transform.GetChild(1).name;

        if (mon.whichTeam == Team.A)
        {
            ListMonstersGoPlayer.Remove(allMonster);
            ListMonstersGoPlayer.Add(AllMonstersGo);
        }
        ListMonstersGo.Remove(allMonster.transform.transform.GetChild(1).gameObject);
        ListMonstersGo.Add(monsterGo);
        Destroy(allMonster);
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
            monster.currentSquare = s;


            //Team.B a générer
            if (monster.whichTeam != Team.A)
            {
                GameObject AllMonstersGo = (GameObject)Instantiate(Resources.Load("Prefabs/Prefab_MonsterInfos"), pos, Quaternion.identity);
                GameObject monsterGo = AllMonstersGo.transform.FindChild("Monster").gameObject;

                monster.Infos(AllMonstersGo.transform.FindChild("InfosMonsters").gameObject.transform.GetComponent<TextMesh>());

                AllMonstersGo.name = "MonstersInfos_" + monster.whichTeam.ToString() + monsterCounter;
                AllMonstersGo.transform.parent = parent.transform;

                monster.isSelected = false;
                monsterGo.name = "Monster" + monster.whichTeam.ToString() + "_" + monsterCounter++;
                ListMonstersGo.Add(monsterGo);
                dicoMonsterGOMonster.Add(monsterGo, monster);
            }
            else
            {
                GameObject myValue = dicoMonsterGOMonster.FirstOrDefault(x => x.Value == monster).Key;
                myValue.transform.position = pos;
            }
            
        }
        /*
        var t = ListMonsters.ElementAt(8);
        t.isSelected = true;*/
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
                ListMonsters.Remove(localEnnemies[index]);
                ListMonstersGo.Remove(go);
                Destroy(go);
                ListMonsters.Remove(localEnnemies[index]);
                bool victory = CheckVictory();
                print("End Check Victory");
                if (victory)
                    print("TEAM " + GameManager.Instance.currentTeamTurn.ToString() + " HAS WIN !");
            }

		}
	}

    public bool CheckVictory()
    {
        print("Check Victory");
        return (ListMonsters.Where(m => m.whichTeam == Team.A).Count() == 0 || ListMonsters.Where(m => m.whichTeam == Team.B).Count() == 0);
    }

    public Monster GetMonsterFromGo(GameObject go)
    {
		foreach (Monster m in ListMonsters)
		{
			//print ("mx : " + m.currentSquare.PositionX + ", mz : " + m.currentSquare.PositionZ);
			//print ("gox : " + Mathf.RoundToInt(go.transform.position.x) + ", goz : " + Mathf.RoundToInt(go.transform.position.z));

			if(m.currentSquare.PositionX == Mathf.RoundToInt(go.transform.position.x)
			   && m.currentSquare.PositionZ == Mathf.RoundToInt(go.transform.position.z))
			{
				return m;
			}
		}

		return null;
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
