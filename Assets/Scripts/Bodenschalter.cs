using UnityEngine;
using System.Collections;

public class Bodenschalter : MonoBehaviour {

	public int id;
	public bool on;

	public bool save = true; //Schalter, die Wände verschieben, sollten nie gespeichert werden, weil manche Level sonst vom Eingang aus nicht mehr lösbar sind.

	public GameObject bodenPlatte;
	public GameObject dachPlatte;
	public ParticleSystem partikelOben;
	public ParticleSystem partikelUnten;

	public Color offColor;
	public Color onColor;

	private bool justActivated;
	private Material matBoden;
	private Material matDach;


	void Start () {

		if(PlayerPrefs.GetInt(Application.loadedLevelName + "Switch" + id) == 0){
			on = false;
		}else{
			on = true;
		}

		partikelOben.startColor = onColor;
		partikelUnten.startColor = onColor;

		matDach = dachPlatte.GetComponent<MeshRenderer>().material;
		matBoden = bodenPlatte.GetComponent<MeshRenderer>().material;

		matBoden.SetColor("_Color", offColor);
		matDach.SetColor("_Color", offColor);

		justActivated = false;
	}
	

	void Update () {
		if(on){
			matBoden.SetColor("_Color", onColor);
			matDach.SetColor("_Color", onColor);
			partikelOben.Play();
			partikelUnten.Play();
		}else{
			matBoden.SetColor("_Color", offColor);
			matDach.SetColor("_Color", offColor);
			partikelOben.Stop();
			partikelOben.Clear();
			partikelUnten.Stop();
			partikelUnten.Clear();
		}

	}

	void OnTriggerEnter(Collider other) {
		if(!justActivated && other.CompareTag("Player")){
			if(!on){
				on = true;
				if(save && GlobalVariables.Instance.autoSave){
					PlayerPrefs.SetInt(Application.loadedLevelName + "Switch" + id, 1);
				}
			}else {
					on = false;
					if(save && GlobalVariables.Instance.autoSave){
						PlayerPrefs.SetInt(Application.loadedLevelName + "Switch" + id, 0);
					}
				}

			justActivated = true;
		}

	}


	void OnTriggerExit(Collider other){
		if(other.CompareTag("Player")){
			justActivated = false;
		}
	}
}
