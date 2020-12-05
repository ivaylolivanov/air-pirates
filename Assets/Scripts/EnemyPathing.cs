using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;

    List<Transform> waypoints;
    int currentWaypointIndex = 0;

    void Start() {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[currentWaypointIndex].transform.position;
    }

    void Update() {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig) {
        this.waveConfig = waveConfig;
    }

    void Move() {
        if(currentWaypointIndex <= (waypoints.Count - 1)){
            var targetPosition = waypoints[currentWaypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                movementThisFrame
            );

            if(transform.position == targetPosition) {
                ++currentWaypointIndex;
            }
        }
        else {
            Destroy(gameObject);
        }
    }
}
