using UnityEngine;
using System.Collections;

public class BlendOut : MonoBehaviour {

	public bool done;

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
		if(alpha <= 1f){
			alpha += Time.deltaTime;
			if(alpha > 1f) alpha = 1f;
		}
		
		if(time >= 1.5f){
			done = true;
		}
	}
	
	void OnGUI(){
		GUI.color = new Color(GUI.color.r,GUI.color.g,GUI.color.b, alpha);
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), schwarz, ScaleMode.StretchToFill);
	}
}
