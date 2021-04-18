using UnityEngine;
using UnityEngine.SceneManagement;

public class UiGameOverController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Reload current scene
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex);
        }
    }
}
