using UnityEngine;
using System.Collections;

public class DialogTrigger : MonoBehaviour {

	public int number;
	private DialogHandler handler;


	void Start () {
		handler = GameObject.Find("DialogHandler").GetComponent<DialogHandler>();
	}

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player")){
			handler.Activate(number);
		}
	}
}
