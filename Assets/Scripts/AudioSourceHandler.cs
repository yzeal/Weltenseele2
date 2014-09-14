using UnityEngine;
using System.Collections;

public class AudioSourceHandler : MonoBehaviour {

	public AudioSource[] audioSources;

	public AudioSource golem;

	private bool golemFollow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void golemFollowStart(){
		if(!golemFollow){
			golemFollow = true;
			foreach(AudioSource audioSource in audioSources){
				audioSource.enabled = false;
			}

			golem.Play();
		}
	}

	public void golemFollowStop(){
		if(golemFollow){
			golemFollow = false;
			foreach(AudioSource audioSource in audioSources){
				audioSource.enabled = true;
			}
			
			golem.Stop();
		}
	}
}
