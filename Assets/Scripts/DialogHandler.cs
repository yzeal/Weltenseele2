using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogHandler : MonoBehaviour {

	public Dialog[] subtitles;

	//Sollten mehrere Dialoge kurz nacheinander ausgelöst werden, sollen sie nacheinander abgespielt werden. 
	private Queue<int> subtitlesToActivate = new Queue<int>();


	void Start () {
		for(int i = 0; i < subtitles.Length; i++){
			if(PlayerPrefs.GetInt(Application.loadedLevelName + "Subtitle" + i) != 0){
				Destroy(subtitles[i].gameObject);
			}
		}

	}
	

	void Update () {
		if(subtitlesToActivate.Count > 0){
			if(subtitles[subtitlesToActivate.Peek()] != null){
				subtitles[subtitlesToActivate.Peek()].Activate();
			}else{
				subtitlesToActivate.Dequeue();
			}
		}

	}

	//Sollten mehrere Dialoge kurz nacheinander ausgelöst werden, sollen sie nacheinander abgespielt werden. 
	//Alternativen wären z.B. den vorherigen abzubrechen, zu pausieren oder den neuen gar nicht zu aktivieren.
	public void Activate(int index){
		if(index < subtitles.Length && subtitles[index] != null){
			subtitlesToActivate.Enqueue(index);
		}
	}

	public void Pause(int index){
		if(index < subtitles.Length && subtitles[index] != null){
			subtitles[index].DeactivatePause();
		}
	}

	public void StopAndReset(int index){
		if(index < subtitles.Length && subtitles[index] != null){
			subtitles[index].DeactivateReset();
		}
	}


}
