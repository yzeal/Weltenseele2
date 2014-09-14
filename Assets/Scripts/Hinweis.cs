using UnityEngine;
using System.Collections;

public class Hinweis : MonoBehaviour {

	public Bodenschalter[] bodenschalter;

	public Color offColor;
	public Color onColor;

	private bool on;

	private Material mat;


	void Start () {

		mat = GetComponentInChildren<MeshRenderer>().material;

		if(bodenschalter.Length > 0){
			onColor = bodenschalter[0].onColor;
			offColor = bodenschalter[0].offColor;
		}

		if(on){
			mat.SetColor("_Color", onColor);
		}else{
			mat.SetColor("_Color", offColor);
		}
	}
	

	void Update () {
		on = true;
		foreach(Bodenschalter schalter in bodenschalter){
			if(!schalter.on){
				on = false;
				break;
			}
		}

		if(on){
			mat.SetColor("_Color", onColor);
		}else{
			mat.SetColor("_Color", offColor);
		}
	}
}
