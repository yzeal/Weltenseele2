using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {

	private AudioSource audioSource;

	void Start(){
		audioSource = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other){
		
		if(other.CompareTag("Player")){
			audioSource.Play();
			Debug.Log ("play");
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.CompareTag("Player")){
			audioSource.Pause();
			Debug.Log("stop");
		}
	}
}
