using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum Attack
{
	Body,
	Head,
	Feets,
	Arms,
	Total
};

public enum Shield
{
	Body,
	Head,
	Feets,
	Arms,
	Total
};

public enum Movement
{
	Horizontal,
	Vertical
};

public enum Team
{
    A,
    B
}

public struct AttackPattern
{
    public Attack atk;
    public int power;
}

public struct DefensePattern
{
    public Shield def;
    public int power;
}

public class Monster
{

	public int level;
	public int pv;
    public List<AttackPattern> listAttackPatterns;
    public List<DefensePattern> listDefensePatterns;
	public List<Movement> listMovements;
	public int speed;
    public int overall = 0;
    public int id = -1;

	public Square currentSquare = null;

    public bool isSelected;
    public Team whichTeam;

    public Monster()
    {
    }

    public Monster(Team whichT, List<AttackPattern> listAttPat, List<DefensePattern> listDefPat, List<Movement> listMov,int l = 0, int s = 0, int pv2 = 0)
    {
        level = l;
        pv = pv2;
        // Attack Pattern
        listAttackPatterns = listAttPat;
        AttackPattern attack = new AttackPattern();
        attack.atk = Attack.Arms;
        attack.power = level * 3;
        listAttackPatterns.Add(attack);
        // Defense Pattern
        listDefensePatterns = listDefPat;
        DefensePattern defense = new DefensePattern();
        defense.def = Shield.Arms;
        defense.power = level;
        listDefensePatterns.Add(defense);
        // Movement Pattern
        listMovements = listMov;
        //listMovements.Add(Movement.Vertical);
        speed = s;
        whichTeam = whichT;
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}


    public void Infos(TextMesh text)
    {
        text.text = "Level: " + level + "\nPV: " + pv + "\nSpeed: " + speed + "\nTeam: " + whichTeam;
    }

    public int LaunchAttack(Monster other)
    {
        // Sélectionner un pattern d'attaque
        int random = Random.Range(0, listAttackPatterns.Count);
        var atkPattern =  listAttackPatterns.ElementAt(random);
        /*** ALLUME LA PARTIE DU CORPS QUI CORRESPOND ***/
        // Sélectionner un pattern de défense
        random = Random.Range(0, other.listDefensePatterns.Count);
        var defPattern = other.listDefensePatterns.ElementAt(random);
        /*** ALLUME LA PARTIE DU CORPS QUI CORRESPOND ***/

        // SI la défense est AU MOINS plus rapide que l'attaque ALORS la défense est complète
        // SINON on applique un malus à la défense
        int timeAtk = this.speed * atkPattern.power;
        int timeDef = other.speed * defPattern.power;
        int damage = 0;
        if (timeDef <= timeAtk)
        {
            damage = (atkPattern.power - defPattern.power < 0) ? 0 : Mathf.CeilToInt(atkPattern.power - defPattern.power);
        }
        else
        {
            int coefMinus = Mathf.RoundToInt(timeAtk / timeDef);
            int defPower = Mathf.FloorToInt(defPattern.power * coefMinus);
            damage = (atkPattern.power - defPower < 0) ? 0 : Mathf.CeilToInt(atkPattern.power - defPower);
        }

        other.pv -= damage;

        return damage;
    }
}
