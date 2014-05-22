using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Neuronal 
{
	private int numInput;
	private int numHidden;
	private int numOutput;

	private double[] inputs;
	private double[][] ihWeights; // input-to-hidden
	private double[] ihSums;
	private double[] ihBiases;
	private double[] ihOutputs;
	private double[][] hoWeights;  // hidden-to-output
	private double[] hoSums;
	private double[] hoBiases;
	private double[] outputs;

	public Neuronal(int numInput, int numHidden, int numOutput)
	{
		this.numInput = numInput;
		this.numHidden = numHidden;
		this.numOutput = numOutput;
		inputs = new double[numInput];
		ihWeights = MakeMatrix(numInput, numHidden);
		ihSums = new double[numHidden];
		ihBiases = new double[numHidden];
		ihOutputs = new double[numHidden];
		hoWeights = MakeMatrix(numHidden, numOutput);
		hoSums = new double[numOutput];
		hoBiases = new double[numOutput];
		outputs = new double[numOutput];
	}

	public void SetWeights(double[] weights)
	{
		int numWeights = (numInput * numHidden) +
			(numHidden * numOutput) + numHidden + numOutput;
		if (weights.Length != numWeights)
			throw new Exception("xxxxxx");
		int k = 0;

		for (int i = 0; i < numInput; ++i)
			for (int j = 0; j < numHidden; ++j)
				ihWeights[i][j] = weights[k++];
		for (int i = 0; i < numHidden; ++i)
			ihBiases[i] = weights[k++];
		for (int i = 0; i < numHidden; ++i)
			for (int j = 0; j < numOutput; ++j)
				hoWeights[i][j] = weights[k++];
		for (int i = 0; i < numOutput; ++i)
			hoBiases[i] = weights[k++];
	}

	public double[] ComputeOutputs(double[] xValues)
	{
		if (xValues.Length != numInput)
			throw new Exception("xxxxxx");
		for (int i = 0; i < numHidden; ++i)
			ihSums[i] = 0.0;
		for (int i = 0; i < numOutput; ++i)
			hoSums[i] = 0.0;
		for (int i = 0; i < xValues.Length; ++i)
			this.inputs[i] = xValues[i];
		for (int j = 0; j < numHidden; ++j)
			for (int i = 0; i < numInput; ++i)
				ihSums[j] += this.inputs[i] * ihWeights[i][j];
		for (int i = 0; i < numHidden; ++i)
			ihSums[i] += ihBiases[i];
		for (int i = 0; i < numHidden; ++i)
			ihOutputs[i] = SigmoidFunction(ihSums[i]);
		for (int j = 0; j < numOutput; ++j)
			for (int i = 0; i < numHidden; ++i)
				hoSums[j] += ihOutputs[i] * hoWeights[i][j];
		for (int i = 0; i < numOutput; ++i)
			hoSums[i] += hoBiases[i];
		for (int i = 0; i < numOutput; ++i)
			this.outputs[i] = HyperTanFunction(hoSums[i]);
		double[] result = new double[numOutput];
		this.outputs.CopyTo(result, 0);
		return result;
	}

	private double SigmoidFunction(double x)
	{
		if (x < -45.0) return 0.0;
		else if (x > 45.0) return 1.0;
		else return 1.0 / (1.0 + Math.Exp(-x));
	}

	private double HyperTanFunction(double x)
	{
		if (x < -10.0) return -1.0;
		else if (x > 10.0) return 1.0;
		else return Math.Tanh(x);
	}

	public double[][] MakeMatrix(int rows, int cols)
	{
		double[][] result = new double[rows][];
		for (int i = 0; i < rows; ++i)
			result[i] = new double[cols];
		return result;
	}

	/*List<List<Movement>> _inputs;
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

	public void Test(List<List<Movement>> inputs)
	{
		foreach(List<Movement> lm in inputs)
		{
			MonoBehaviour.print(
		}
	}

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
	}*/
}
