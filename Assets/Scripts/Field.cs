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

        sceneInGameCtrl.InitGame();
		GameManager.Instance.currentTeamTurn = Team.A;
	}

    //create monster for player
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

             //For TeamA
             Vector3 pos = new Vector3(posX, posY, posZ);
             GameObject AllMonstersGo = (GameObject)Instantiate(Resources.Load("Prefabs/Prefab_MonsterInfos"), pos, Quaternion.identity);
             GameObject monsterGo = AllMonstersGo.transform.FindChild("Monster").gameObject;

             dicoMonsterGOMonster.Add(monsterGo, m);
             m.Infos(AllMonstersGo.transform.FindChild("InfosMonsters").gameObject.transform.GetComponent<TextMesh>());
             AllMonstersGo.name = "MonstersInfos_" + m.whichTeam.ToString() + i;
             AllMonstersGo.transform.parent = parent.transform;

             monsterGo.name = "Monster_" + m.whichTeam.ToString() + i;
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

		if (Input.GetKeyDown(KeyCode.G))
		{
			Genetic.Instance.DisplayPattern(Genetic.Instance.Generate());
		}
	}

    //Regenerate another monster
    public void RegenerateMonster(Monster m, GameObject allMonster)
    {
        Monster mon = new Monster(m.whichTeam,m.listAttackPatterns,m.listDefensePatterns,m.listMovements,m.level,m.speed,m.pv);
        mon.isSelected = false;
        ListMonsters.Remove(m);
        ListMonsters.Add(mon);

        GameObject AllMonstersGo = (GameObject)Instantiate(Resources.Load("Prefabs/Prefab_MonsterInfos"), allMonster.transform.position, Quaternion.identity);
        GameObject monsterGo = AllMonstersGo.transform.FindChild("Monster").gameObject;
        monsterGo.name = allMonster.transform.GetChild(1).name;
        dicoMonsterGOMonster.Add(monsterGo, mon);
        mon.Infos(AllMonstersGo.transform.FindChild("InfosMonsters").gameObject.transform.GetComponent<TextMesh>());
        AllMonstersGo.name = allMonster.name;
        AllMonstersGo.transform.parent = allMonster.transform.parent;

        ListMonstersGo.Remove(allMonster.transform.transform.GetChild(1).gameObject);
        ListMonstersGo.Add(monsterGo);

        Destroy(allMonster);
    }

    //Fusion 2 monsters
    public void Fusion()
    {
        Monster monster1 = null,monster2 = null;
        //Get Monsters Select
        foreach (Monster monster in ListMonsters)
        {
            if(monster.isSelected)
            {
                if(monster1 == null)
                    monster1 = monster;
                if (monster1 != monster && monster2 == null)
                {
                    monster2 = monster;
                    break;
                }
            }
        }

        //print("Monsters select is " + monster1.level + " _ " + monster2.level);

        //Create New Monster and Destroy old
        int level,speed,pv;
        if((monster1.level + monster2.level) <= 5)
            level = (monster1.level + monster2.level);
        else
            level = 5;

        List<AttackPattern> listAttackPatterns = new List<AttackPattern>();
        List<DefensePattern> listDefensePatterns = new List<DefensePattern>();
        List<Movement> listMovements = new List<Movement>();

        if (monster1.level > monster2.level)
        {
            speed = (int)(0.5 * monster1.speed) + monster2.speed;
            pv = (int)(1.5 * monster1.pv) + monster2.pv;
            listAttackPatterns = monster2.listAttackPatterns;
            for (int i = 0; i < monster1.listDefensePatterns.Capacity; i++ )
            {
                DefensePattern def = monster1.listDefensePatterns.ElementAt(i);
                def.power += 2;
                listDefensePatterns.Add(def);
            }
            if (Random.Range(0, 1) == 0)
            {
                listMovements.Add(Movement.Horizontal);
            }
            else
            {
                listMovements.Add(Movement.Vertical);
            }
        }
        else
        {
            speed = (int)(0.5 * monster2.speed) + monster1.speed;
            pv = (int)(1.5 * monster2.pv) + monster1.pv;
            listAttackPatterns = monster1.listAttackPatterns;
            for (int i = 0; i < monster2.listDefensePatterns.Count; i++)
            {
                DefensePattern def = monster2.listDefensePatterns.ElementAt(i);
                def.power += 2;
                listDefensePatterns.Add(def);
            }
            if (Random.Range(0, 1) == 0)
            {
                listMovements.Add(Movement.Horizontal);
            }
            else
            {
                listMovements.Add(Movement.Vertical);
            }
        }
        


        Monster mon = new Monster(monster1.whichTeam, listAttackPatterns, listDefensePatterns, listMovements, level, speed, pv);
        mon.isSelected = false;

        GameObject GoM1 =  dicoMonsterGOMonster.FirstOrDefault(x => x.Value == monster1).Key;
        GameObject GoM2 = dicoMonsterGOMonster.FirstOrDefault(x => x.Value == monster2).Key;

        GameObject AllMonstersGo = (GameObject)Instantiate(Resources.Load("Prefabs/Prefab_MonsterInfos"), GoM1.transform.parent.gameObject.transform.position, Quaternion.identity);
        GameObject monsterGo = AllMonstersGo.transform.FindChild("Monster").gameObject;
        dicoMonsterGOMonster.Add(monsterGo, mon);
        mon.Infos(AllMonstersGo.transform.FindChild("InfosMonsters").gameObject.transform.GetComponent<TextMesh>());
        AllMonstersGo.name = "MonstersInfos_" + mon.whichTeam.ToString() + (monster1.level -1);
        AllMonstersGo.transform.parent = GoM1.transform.parent.gameObject.transform.parent;

        monsterGo.name = "Monster_" + mon.whichTeam.ToString() + (monster1.level -1);

        dicoMonsterGOMonster.Remove(GoM1);
        dicoMonsterGOMonster.Remove(GoM2);

        ListMonsters.Remove(monster1);
        ListMonsters.Remove(monster2);
        ListMonsters.Add(mon);

        ListMonstersGo.Remove(GoM1);
        ListMonstersGo.Remove(GoM2);
        ListMonstersGo.Add(monsterGo);

        Destroy(GoM1.transform.parent.gameObject);
        Destroy(GoM2.transform.parent.gameObject);
    }


	public void Generate()
	{
		int squareCounter = 0;
		foreach(var square in ListSquares)
		{
			Vector3 pos = new Vector3(square.PositionX, 0.0f, square.PositionZ);
			
			GameObject squareGo = (GameObject) Instantiate(Resources.Load("Prefabs/Prefab_Square"), pos, Quaternion.identity);
            squareGo.transform.parent = parent.transform;
			squareGo.name = "Square_" + squareCounter++;
			ListSquaresGo.Add(squareGo);
		}

        int monsterCounter = 0;
        foreach (var monster in ListMonsters)
        {
            //Récupère le square min en z pour la Team A et le max pour la Team B et le x current
            Square s;
            Vector3 pos;
            if (monster.whichTeam == Team.A)
            {
                GameObject myValue = dicoMonsterGOMonster.FirstOrDefault(x => x.Value == monster).Key;
                string[] separator = new string[] {"_A"};
                var strings = myValue.name.Split(separator, System.StringSplitOptions.None);
                int num = int.Parse(strings[1]);

                s = ListSquares.Where(p => p.PositionX == num && p.PositionZ == ListSquares.Min(z => z.PositionZ)).FirstOrDefault();

                pos = new Vector3(s.PositionX, 0.0f, s.PositionZ);
                myValue.transform.parent.transform.localPosition = pos;
            }
            else
            {
                s = ListSquares.Where(p => p.PositionX == monsterCounter && p.PositionZ == ListSquares.Max(z => z.PositionZ)).FirstOrDefault();

                pos = new Vector3(s.PositionX, 0.0f, s.PositionZ);
                GameObject AllMonstersGo = (GameObject)Instantiate(Resources.Load("Prefabs/Prefab_MonsterInfos"), pos, Quaternion.identity);
                GameObject monsterGo = AllMonstersGo.transform.FindChild("Monster").gameObject;

                monster.Infos(AllMonstersGo.transform.FindChild("InfosMonsters").gameObject.transform.GetComponent<TextMesh>());

                AllMonstersGo.name = "MonstersInfos_" + monster.whichTeam.ToString() + monsterCounter;
                AllMonstersGo.transform.parent = parent.transform;


                monsterGo.name = "Monster_" + monster.whichTeam.ToString() + monsterCounter++;
                ListMonstersGo.Add(monsterGo);
                dicoMonsterGOMonster.Add(monsterGo, monster);
            }

            monster.isSelected = false;
            monster.currentSquare = s;        
        }
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

    public void ResetGame()
    {
        dicoMonsterGOMonster.Clear();
        ListMonsters.Clear();
        ListMonstersGo.Clear();
    }
}
