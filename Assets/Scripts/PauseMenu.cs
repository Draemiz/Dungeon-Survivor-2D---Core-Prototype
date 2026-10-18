using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panelPausa;

    private bool isPaused;

    void Start()
    {
        if (panelPausa == null)
        {
            Debug.LogError(
                "Asigna PanelPausa en el Inspector.",
                this
            );

            enabled = false;
            return;
        }

        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
    }

    void OnDisable()
    {
        // Evita dejar el juego congelado al salir de la escena.
        Time.timeScale = 1f;
    }
}