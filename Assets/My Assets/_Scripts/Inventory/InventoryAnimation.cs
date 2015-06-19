using UnityEngine;
using System.Collections;

public class InventoryAnimation : MonoBehaviour {
    private Animator _animator;
    private bool open = false;
    void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>();
    }
    public void OpenClose()
    {
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
