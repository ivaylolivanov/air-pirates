using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float loadDelay = 5f;

    public void LoadStartMenuScene() {
        SceneManager.LoadScene(0);
        Cursor.visible = true;
        FindObjectOfType<GameSession>().Reset();
    }

    public void LoadGame() {
        SceneManager.LoadScene("Game");
        Cursor.visible = false;
    }

    public void LoadGameOver() {
        Cursor.visible = true;
        StartCoroutine(DelayLoad());
    }

    IEnumerator DelayLoad() {
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene("GameOver");
    }

    public void Quit() {
        Application.Quit();
    }
}
