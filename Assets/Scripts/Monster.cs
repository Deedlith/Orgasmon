using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Monster : MonoBehaviour {

	Vector3 Position;
	Quaternion Rotation;

	int Level;
	int PV;
	List<Attack> Attacks;
	List<Shield> Shields;
	List<Movement> Movements;
	int Speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
