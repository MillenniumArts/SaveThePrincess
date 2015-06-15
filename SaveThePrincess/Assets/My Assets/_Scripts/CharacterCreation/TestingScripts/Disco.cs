using UnityEngine;
using System.Collections;

public class Disco : MonoBehaviour {
	public CreateCombination[] swapRef;
	public float delay = 1.0f;
	private bool keepDisco = false;

	public void StartDisco(){
		if(!keepDisco){
			keepDisco = true;
			StartCoroutine("GoDiscoGo");
		}
	}

	public void StopDisco(){
		if(keepDisco)
			keepDisco = false;
	}

	IEnumerator GoDiscoGo(){
		for(int i = 0; i < swapRef.Length; i++){
			swapRef[i].UseRandomCombo();
		}
		yield return new WaitForSeconds(delay);
		if(keepDisco){
			StartCoroutine("GoDiscoGo");
		}
	}
}
