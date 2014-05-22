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

	public Square currentSquare = null;

    public bool isSelected;
	public Team whichTeam;

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
