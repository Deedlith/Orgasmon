using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

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

	public struct Notation
	{
		public Notation(int note, int indexPattern, int indexPopulation)
		{
			Note = note;
			IndexPattern = indexPattern;
			IndexPopulation = indexPopulation;
		}
		
		public int Note { get; private set; }
		public int IndexPattern { get; private set; }
		public int IndexPopulation { get; private set; }
	}
	
	int _patternsLength = 50;
	int _populationLength = 100;
	int _iterations = 5;
	int _mutation = 5;

	List<List<Movement>> _patterns;
	List<List<Movement>> _newPatterns;
	List<Neuronal> _population;
	List<Neuronal> _newPopulation;
	List<Notation> _notations;
	List<Notation> _newNotations;
	
	public List<List<Movement>> GeneratePattern()
	{
		List<List<Movement>> patterns = new List<List<Movement>>();

		for(int i = 0; i < _patternsLength; i++)
		{
			List<Movement> temp = new List<Movement>();

			int movementLength = UnityEngine.Random.Range(1, 6);

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
							Movement m = (Movement) UnityEngine.Random.Range(0, 2);
							temp.Add(m);
						}
					}
				}
				while(!temp.Contains(Movement.Horizontal) || !temp.Contains(Movement.Vertical));
			}

			if(temp != null) patterns.Add(temp);
		}

		return patterns;
	}

	public List<Neuronal> GeneratePopulation(int mutation = 1)
	{
<<<<<<< HEAD
		List<Neuronal> population = new List<Neuronal>();

		for(int i = 0; i < _populationLength; i++)
		{
			Neuronal network = new Neuronal(_patternsLength, _patternsLength, 1);

			int lengthIHSums = (_patternsLength * _patternsLength);
			int lengthIHBiaises = _patternsLength;
			int lengthHOSums = (1 * _patternsLength);
			int lengthHOBiaises = 1;
			int lengthWeights = lengthIHSums + lengthIHBiaises + lengthHOSums + lengthHOBiaises;

			double[] weights = new double[lengthWeights];

			double stepper = 0.1;

			for(int counter = 0; counter < lengthWeights; counter++)
			{
				if(counter < lengthIHSums)
				{
					weights[counter] = stepper;
					stepper += 0.1;
				}
				else if(counter < (lengthIHSums + lengthIHBiaises))
				{
					weights[counter] = -((double) UnityEngine.Random.Range(1, 10)) * mutation;
				}
				else if(counter < (lengthIHSums + lengthIHBiaises + lengthHOSums))
				{
					weights[counter] = stepper;
					stepper += 0.1;
				}
				else if(counter < (lengthIHSums + lengthIHBiaises + lengthHOSums + lengthHOBiaises))
				{
					weights[counter] = -((double) UnityEngine.Random.Range(1, 10)) * mutation;
				}
			}

			network.SetWeights(weights);

			population.Add(network);
		}

		return population;
=======
        return 0;
>>>>>>> FETCH_HEAD
	}

	public int Evaluate(List<Movement> pattern)
	{
		int max = 20;
		int sPosX = 0;
		int sPosZ = 0;
		int aPosX = 1;
		int aPosZ = 9;

		double distance = Math.Sqrt(Math.Pow((Math.Abs(aPosX) - Math.Abs(sPosX)), 2) + Math.Pow((Math.Abs(aPosZ) - Math.Abs(sPosZ)), 2));

		for(int i = 0; i < max; i++)
		{
			foreach(Movement m in pattern)
			{
				if(m == Movement.Vertical)
				{
					if(UnityEngine.Random.Range(0,2) == 1)
					{
						sPosX += 1;
					}
					else
					{
						sPosX -= 1;
					}
				}
				else if(m == Movement.Horizontal)
				{
					if(UnityEngine.Random.Range(0,2) == 1)
					{
						sPosZ += 1;
					}
					else
					{
						sPosZ -= 1;
					}
				}

				distance = Math.Sqrt(Math.Pow((Math.Abs(aPosX) - Math.Abs(sPosX)), 2) + Math.Pow((Math.Abs(aPosZ) - Math.Abs(sPosZ)), 2));

				if(distance == 0)
				{
					return i;
				}
			}
		}

		return max;
	}

	public void Compare()
	{
		_notations = _notations.OrderBy(n => n.Note).ToList();
		_newNotations = _newNotations.OrderBy(n => n.Note).ToList();

		List<Notation> _tempNotation = new List<Notation>();
		List<Neuronal> _tempPopulation = new List<Neuronal>();
		List<List<Movement>> _tempPatterns = new List<List<Movement>>();

		for(int i = 0; i < _notations.Count(); i++)
		{
			if(_notations[i].Note <= _newNotations[i].Note)
			{
				_tempNotation.Add(new Notation(_notations[i].Note, _notations[i].IndexPattern, _notations[i].IndexPopulation));
				_tempPatterns.Add(_patterns[_notations[i].IndexPattern]);
				_tempPopulation.Add(_population[_notations[i].IndexPopulation]);
			}
			else if(_notations[i].Note > _newNotations[i].Note)
			{
				_tempNotation.Add(new Notation(_newNotations[i].Note, _newNotations[i].IndexPattern, _newNotations[i].IndexPopulation));
				_tempPatterns.Add(_newPatterns[_newNotations[i].IndexPattern]);
				_tempPopulation.Add(_newPopulation[_newNotations[i].IndexPopulation]);
			}
		}

		_notations = _tempNotation;
		_patterns = _tempPatterns;
		_population = _tempPopulation;
	}

	public List<Movement> Generate()
	{
		_patterns = GeneratePattern();
		_population = GeneratePopulation();

		for(int i = 0; i < _iterations; i++)
		{
			_notations = new List<Notation>();

			foreach(Neuronal n in _population)
			{
				double[] xValues = new double[_patternsLength];
				
				for(int x = 0; x < _patternsLength; x++)
				{
					xValues[x] = (double) x;
				}
				
				double[] yValues = n.ComputeOutputs(xValues);

				int index = (int) yValues[0];

				int note = Evaluate(_patterns[index]);

				_notations.Add(new Notation(note, index, _population.IndexOf(n)));
			}

			_newPatterns = GeneratePattern();

			_newPopulation = GeneratePopulation(_mutation);

			_newNotations = new List<Notation>();
			
			foreach(Neuronal n in _newPopulation)
			{
				double[] xValues = new double[_patternsLength];
				
				for(int x = 0; x < _patternsLength; x++)
				{
					xValues[x] = (double) x;
				}
				
				double[] yValues = n.ComputeOutputs(xValues);
				
				int index = (int) yValues[0];
				
				int note = Evaluate(_newPatterns[index]);
				
				_newNotations.Add(new Notation(note, index, _newPopulation.IndexOf(n)));
			}

			Compare();
		}

		MonoBehaviour.print("Note : " + _notations[0].Note);

		return _patterns[0];
	}

	public void DisplayPattern(List<Movement> pattern)
	{
		foreach(Movement m in pattern)
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
