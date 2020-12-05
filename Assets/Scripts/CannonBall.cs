using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
     void OnBecameInvisible() {
         Destroy(gameObject);
     }
}
