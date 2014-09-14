using UnityEngine;
using System.Collections;

public class AudioAnalyser : MonoBehaviour {

	public GameObject sphere;

	private Texture2D schwarz;

	private float[] spectrum;

	private float fMax;

	private float[] midiNotes = {0,0,0,0,0,0,0,0,0,0,0,0}; // C-1 bis C10
	private float[] test12 = {0,0,0,0,0,0,0,0,0,0,0,0};
	private float[] bands = {0,0,0,0,0,0,0,0};

	private float y = 0f;

	private float lastTime;
	private int beat = 0;

	// Use this for initialization
	void Start () {
		fMax = AudioSettings.outputSampleRate/2f;

		schwarz = new Texture2D(1,1);
		schwarz.SetPixel(0,0, new Color(0f,0f,0f,0.5f));
		schwarz.Apply();

		lastTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		y += Time.deltaTime*1f;
		transform.position += Vector3.up * Time.deltaTime;
		beat++;

		spectrum = AudioListener.GetSpectrumData(1024, 0, FFTWindow.Hamming);
		
		bands[0] += Mathf.Round(BandVol(0f, 16f));
		bands[1] += Mathf.Round(BandVol(16f, 32f));
		bands[2] += Mathf.Round(BandVol(32f, 512f));
		bands[3] += Mathf.Round(BandVol(512f, 1024f));
		bands[4] += Mathf.Round(BandVol(1024f, 2048f));
		bands[5] += Mathf.Round(BandVol(2048f, 4196f));
		bands[6] += Mathf.Round(BandVol(4196f, 8192f));
		bands[7] += Mathf.Round(BandVol(8192f, 16384f));
		
		if(Time.time - lastTime > 1f){


			for(int i = 0; i < bands.Length; i++){
				float band = bands[i] / beat;
				if(band > 0.3f){
					Instantiate(sphere, sphere.transform.position + new Vector3(i * 8f, y, 0f), Quaternion.identity);
				}
				bands[i] = 0; 

			}

			lastTime = Time.time;
			beat = 0;
			Debug.Log ("tap");
		}



//		spectrum = AudioListener.GetSpectrumData(1024, 0, FFTWindow.Hamming);
//
//		string text = "";
//		text += Mathf.Round(BandVol(0f, 16f)) + ", ";
//		text += Mathf.Round(BandVol(16f, 32f)) + ", ";
//		text += Mathf.Round(BandVol(32f, 512f)) + ", ";
//		text += Mathf.Round(BandVol(512f, 1024f)) + ", ";
//		text += Mathf.Round(BandVol(1024f, 2048f)) + ", ";
//		text += Mathf.Round(BandVol(2048f, 4196f)) + ", ";
//		text += Mathf.Round(BandVol(4196f, 8192f)) + ", ";
//		text += Mathf.Round(BandVol(8192f, 16384f)) + "";
//		Debug.Log(text);
		
	}

	private float  BandVol(float fLow, float fHigh){
		
		fLow = Mathf.Clamp(fLow, 20, fMax); // limit low...
		fHigh = Mathf.Clamp(fHigh, fLow, fMax); // and high frequencies
		// get spectrum
//		audio.GetSpectrumData(freqData, 0, FFTWindow.BlackmanHarris);
//		spectrum = AudioListener.GetSpectrumData(1024, 0, FFTWindow.BlackmanHarris);
		int n1 = Mathf.FloorToInt(fLow * 1024 / fMax);
		int n2 = Mathf.FloorToInt(fHigh * 1024 / fMax);
		float sum = 0;
		// average the volumes of frequencies fLow to fHigh
		for (int i = n1; i <= n2; i++){
			sum += spectrum[i];
		}
//		return sum / (n2 - n1 + 1);
		return sum;
	}


//	void OnGUI(){
//
//		GUI.DrawTexture(new Rect(45f, Screen.height/2f - y, 3f, 3f), schwarz, ScaleMode.StretchToFill);
//
//		for(int i = 0; i < 1024; i++){
//			if(spectrum[i] > 0.01f){
//				GUI.DrawTexture(new Rect(50f + i, Screen.height/2f - y, spectrum[i]*100f, spectrum[i]*100f), schwarz, ScaleMode.StretchToFill);
//			}
//		}
//
//		GUI.DrawTexture(new Rect(1076f, Screen.height/2f - y, 3f, 3f), schwarz, ScaleMode.StretchToFill);
//
//	}
}




