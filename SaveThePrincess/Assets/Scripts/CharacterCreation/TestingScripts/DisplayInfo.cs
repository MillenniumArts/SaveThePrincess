using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayInfo : MonoBehaviour {
	public Text[] textArray;
	public CreateCombination[] characters;

	void Update(){
		for(int i = 0; i < textArray.Length; i++){
			textArray[i].text = characters[i].GetCurrentComboString();
		}
	}
}
