using UnityEngine;
using System.Collections;
using UnityEngine.iOS;

public class PlayerMoveAnim : MonoBehaviour {
	public Animator parentAnimator;

	public bool isIpad = false;

	void Start(){
		CheckDevice ();
	}

	private void CheckDevice(){
		if(SystemInfo.deviceModel == DeviceGeneration.iPad1Gen.ToString() || SystemInfo.deviceModel == DeviceGeneration.iPad2Gen.ToString() ||
		   SystemInfo.deviceModel == DeviceGeneration.iPad3Gen.ToString() || SystemInfo.deviceModel == DeviceGeneration.iPad4Gen.ToString() ||
		   SystemInfo.deviceModel == DeviceGeneration.iPadAir1.ToString() || SystemInfo.deviceModel == DeviceGeneration.iPadAir2.ToString() ||
		   SystemInfo.deviceModel == DeviceGeneration.iPadMini1Gen.ToString() || SystemInfo.deviceModel == DeviceGeneration.iPadMini2Gen.ToString() ||
		   SystemInfo.deviceModel == DeviceGeneration.iPadMini3Gen.ToString() || SystemInfo.deviceModel == DeviceGeneration.iPadUnknown.ToString()){
			isIpad = true;
		}
	}

	public void JumpFwd(){
		if(isIpad){
			parentAnimator.SetTrigger("iPadFwd");
		}
		else if(parentAnimator != null){
			parentAnimator.SetTrigger("MoveForward");
		}
	}

	public void JumpBack(){
		if(isIpad){
			parentAnimator.SetTrigger("iPadBack");
		}
		else if(parentAnimator != null){
			parentAnimator.SetTrigger("MoveBack");
		}
	}

    public void RunFwd()
    {
		if(isIpad){
			parentAnimator.SetTrigger("iPadRunFwd");
		}
        else if (parentAnimator != null)
        {
            parentAnimator.SetTrigger("RunFwd");
        }
    }

    public void RunBack()
    {
		if(isIpad){
			parentAnimator.SetTrigger("iPadRunBack");
		}
        else if (parentAnimator != null)
        {
            parentAnimator.SetTrigger("RunBack");
        }
    }
}
