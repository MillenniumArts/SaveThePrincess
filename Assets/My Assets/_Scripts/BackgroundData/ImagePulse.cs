using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImagePulse : MonoBehaviour {
    public float speed = 1.0f;
    public float time = 1.0f;
    public float minAlpha = 0.6f;
    public float maxAlpha = 1.0f;
    public bool isOn = false;
    public Image _image;
    private Color _colour;
    public string colourName;
    public Vector4 colourNum = new Vector4(1, 1, 1, 1);

    void Start()
    {
        _colour = _image.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            Pulse();
        }
        else
        {
            _colour = colourNum;//new Color(1, 1, 1, 1);
            _image.color = _colour;
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
                _colour = new Color(_colour.r, 
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), 
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), 
                                    _colour.a
                                    );
                _image.color = _colour;
                break;
            case "green":
                _colour = new Color(
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha),
                                    _colour.g, 
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), 
                                    _colour.a
                                    );
                _image.color = _colour;
                break;
            case "blue":
                _colour = new Color(
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), 
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), 
                                    _colour.b, 
                                    _colour.a
                                    );
                _image.color = _colour;
                break;
            case "alpha":
                _colour = new Color(
                                    _colour.r, 
                                    _colour.g, 
                                    _colour.b, 
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha)
                                    );
                _image.color = _colour;
                break;
            case "black":
                _colour = new Color(
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), 
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha),
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha), 
                                    _colour.a
                                    );
                _image.color = _colour;
                break;
            default:
                _colour = new Color(
                                    _colour.r,
                                    _colour.g, 
                                    _colour.b, 
                                    Mathf.Clamp(Mathf.PingPong((Time.time * speed), time), minAlpha, maxAlpha)
                                    );
                _image.color = _colour;
                break;
        }
    }
}
