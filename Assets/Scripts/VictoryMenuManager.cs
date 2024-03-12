using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenuManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera mainCam;
    public void OnPlayAgainPressed()
    {
        StartCoroutine(SceneTransition());
    }

    private IEnumerator SceneTransition()
    {
        while (mainCam.transform.position.y > 0)
        {
            mainCam.transform.position += new Vector3(0, -0.2f, 0);
            foreach (Transform child in canvas.GetComponentsInChildren<Transform>())
            {
                if (child.parent == canvas.transform)
                {
                    child.position += new Vector3(0, 10f, 0);
                }

            }
            yield return null;
        }
        mainCam.transform.position = new Vector3(0, 0, -10);
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("GameScene");
    }
}
