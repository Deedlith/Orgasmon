using UnityEngine;
using System.Collections;
using System.Linq;

public class Player : MonoBehaviour {

	void Awake()
	{

	}
	
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
		print ("HIT PRESSED");
		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// Sélectionner un monstre
		// Lui afficher son pattern de déplacement
		// Si un monstre est sélectionné et qu'il clique sur une des cases, le déplacer sur cette case
		// Je te laisse le code en dessous exemple
		/*if(Physics.Raycast(ray, out hit, 1000)) 
		{
			GameObject objectHit = hit.collider.gameObject;
			if(objectHit.name.Substring(0, 6).Equals("Square"))
			{
				if(onDice != null)
				{
					onDice.IsSelected = false;
					onDice = null;
				}
				
				this.transform.position = objectHit.transform.position;
				GameManager.Instance.RefreshGui();
			}
			else if(objectHit.name.Substring(0, 4).Equals("Dice"))
			{
				if(onDice != null)
					onDice.IsSelected = false;
				onDice = objectHit.GetComponent<Dice>();
				onDice.IsSelected = true;
				this.transform.position = new Vector3(objectHit.transform.position.x,
				                                      objectHit.collider.bounds.size.y,
				                                      objectHit.transform.position.z);
				GameManager.Instance.RefreshGui();
			}
		}*/
	}
	
	bool CanMove(int x, int z)
	{
		return true;
		/*if (onDice == false)
			return false;
		
		var nextSquare = Level.Instance._field.ListSquares.Where(s => s.PositionX == x && s.PositionZ == z).FirstOrDefault();
		
		if(nextSquare == null || !nextSquare.SquareIsExisting())
		{
			return false;
		}
		
		if(this.onDice.actualSquare != nextSquare)
			return false;
		
		return true;*/
	}
}
