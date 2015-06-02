using UnityEngine;
using System.Collections;

public class InnController : MonoBehaviour {

	public PlayerController player;

	// Use this for initialization
	void Start () {
		this.player = GetComponent<PlayerController> ();
	}

	public void LeaveInn(){
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Town_LVP");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
