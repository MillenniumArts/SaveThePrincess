using UnityEngine;
using System.Collections;

public class ItemRenderer : MonoBehaviour
{
    public Item _item;
    public SpriteRenderer _renderer;

    void Start()
    {
        StartCoroutine("LateStart");
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.01f);
        if (_item.GetSprite() != null)
        {
            _renderer.sprite = _item.GetSprite();
        }
    }
}
