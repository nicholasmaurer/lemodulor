﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modulor : MonoBehaviour {

	public List<float> redSeries = new List<float>(){
		0.6f,
		0.9f,
		1.5f,
		2.4f,
		3.9f,
		6.3f,
		10.2f,
		16.5f,
		26.7f,
		43.2f,
		69.8f,
		113.0f,
		182.9f,
		295.9f,
		478.8f,
		774.7f,
		1253.5f,
		2028.2f,
		3981.6f,
		5309.8f,
		8591.4f,
		13901.3f,
		22492.7f,
		36394.0f,
		58886.7f,
		95280.7f
	};

	public List<float> blueSeries = new List<float>(){
		1.1f,
		1.8f,
		3.0f,
		4.8f,
		7.8f,
		12.6f,
		20.4f,
		33.0f,
		53.4f,
		86.3f,
		139.7f,
		226.0f,
		365.8f,
		591.8f,
		957.6f,
		1549.4f,
		2506.9f,
		4056.3f,
		6563.3f,
		10619.6f,
		17182.9f,
		27802.5f,
		44985.5f,
		72788.0f,
		117773.5f
	};

	public float measure;
	public float distance;
	public List<float> divs;
	
	private void Update()
	{
		divs = new List<float>();
		measure = 0;
		for(int i = 0; i < redSeries.Count; i++){
			measure = redSeries[i];
			if(measure >= distance){
				break;
			}
		}
	}

	private void OnGUI()
	{
		if(GUILayout.Button("")){
			
		}
	}
}