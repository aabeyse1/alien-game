using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextScene : MonoBehaviour
{
    [SerializeField] string nextSceneName;
   private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.CompareTag("Player")) {
             SceneManager.LoadScene(nextSceneName);
        }
   }
}
