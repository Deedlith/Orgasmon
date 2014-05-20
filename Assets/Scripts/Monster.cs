using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

enum Attack
{
	Body,
	Head,
	Feets,
	Arms,
	Total
};

enum Shield
{
	Body,
	Head,
	Feets,
	Arms,
	Total
};

enum Movement
{
	Horizontal,
	Vertcal
};

public class Monster : MonoBehaviour {

	int _level;
	int _pv;
	List<Attack> _listAttacks;
	List<Shield> _listShields;
	List<Movement> _listMovements;
	int _sSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
