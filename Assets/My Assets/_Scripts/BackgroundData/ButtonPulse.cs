using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonPulse : MonoBehaviour {
    public float speed = 1.0f;
    public float time = 1.0f;
    public float minAlpha = 0.6f;
    public float maxAlpha = 1.0f;
    public bool isOn = false;
    public Button _button;
    private ColorBlock _colour;
    public string colourName;

    void Start()
    {
        _colour = _button.colors;
    }

	// Update is called once per frame
	void Update () {
        if (isOn)
        {
            Pulse();
        }
        else
        {
            _colour.normalColor = new Color(1, 1, 1, 1);
            _button.colors = _colour;
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

    private void Pulse()
    {
        colourName.ToLower();
        switch (colourName)
        {
            case "red":
                _colour.normalColor = new Color(_colour.normalColor.r, Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), _colour.normalColor.a);
                _button.colors = _colour;
                break;
            case "green":
                _colour.normalColor = new Color(Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), _colour.normalColor.g, Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), _colour.normalColor.a);
                _button.colors = _colour;
                break;
            case "blue":
                _colour.normalColor = new Color(Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), _colour.normalColor.b, _colour.normalColor.a);
                _button.colors = _colour;
                break;
            case "alpha":
                _colour.normalColor = new Color(_colour.normalColor.r, _colour.normalColor.g, _colour.normalColor.b, Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha));
                _button.colors = _colour;
                break;
            default:
                _colour.normalColor = new Color(_colour.normalColor.r, _colour.normalColor.g, _colour.normalColor.b, Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha));
                _button.colors = _colour;
                break;
        }
    }
}
