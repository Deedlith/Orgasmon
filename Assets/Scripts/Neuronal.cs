using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Neuronal 
{
	List<List<Movement>> _inputs;
	List<List<Movement>> _targets;

	int _ni = 0;
	int _nh = 0;
	int _no = 0;

	float[] _ai;
	float[] _ah;
	float[] _ao;

	float[][] _wi;
	float[][] _wo;

	public float[][] MakeMatrix(int I, int J, float fill = 0.0f)
	{
		float[] m = new float[I]();

		foreach(int i in I)
		{
			m[i] = new float[J]();

			foreach(int j in J)
			{
				m[i][j] = fill;
			}
		}

		return m;
	}

	public float Sigmoid(float x)
	{
		return Mathf.Tan(x);
	}

	public float Dsigmoid(float y)
	{
		return (1.0f - (y * y));
	}

	public void Neuronal(int ni, int nh, int no)
	{
		_ni = ni + 1;
		_nh = nh;
		_no = no;

		_ai = [1.0f] * _ni;
		_ah = [1.0f] * _nh;
		_ao = [1.0f] * _no;

		_wi = MakeMatrix(_ni, _nh);
		_wo = MakeMatrix(_nh, _no);

		foreach(int i in _ni)
		{
			foreach(int h in _nh)
			{
				_wi[i][h] = Random.Range(-2.0f, 2.0f);
			}
		}

		foreach(int h in _nh)
		{
			foreach(int o in _no)
			{
				_wo[h][o] = Random.Range(-2.0f, 2.0f);
			}
		}

		_inputs = new List<List<Movement>>();

		foreach(List<Movement> lm in inputs)
		{			
			List<Movement> temp = new List<Movement>();

			foreach(Movement m in lm)
			{
				temp.Add(m);
			}

			_inputs.Add(temp);
		}
	}

	public float[] Update(List<List<Movement>> inputs)
	{
		if(inputs.Count() != _ni - 1)
		{
			MonoBehaviour.print("Error : inputs and ni not identical");
			return;
		}

		foreach(int i in (_ni - 1))
		{
			_ai[i] = inputs[i].Count(); //Not exactly
		}

		foreach(int h in _nh)
		{
			float sum = 0.0f;

			foreach(int i in _ni)
			{
				sum = (sum + _ai[i] * _wi[i][h]);
				_ah[h] = Sigmoid(sum);
			}
		}

		foreach(int o in _no)
		{
			float sum = 0.0f;
			
			foreach(int h in _nh)
			{
				sum = (sum + _ah[h] * _wo[h][o]);
				_ah[o] = Sigmoid(sum);
			}
		}

		return _ao;
	}

	public float BackPropagate(List<List<Movement>> targets, float N, float M)
	{
		float error = 0.0f;

		if(targets.Count() != _no)
		{
			MonoBehaviour.print("Error : targets and no not identical");
			return;
		}

		float[] output_deltas = [0.0f] * _no;

		foreach(int o in _no)
		{
			error = targets[o] - _ao[o];
			output_deltas[o] = Dsigmoid(_ao[o]) * error;
		}

		float[] hidden_deltas = [0.0f] * _nh;
		
		foreach(int h in _nh)
		{
			error = 0.0f;

			foreach(int o in _no)
			{
				error = (error + _output_deltas[o] * _wo[h][o])
			}

			hidden_deltas[h] = Dsigmoid(_ah[o]) * error;
		}

		foreach(int h in _nh)
		{
			foreach(int o in _no)
			{
				float change = output_deltas[o] * _ah[h];
				_wo[h][o] = (_wo[h][o] + N * change + M * _co[h][o]);
				_co[h][o] = change;
			}
		}

		foreach(int i in _ni)
		{
			foreach(int h in _nh)
			{
				float change = hidden_deltas[h] * _ai[i];
				_wi[i][h] = (_wi[i][h] + N * change + M * _ci[i][h]);
				_ci[i][h] = change;
			}
		}

		error = 0.0f;

		foreach(int o in targets.Count ())
		{
			error = (error + 0.5f * ((targets[o] - _ao[o]) * (targets[o] - _ao[o])));
		}

		return error;
	}

	/*public void Test(List<List<Movement>> inputs)
	{
		foreach(List<Movement> lm in inputs)
		{
			MonoBehaviour.print(
		}
	}*/

	public void Weights()
	{
		MonoBehaviour.print("Inputs weights : ");
		foreach(int i in _ni)
		{
			MonoBehaviour.print(_wi[i]);
		}
		MonoBehaviour.print("");
		MonoBehaviour.print("Outputs weights : ");
		foreach(int o in _no)
		{
			MonoBehaviour.print(_wo[o]);
		}
	}

	public void Train(List<List<Movement>> inputs, int interations = 1000, float N = 0.5f, float M = 0.1f)
	{
		foreach(int i in interations)
		{
			float error = 0.0f;

			foreach(List<Movement> lm in inputs)
			{

			}
		}
	}
}
