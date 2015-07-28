using UnityEngine;
using System.Collections;

public class PlayerPositionController : MonoBehaviour {
    private float width, height;
    public void MovePlayer(int percentFormBottom, int percentFromLeft)
    {
        float heightPercent = percentFormBottom * 0.01f;
        float widthPercent = percentFromLeft * 0.01f;

        height = 1 / heightPercent;
        width = 1 / widthPercent;

        //Debug.Log(Screen.width);
        ///Debug.Log(Screen.height);

        float positionX = Screen.width / width;
        float positionY = Screen.height / height;

        //Debug.Log(positionX);
        //Debug.Log(positionY);

        Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(positionX, positionY, 0));
        newPos.z = 0;
        this.gameObject.transform.localPosition = newPos;
    }
}
