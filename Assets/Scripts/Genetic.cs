using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Genetic 
{
	#region SINGLETON
	private static Genetic _instance = null;
	
	public static Genetic Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new Genetic();
			}
			
			return _instance;
		}
	}
	#endregion


	int _populateLength = 100;

	List<List<Movement>> populate;


	public void GeneratePopulation()
	{
		populate = new List<List<Movement>>();

		for(int i = 0; i < _populateLength; i++)
		{
			List<Movement> temp = new List<Movement>();

			int movementLength = Random.Range(1, 6);

			if(movementLength == 1)
			{
				temp.Add(Movement.Vertical);
			}
			else
			{
				do
				{
					temp.Clear();

					for(int j = 0; j < movementLength; j++)
					{
						if(j == 0) temp.Add(Movement.Vertical);
						else
						{
							Movement m = (Movement) Random.Range(0, 2);
							temp.Add(m);
						}
					}
				}
				while(!temp.Contains(Movement.Horizontal) || !temp.Contains(Movement.Vertical));
			}

			if(temp != null) populate.Add(temp);
		}
	}

	public int Evaluate()
	{
        return 0;
	}

	public void DisplayPopulation()
	{
		foreach(List<Movement> lm in populate)
		{
			MonoBehaviour.print("LM : " + lm.Count());

			foreach(Movement m in lm)
			{
				if(m == Movement.Horizontal)
				{
					MonoBehaviour.print("M : Horizontal");
				}
				else if (m == Movement.Vertical)
				{
					MonoBehaviour.print("M : Vertical");
				}
			}
		}
	}

}
