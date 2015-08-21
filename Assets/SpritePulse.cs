using UnityEngine;
using System.Collections;

public class SpritePulse : MonoBehaviour {

    public GameObject spriteParent;
    public float speed = 1.0f;
    public float time = 1.0f;
    public float minAlpha = 0.6f;
    public float maxAlpha = 1.0f;
    public bool isOn = false;

	// Update is called once per frame
	void Update () {
        if (isOn)
        {
            ChangeSpriteColour();
        }
        else
        {
            ChangeSpriteColour(1.0f);
        }
	}

    public void PulseOn()
    {
        isOn = true;
    }

    public void PulseOff()
    {
        isOn = false;
    }

    private void ChangeSpriteColour()
    {
        Color newColour = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha));
        foreach (SpriteRenderer renderer in spriteParent.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = newColour;
        }
    }

    private void ChangeSpriteColour(float alpha)
    {
        Color newColour = new Color(1.0f, 1.0f, 1.0f, alpha);
        foreach (SpriteRenderer renderer in spriteParent.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = newColour;
        }
    }
}
