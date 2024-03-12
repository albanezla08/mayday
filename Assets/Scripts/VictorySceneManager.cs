using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictorySceneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyShipsToActivate;
    [SerializeField] private GameObject[] friendlyShipsToActivate;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private Vector3 firstPlayerPosition;
    [SerializeField] private Vector3 secondPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerWon()
    {
        StartCoroutine(moveCameraToPoint());
    }

    private IEnumerator moveCameraToPoint()
    {
        while (mainCam.transform.position.x < 244)
        {
            mainCam.transform.position += new Vector3(1f, 0, 0) * 0.02f * Time.deltaTime * 40;
            playerShip.transform.position += new Vector3(1f, 0, 0) * 0.02f * Time.deltaTime * 40;
            yield return null;
        }
        StartCoroutine(movePlayerToFirstPos());
    }

    private IEnumerator movePlayerToFirstPos()
    {
        while (playerShip.transform.position != firstPlayerPosition)
        {
            playerShip.transform.position = Vector3.MoveTowards(playerShip.transform.position, firstPlayerPosition, 0.02f * Time.deltaTime * 40);
            yield return null;
        }
        StartCoroutine(moveEnemyShips());
    }

    private IEnumerator moveEnemyShips()
    {
        yield return new WaitForSeconds(2f);
        foreach (GameObject enemyShip in enemyShipsToActivate)
        {
            enemyShip.SetActive(true);
            yield return null;
            enemyShip.GetComponent<LeftGunnerController>().VictoryCutscene();
        }
        StartCoroutine(moveFriendlyShips());
    }

    private IEnumerator moveFriendlyShips()
    {
        yield return new WaitForSeconds(2f); //wait for enemy ships to stare a bit
        foreach (GameObject friendlyShip in friendlyShipsToActivate)
        {
            friendlyShip.SetActive(true);
            yield return null;
            friendlyShip.GetComponent<GunnerController>().VictoryCutscene();
        }
        StartCoroutine(movePlayerIntoStation());
    }

    private IEnumerator movePlayerIntoStation()
    {
        yield return new WaitForSeconds(1f);
        while (playerShip.transform.position != secondPlayerPosition)
        {
            playerShip.transform.position = Vector3.MoveTowards(playerShip.transform.position, secondPlayerPosition, 0.015f * Time.deltaTime * 40);
            yield return null;
        }
        playerShip.SetActive(false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("VictoryScreen");
    }
}
