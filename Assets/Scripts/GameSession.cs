using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score = 0;

    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton() {
        int sessionsCount = FindObjectsOfType<GameSession>().Length;
        if(sessionsCount > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore() {
        return score;
    }

    public void AddPoints(int points) {
        score += points;
    }

    public void Reset() {
        Destroy(gameObject);
    }
}
