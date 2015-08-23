using UnityEngine;
using System.Collections;

public class PlayerMoveAnim : MonoBehaviour {
	public Animator parentAnimator;

	public void JumpFwd(){
		if(parentAnimator != null)
			parentAnimator.SetTrigger("MoveForward");
	}

	public void JumpBack(){
		if(parentAnimator != null)
			parentAnimator.SetTrigger("MoveBack");
	}

    public void RunFwd()
    {
        if (parentAnimator != null)
        {
            parentAnimator.SetTrigger("RunFwd");
        }
    }

    public void RunBack()
    {
        if (parentAnimator != null)
        {
            parentAnimator.SetTrigger("RunBack");
        }
    }
}
