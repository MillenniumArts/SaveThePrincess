using UnityEngine;
using System.Collections;

public class InventoryAnimation : MonoBehaviour {
    private Animator _animator;
    public bool open = false;
    void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>();
    }
    public void OpenClose()
    {
        AudioManager.Instance.PlaySFX("Button2");
        if (!open)
        {
            _animator.SetTrigger("Open");
            open = true;
        }
        else
        {
            _animator.SetTrigger("Close");
            open = false;
        }
    }
}
