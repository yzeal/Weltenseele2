using UnityEngine;
using System.Collections;

public class Gas : MonoBehaviour {

	public Vector3[] positions;

	public ParticleSystem smoke;

	private int currentPositionIndex;
	

	public void InitializeGas(){
		float time = 0f;
		
		for(int i = 0; i < positions.Length; i++){
			Invoke("StartGas", time);
			time += 1f;
		}
	}

	private void StartGas(){
		Instantiate(smoke, positions[currentPositionIndex], Quaternion.identity);
		currentPositionIndex++;
	}
}
