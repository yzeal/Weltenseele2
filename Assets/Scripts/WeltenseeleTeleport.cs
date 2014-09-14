using UnityEngine;
using System.Collections;

public class WeltenseeleTeleport : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Weltenseele")){
			if(Application.loadedLevelName == "Weltenseele"){
				GlobalVariables.Instance.updatePlayerData();
				GlobalVariables.Instance.changeScene(GlobalVariables.Instance.lastScene);
			}else{
				GlobalVariables.Instance.updatePlayerData();
				GlobalVariables.Instance.changeScene("Weltenseele");
			}

		}

	}
}
