using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextScene : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private CameraFade cameraFade;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            cameraFade.StartFadeOutAndIn(() => 
            {
                LoadScene();
            });
        }
    }

    private void LoadScene() {
        SceneManager.LoadScene(nextSceneName);
    }
}
