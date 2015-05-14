using UnityEngine;
using System.Collections;

public class ItemTest : MonoBehaviour {
	public Item _item;
	public SpriteRenderer _renderer;
	// Update is called once per frame
	void Update () {
		if(_item.GetSprite() != null){
			_renderer.sprite = _item.GetSprite();
		}
	}
}
