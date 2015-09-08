using UnityEngine;
using System.Collections;

public class SpritePulse : MonoBehaviour {

    public GameObject spriteParent;
    public float speed = 1.0f;
    public float time = 1.0f;
    public float minAlpha = 0.6f;
    public float maxAlpha = 1.0f;
    public bool isOn = false;
    public string colour;
    private Color newColour;

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
        colour.ToLower();
        switch (colour)
        {
            case "alpha":
                newColour = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha));
                break;
            case "black":
                newColour = new Color(Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), 
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha),
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), 
                                    1.0f
                                    );
                break;
            default:
                    Debug.Log("Choose a colour or spell the colour correctly.");
                break;
        }
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
