using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class levelselect : MonoBehaviour {

	EventSystem eventSystem;

	// Use this for initialization
	void Start () {
		//sys = this.GetComponent<EventSystem>();
		eventSystem = this.GetComponent<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
			if(Input.GetButtonDown("Submit") && eventSystem.currentSelectedGameObject.name == "level1"){
				SceneManager.LoadScene("BlueCity");
			}
			if(Input.GetButtonDown("Submit") && eventSystem.currentSelectedGameObject.name == "level2"){
				SceneManager.LoadScene("mels");
			}
			if(Input.GetButtonDown("Submit") && eventSystem.currentSelectedGameObject.name == "level3"){
				SceneManager.LoadScene("level_test1");
			}
			
			if(Input.GetButtonDown("Submit") && eventSystem.currentSelectedGameObject.name == "level4"){
				SceneManager.LoadScene("WOW");
			}
		
	}
	
	
	
}
