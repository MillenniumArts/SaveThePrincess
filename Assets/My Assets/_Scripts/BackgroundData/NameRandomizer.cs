using UnityEngine;
using System.Collections;

public class NameRandomizer : MonoBehaviour{

	#region Singleton
	private static NameRandomizer _instance;
	
	public static NameRandomizer instance{
		get{
			if(_instance == null){
				_instance = GameObject.FindObjectOfType<NameRandomizer>();
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	
	void Awake(){
		if(_instance == null){
			_instance = this;
			DontDestroyOnLoad(this);
		}else{
			if(this != _instance){
				Destroy(this.gameObject);
			}
		}
	}
	#endregion Singleton

	public string[] part1;
	public string[] part2;

	public string GetPart1(){
		int i = Random.Range(0, part1.Length);
		return part1[i];
	}

	public string GetPart2(){
		int i = Random.Range(0, part2.Length);
		return part2[i];
	}

}
