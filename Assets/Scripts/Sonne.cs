using UnityEngine;
using System.Collections;

public class Sonne : MonoBehaviour {

	public float uhrzeit = 12.0f;
	public float stundeInMin = 3.0f;

	public float sunHeight; //Radius

	private float sunAngle = 0.0f;
	private float deltaAngle = 0f;
	private float sunAngleStep = 15.0f; //15° pro Stunde = 360° insgesamt

	public Light pointLight;
	public Light directionalLight;
	public GameObject ball;

	private float zeitKonstante;

	// Use this for initialization
	void Start () {

		zeitKonstante = 60f / stundeInMin / 60f;

		if(PlayerPrefs.HasKey("SunAngle")){
			sunAngle = PlayerPrefs.GetFloat("SunAngle");
		}


		pointLight.transform.position = new Vector3(0f, sunHeight, 0f);
		ball.transform.position = pointLight.transform.position;

		directionalLight.transform.Rotate(new Vector3(sunAngle + 90f, 0f, 0f));
		
		
		pointLight.transform.RotateAround(Vector3.zero, new Vector3(1f, 0f, 0f), sunAngle);
		ball.transform.RotateAround(Vector3.zero, new Vector3(1f, 0f, 0f), sunAngle);


	}
	
	// Update is called once per frame
	void Update () {
		deltaAngle = Time.deltaTime * zeitKonstante;
		moveSun();
		changeSunlight();
	}

	private void changeSunlight(){

		if(sunAngle > 270f){
			directionalLight.intensity = sunAngle / 360f;
//			directionalLight.color = Color.
		}else if(sunAngle < 90){
			directionalLight.intensity = (360f - sunAngle) / 360f;
		}else{
			directionalLight.intensity = 0f;
		}



//		if(la > 180){
//			directionalLight.intensity = 0f;
//		}
	}

	public void moveSun(){
		directionalLight.transform.Rotate(new Vector3(deltaAngle, 0f, 0f));
		

		pointLight.transform.RotateAround(Vector3.zero, new Vector3(1f, 0f, 0f), deltaAngle);
		ball.transform.RotateAround(Vector3.zero, new Vector3(1f, 0f, 0f), deltaAngle);


		sunAngle += deltaAngle;
		if(sunAngle > 360f){
			sunAngle -= 360f;
		}

		deltaAngle = 0f;
	}

	public void saveSun(){
		PlayerPrefs.SetFloat("SunAngle", sunAngle);
	}
}
