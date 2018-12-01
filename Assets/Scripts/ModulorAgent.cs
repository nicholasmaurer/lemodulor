﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulorAgent : MonoBehaviour {

	public string id;
	public BonedCube bc;
	public Vector3 center = Vector3.zero;
	public bool offGround;
	public float height;
	public List<float> storeys = new List<float>();
	public BoxCollider boxCollider;
	public Rigidbody rb;
	public Bounds bounds;
	public SkinnedMeshRenderer skinnedMesh;
	public List<Vector3> topVs = new List<Vector3>();
	// Use this for initialization
	void Start () {

		this.gameObject.name = id;
		
		// Set the size of the volume to the closest measurement contained in the red series
		float length = LeModular.GetClosest(bc.length * 100) / 100;
		float width = LeModular.GetClosest(bc.width * 100) / 100;
		float height = LeModular.GetClosest(bc.height * 100) / 100;
		bc.Set(length,width,height);
		
		// get the center of the volume
		skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
		skinnedMesh.updateWhenOffscreen = true;
		bounds = skinnedMesh.bounds;
		center = bounds.center;
		topVs.Add(bounds.center);

		topVs.Add(bounds.center + new Vector3(width/2,height/2,length/2));
		topVs.Add(bounds.center + new Vector3(-width/2,height/2,length/2));
		topVs.Add(bounds.center + new Vector3(width/2,height/2,-length/2));
		topVs.Add(bounds.center + new Vector3(-width/2,height/2,-length/2));
		boxCollider = bc.gameObject.AddComponent<BoxCollider>();
		boxCollider.enabled = true;
		boxCollider.isTrigger = true;
		boxCollider.center = center;
		boxCollider.size = new Vector3(bc.width, bc.height, bc.length);
		rb = bc.gameObject.AddComponent<Rigidbody>();
		rb.isKinematic = true;

		return;
		// Instantiate piloti if the volume is off the ground
		if(offGround){
			var piloti = Resources.Load<GameObject>("piloti");
			var pos = center;
			pos.y -= bc.height/2;
			piloti = Instantiate(piloti, pos, Quaternion.identity, this.transform);
			var pilotiScript = piloti.GetComponent<Pilotis>();
			// get the shortest side
			pilotiScript.length = bc.width;
			pilotiScript.width = bc.length;
			var a = pilotiScript.length > pilotiScript.width ? pilotiScript.width : pilotiScript.length;	// get the shortet side
			a *= 0.1f;
			a = Mathf.Clamp(a, 0.1f, 0.75f);
			pilotiScript.pilotiWidth = a;
			pilotiScript.width -= a;
			pilotiScript.length -= a;
			var ground = bc.a.position;
			ground.y = 0;
			height = Vector3.Distance(bc.a.position, ground);
		}

		List<float> floors = LeModular.Divisions(bc.height);
		var floorBC = BonedCube.Make();
		floorBC.transform.SetParent(this.transform);
		floorBC.a.transform.position = this.bc.a.transform.position;
		floorBC.Set(bc.length, bc.width, 0.1f);
		var _floors = BuildFloors(floors);
		if(_floors != null){
			foreach(var floor in _floors){
				floorBC = BonedCube.Make();
				floorBC.transform.SetParent(this.transform);
				floorBC.a.transform.position = this.bc.a.transform.position + new Vector3(0, floor, 0);
				floorBC.Set(bc.length, bc.width, 0.1f);
			}
		}

		// bc.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
	}

	List<float> BuildFloors(List<float> floors){
		// get random number that is the =< the size of the list
		int a = Random.Range(0, floors.Count);
		int b = Random.Range(floors.Count - a, floors.Count);
		Debug.Log(string.Format("{0} a {1} b {2}", id, a, b));
		if(a + b > floors.Count){
			Debug.Log(string.Format("! a {0} b {1}", a, b));
			return BuildFloors(floors);
		}
		return floors.GetRange(a,b);;
	}
	private void OnTriggerEnter(Collider other)
	{

		Debug.Log(string.Format("{0} has collided with {1}", id, other.gameObject.name));
	}

	// private void OnTriggerStay(Collider other)
	// {
	// 	Debug.Log("Modulor Agent  On Trigger Stay");
	// }
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDrawGizmos() {
		
		foreach(var vec in topVs){
			Gizmos.DrawCube(this.transform.TransformPoint(vec), Vector3.one * 0.01f);
		}
	}
}
