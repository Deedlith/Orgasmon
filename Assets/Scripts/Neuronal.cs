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
			ihSums[i] = 0.0f;

		for (int i = 0; i < numOutput; ++i)
			hoSums[i] = 0.0f;

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
		return 1.0 / (1.0 + Math.Exp(-x));
	}

	private double HyperTanFunction(double x)
	{
		return Math.Tanh(x);
	}

	public static double[][] MakeMatrix(int rows, int cols)
	{
		double[][] result = new double[rows][];
		for (int i = 0; i < rows; ++i)
			result[i] = new double[cols];
		return result;
	}
}
