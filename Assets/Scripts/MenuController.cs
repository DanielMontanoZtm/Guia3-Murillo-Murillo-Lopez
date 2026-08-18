using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LoadTouchScene()
    {
        SceneManager.LoadScene("TouchScene");
    }

    public void LoadMultitouchScene()
    {
        SceneManager.LoadScene("MultitouchScene");
    }

    public void LoadVirtualPadScene()
    {
        SceneManager.LoadScene("VirtualPadScene");
    }

    public void LoadSensorsScene()
    {
        SceneManager.LoadScene("SensorsScene");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ExitApplication()
    {
        Debug.Log("Saliendo de la aplicación...");
        Application.Quit();
    }
}