using UnityEngine;
using System.Collections;

public class SavePoint : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player")){
			GlobalVariables.Instance.savePoint = transform.position;
		}
	}
}
