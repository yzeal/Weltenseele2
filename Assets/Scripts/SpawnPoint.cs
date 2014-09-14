using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Cameras;
//using com.ootii.AI.Controllers;

public class SpawnPoint : MonoBehaviour {

//	public int currentLevelNumber;
//	public ExitDirection spawnPointDirection;
//	public ExitLevel spawnPointLevel;

//	public GameObject mainCameraRig;

	// Use this for initialization
	void Start () {

//		Debug.Log("PD Start");
//		foreach(KeyValuePair<string, PlayerData> pData in GlobalVariables.Instance.playerDataPerScene){
//			Debug.Log(pData.Key + ": " + pData.Value.position + ", " + pData.Value.rotation);
//		}
//		Debug.Log("PD Ende");
//		Debug.Log("Contains " + Application.loadedLevelName + ": " + GlobalVariables.Instance.playerDataPerScene.ContainsKey(Application.loadedLevelName));

		Vector3 newPos = new Vector3();

		newPos.x = PlayerPrefs.GetFloat("PlayerPosX");
		newPos.y = PlayerPrefs.GetFloat("PlayerPosY");
		newPos.z = PlayerPrefs.GetFloat("PlayerPosZ");

		Debug.Log(Application.loadedLevelName + " Player X SpawPoint: " + PlayerPrefs.GetFloat("PlayerPosX"));

		Debug.Log("newPos: " + newPos);

		Quaternion newRot = new Quaternion();

		newRot.x = PlayerPrefs.GetFloat("PlayerRotX");
		newRot.y = PlayerPrefs.GetFloat("PlayerRotY");
		newRot.z = PlayerPrefs.GetFloat("PlayerRotZ");
		newRot.w = PlayerPrefs.GetFloat("PlayerRotW");

//		if(!GlobalVariables.Instance.playerDataPerScene.ContainsKey(Application.loadedLevelName)){
		if(newPos == Vector3.zero || Application.loadedLevelName == "Weltenseele"){
			GameObject player = GameObject.FindWithTag("Player");
			player.transform.position = transform.position + Vector3.up;
			player.transform.rotation = transform.rotation;
			GameObject.FindWithTag("MainCameraRig").transform.position = transform.position - 20f * player.transform.forward;

			if(GlobalVariables.Instance.autoSave) GlobalVariables.Instance.save();
		}else{
//			PlayerData pd = GlobalVariables.Instance.playerDataPerScene[Application.loadedLevelName];
//
//			GameObject player = GameObject.FindWithTag("Player");
//			player.transform.position = pd.position;
//			player.transform.rotation = pd.rotation;
//			GameObject.FindWithTag("MainCameraRig").transform.position = pd.position - 20f * player.transform.forward;

//			PlayerData pd = GlobalVariables.Instance.playerDataPerScene[Application.loadedLevelName];
			
			GameObject player = GameObject.FindWithTag("Player");
			player.transform.position = newPos + Vector3.up;
			player.transform.rotation = newRot;
			GameObject.FindWithTag("MainCameraRig").transform.position = newPos - 20f * player.transform.forward;

			if(GlobalVariables.Instance.autoSave) GlobalVariables.Instance.save();
			Debug.Log("Position from list.");
		}

	}
	

}
