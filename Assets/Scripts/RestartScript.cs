using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Attempting to reset level.");
    }
}
