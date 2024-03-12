using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static float moveSpeed = 0.02f;
    public static Vector3 btmlftBorder;
    private Vector3 btmlftCalc;
    public static Vector3 topritBorder;
    private Vector3 topritCalc;
    private Camera ownCam;
    /*public static CameraController instance;*/

    [SerializeField] private Vector3 winCutscenePosition;
    // Start is called before the first frame update
    void Start()
    {
        /*instance = this;*/
        moveSpeed = 0.02f;
        btmlftBorder = new Vector3();
        topritBorder = new Vector3();
        ownCam = GetComponent<Camera>();
        btmlftCalc = new Vector3(-ownCam.orthographicSize * ownCam.aspect, -ownCam.orthographicSize);
        topritCalc = new Vector3(ownCam.orthographicSize * ownCam.aspect, ownCam.orthographicSize);
        btmlftBorder = btmlftCalc + transform.position;
        topritBorder = topritCalc + transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(1f, 0, 0) * moveSpeed * Time.deltaTime * 40;
        btmlftBorder = btmlftCalc + transform.position;
        topritBorder =  topritCalc + transform.position;
    }

    //Will be called by invoking a unity event, which happens when player reaches space station and wins
    public void PlayerWonResponse()
    {
        moveSpeed = 0;
    }

    //Moves camera to a certain point at a set movement speed
    private IEnumerator MoveToScreenPoint()
    {
        while (transform.position.x < winCutscenePosition.x)
        {
            yield return null;
        }
        moveSpeed = 0;
    }
}
