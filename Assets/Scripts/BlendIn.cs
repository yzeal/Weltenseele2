using UnityEngine;
using System.Collections;

public class BlendIn : MonoBehaviour {

	private float time = 0f;

	private Texture2D schwarz;
	private float alpha = 1f;


	void Start () {
		schwarz = new Texture2D(1,1);
		schwarz.SetPixel(0,0, new Color(0f,0f,0f));
		schwarz.Apply();
	}
	

	void Update () {
		time += Time.deltaTime;
		if(time >= 0.5f){
			alpha -= Time.deltaTime;
		}

		if(alpha <= 0f){
			Destroy(gameObject);
		}
	}

	void OnGUI(){
		GUI.color = new Color(GUI.color.r,GUI.color.g,GUI.color.b, alpha);
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), schwarz, ScaleMode.StretchToFill);
	}
}
