using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public struct PlayerData{
//	public Vector3 position;
//	public Quaternion rotation;
//}

public class GlobalVariables : MonoBehaviour {

//	public bool[] switches;
	public bool deleteProgressAtStart; //zum Testen
	public bool autoSave;

	public string currentScene;
	public string lastScene;
	
	public float minotaurusSpeed = 3.5f;

	public static GlobalVariables Instance { get; private set; }

	public Dictionary<string, PlayerData> playerDataPerScene;

	public Vector3 savePoint = Vector3.zero;

	private GameObject player;
	
	public bool inCrawlArea;
	public bool crawling;
	public float crawlBugFix;


	void Awake(){
		
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		
		Instance = this;
		
		DontDestroyOnLoad(gameObject);

		if(deleteProgressAtStart){
			PlayerPrefs.DeleteAll();
			//TODO neue levelSequence
		}//else{
//			load();
//		}

		player = GameObject.FindWithTag("Player");
//		Debug.Log(player.transform.position.x);

		playerDataPerScene = new Dictionary<string, PlayerData>();
		currentScene = "Stadt";
		load();
	}
	

	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Weltenseele")){
			weltenseeleTeleport();			
		}
		
		crawlBugFix += Time.deltaTime;
		if(crawlBugFix >= 2f){
			inCrawlArea = false;
			crawlBugFix = 0f;
		}
	}

	private void weltenseeleTeleport(){
		if(Application.loadedLevelName == "Weltenseele"){
//			updatePlayerData();
			changeScene(GlobalVariables.Instance.lastScene);
		}else{
			updatePlayerData();
			changeScene("Weltenseele");
		}
	}

	void OnLevelWasLoaded(){
		player = GameObject.FindWithTag("Player");
		
		inCrawlArea = false;
		crawlBugFix = 0f;
	}

	public void load(){
		currentScene = PlayerPrefs.GetString("CurrentScene");
		lastScene = PlayerPrefs.GetString("LastScene");

		if(currentScene == "") currentScene = "Stadt";
		if(lastScene == "") lastScene = "Stadt";
		
		if(PlayerPrefs.GetInt("Crawling") != 0)	crawling = true;

	}

	public void save(){

		PlayerPrefs.SetString("CurrentScene", currentScene);
		PlayerPrefs.SetString("LastScene", lastScene);
		
		if(crawling){
			PlayerPrefs.SetInt("Crawling", 1);
		}

	}

	public void changeScene(string sceneName){
		lastScene = Application.loadedLevelName;
		currentScene = sceneName;
		Application.LoadLevel(sceneName);
	}

	public void updatePlayerData(){

		PlayerPrefs.SetFloat("PlayerPosX", player.transform.position.x);
		PlayerPrefs.SetFloat("PlayerPosY", player.transform.position.y);
		PlayerPrefs.SetFloat("PlayerPosZ", player.transform.position.z);

		PlayerPrefs.SetFloat("PlayerRotX", player.transform.rotation.x);
		PlayerPrefs.SetFloat("PlayerRotY", player.transform.rotation.y);
		PlayerPrefs.SetFloat("PlayerRotZ", player.transform.rotation.z);
		PlayerPrefs.SetFloat("PlayerRotW", player.transform.rotation.w);

		Debug.Log(Application.loadedLevelName + " Player X: " + PlayerPrefs.GetFloat("PlayerPosX"));
//
//		PlayerData pd = new PlayerData();
//		pd.position = player.transform.position;
//		pd.rotation = player.transform.rotation;
//		if(!playerDataPerScene.ContainsKey(Application.loadedLevelName)){
//			DontDestroyOnLoad(pd);
//			playerDataPerScene.Add(Application.loadedLevelName, pd);
//		}else{
//			playerDataPerScene[Application.loadedLevelName] = pd;
//		}

//		foreach(KeyValuePair<string, PlayerData> pData in playerDataPerScene){
//			Debug.Log(pData.Key + ": " + pData.Value.position + ", " + pData.Value.rotation);
//		}
	}

}
