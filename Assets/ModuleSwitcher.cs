﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using deVoid.Utils;

public class ModuleSwitcher : MonoBehaviour
{   
    public LeModule childModule;
    public List<LeModule> modules = new List<LeModule>();
    public float scaleFactor = 0.1f;
    [Range(0f,1f)]
    public float time = 1;
    public int count = 0;
    bool switchEnumerating = false;

    private void OnEnable()
	{
		Signals.Get<LeModule.OnStart>().AddListener(onModuleStart);
	}

	private void OnDisable()
	{
		Signals.Get<LeModule.OnStart>().RemoveListener(onModuleStart);
	}

	public void onModuleStart(LeModule newModule)
	{	
        modules.Add(newModule);
		count++;
	}

    // Start is called before the first frame update
    void Start()
    {
        childModule = gameObject.GetComponent<LeModule>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!switchEnumerating){
            StartCoroutine(Switch());
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            
            foreach(LeModule child in childModule.children){
                child.Subdivide(LeModule.axis.x);
            }
        }
    }
    
    IEnumerator Switch(){
        
        LeModule module = childModule;
        if(ExtRandom<bool>.Chance(1,2)){
            
            LeModule.axis newAxis = RandomAxis();
            
            module.Subdivide(newAxis);
            
            foreach(var child in module.children){
                Renderer renderer = child.meshGo.GetComponent<Renderer>();
                RandomColor(renderer);
                child.gameObject.AddComponent<ModuleSwitcher>();
            }
        }else{
            module.UnDivide();
        }
        
        
        // wait a second before allowing this function to run
        switchEnumerating = true;
        yield return new WaitForSeconds(time);
        switchEnumerating = false;
    }

    LeModule.axis RandomAxis(){
        // Get a random axis
        List<string> axis = new List<string>();
        axis.Add(Module.axis.x.ToString());
        axis.Add(Module.axis.y.ToString());
        axis.Add(Module.axis.z.ToString());
        var choice = ExtRandom<string>.WeightedChoice(axis, new int[]{33,33,33});
        LeModule.axis newAxis = LeModule.axis.x;
        switch(choice){
            case "x":
                newAxis = LeModule.axis.x;
            break;
            case "y":
                newAxis = LeModule.axis.y;
            break;
            case "z":
                newAxis = LeModule.axis.z;
            break;					
        }
        return newAxis;
    }

    public void RandomColor(Renderer renderer){
        // Change colour randomly
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        LeModule module = childModule;
        if(module.meshGo){
            float r = Random.Range(0.0f, 1.0f);
            float g = Random.Range(0.0f, 1.0f);
            float b = Random.Range(0.0f, 1.0f);
            props.SetColor("_Color", new Color(r, g, b));
            if(module.size.x > module.size.z){
                props.SetVector("_ST", new Vector4(module.size.x * scaleFactor, module.size.y * scaleFactor,1,1));
            }else{
                props.SetVector("_ST", new Vector4(module.size.z * scaleFactor, module.size.y * scaleFactor,1,1));
            }
            renderer.SetPropertyBlock(props);
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireCube(transform.position, Vector3.one * 2.26f);
    // }
}