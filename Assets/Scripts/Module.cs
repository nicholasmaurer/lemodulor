﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class Module : MonoBehaviour {

	public class ModuleStart : ASignal <Module> {};
	public enum axis {x,y,z}
	public axis divAxis = axis.x;
	public string id = "Node";
	public int divs = 0;
	public Module parentNode;
	public List<Module> childNodes;
	public Vector3 size = Vector3.one;
	public GameObject meshGo;
	// public float margin = 0; // how much space on each side to leave between the next
	
	private void Start()
	{
		id += parentNode ? " " + parentNode.childNodes.IndexOf(this).ToString(): "";
		gameObject.name = id;
		Signals.Get<ModuleStart>().Dispatch(this);
	}

	public void OnDestroy()
	{
		GameObject.Destroy(this.gameObject);
	}

	void Update()
	{	
		// todo: switch statement to set the current axis count
		
		if(divs > 0){
			
			if(divs > childNodes.Count){
				while(divs > childNodes.Count){
					var go = new GameObject();
					go.transform.SetParent(this.transform);
					go.transform.localPosition = Vector3.zero;
					var node = go.AddComponent<Module>();
					childNodes.Add(node);
					node.parentNode = this;
					node.meshGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
					node.meshGo.transform.SetParent(node.transform);
					node.meshGo.transform.localPosition = Vector3.zero;
					// Debug.Log("Added child");
				}
			}else if(divs < childNodes.Count){
				while(divs < childNodes.Count){
					var node = childNodes[childNodes.Count-1];
					GameObject.Destroy(node.gameObject);
					childNodes.RemoveAt(childNodes.Count-1);
					// Debug.Log("Removed child");
				}
			}		
		}
		// Apply scale based child count
		if(childNodes != null){
			if(childNodes.Count > 0){
				foreach (Module item in childNodes)
				{
					switch(divAxis){
						case axis.x:
						item.size.x = size.x / divs;
						break;
						case axis.y:
						item.size.y = size.y / divs;
						break;
						case axis.z:
						item.size.z = size.z / divs;
						break;
					}
				}
				// margin = Mathf.Clamp(margin,0f, (size.x / divs) - 0.01f);	// 0.01 because if we use the exact value the model doesn't render properly
				foreach (Module item in childNodes) {
					var scale = item.meshGo.transform.localScale;
					switch(divAxis){
						case axis.x:
							scale.x = item.size.x;
							scale.y = size.y;
							scale.z = size.z;
							// scale.x -= margin;
						break;
						case axis.y:
							scale.x = size.x;
							scale.y = item.size.y;
							scale.z = size.z;
							// scale.y -= margin;
						break;
						case axis.z:
							scale.x = size.x;
							scale.y = size.y;
							scale.z = item.size.z;
							// scale.z -= margin;
						break;					
					}
					item.meshGo.transform.localScale = scale;
				}
				if(parentNode){
					foreach (Module item in childNodes)
					{
						switch(divAxis){
							case axis.x:
								size.y = parentNode.size.y;
								size.z = parentNode.size.z;
							break;
							case axis.y:
								size.x = parentNode.size.x;
								size.z = parentNode.size.z;
							break;
							case axis.z:
								size.x = parentNode.size.x;
								size.y = parentNode.size.y;
							break;					
						}
					}
					foreach (Module item in childNodes)
					{
						switch(parentNode.divAxis){
							case axis.x:
								size.x = parentNode.size.x / parentNode.divs;
							break;
							case axis.y:
								size.y = parentNode.size.y / parentNode.divs;
							break;
							case axis.z:
								size.z = parentNode.size.z / parentNode.divs;
							break;					
						}
					}	
					if(parentNode.parentNode){
						foreach (Module item in childNodes)
						{
							switch(parentNode.parentNode.divAxis){
								case axis.x:
									size.x = parentNode.parentNode.size.x / parentNode.parentNode.divs;
								break;
								case axis.y:
									size.y = parentNode.parentNode.size.y / parentNode.parentNode.divs;
								break;
								case axis.z:
									size.z = parentNode.parentNode.size.z / parentNode.parentNode.divs;
								break;					
							}
						}
						if(parentNode.parentNode.parentNode){
							foreach (Module item in childNodes)
							{
								switch(parentNode.parentNode.parentNode.divAxis){
									case axis.x:
										size.x = parentNode.parentNode.parentNode.size.x / parentNode.parentNode.parentNode.divs;
									break;
									case axis.y:
										size.y = parentNode.parentNode.parentNode.size.y / parentNode.parentNode.parentNode.divs;
									break;
									case axis.z:
										size.z = parentNode.parentNode.parentNode.size.z / parentNode.parentNode.parentNode.divs;
									break;					
								}
							}
						}else{
							foreach (Module item in childNodes)
							{
								switch(parentNode.parentNode.divAxis){
									case axis.x:
										size.x = parentNode.parentNode.size.x / parentNode.parentNode.divs;
									break;
									case axis.y:
										size.y = parentNode.parentNode.size.y / parentNode.parentNode.divs;
									break;
									case axis.z:
										size.z = parentNode.parentNode.size.z / parentNode.parentNode.divs;
									break;					
								}
							}
						}
					}else{
						foreach (Module item in childNodes) {
							switch(parentNode.divAxis){
								case axis.x:
									size.x = parentNode.size.x / parentNode.divs;
								break;
								case axis.y:
									size.y = parentNode.size.y / parentNode.divs;
								break;
								case axis.z:
									size.z = parentNode.size.z / parentNode.divs;
								break;					
							}
						}
					}
				}
				foreach (Module item in childNodes)
				{
					var pos = item.transform.localPosition;
					int index = childNodes.IndexOf(item);
					float a = 0;
					float b = 0;
					float offset = 0;
					switch(divAxis){
						case axis.x:
							a = size.x /2;
							b = a / divs;
							offset = a - b;
							pos.x = (item.size.x * index) - offset;	
						break;
						case axis.y:
							a = size.y /2;
							b = a / divs;
							offset = a - b;
							pos.y = (item.size.y * index) - offset;	
						break;
						case axis.z:
							a = size.z /2;
							b = a / divs;
							offset = a - b;
							pos.z = (item.size.z * index) - offset;	
						break;					
					}
					item.transform.localPosition = pos;
				}
				
			}	
		}

		if(meshGo)
			if(divs > 0) meshGo.SetActive(false); else meshGo.SetActive(true);
	}

	private void OnGUI()
	{
		
	}
}