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
}
