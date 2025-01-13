using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    public GameObject pausePanel;
    PlayerControls controls;
    // Start is called before the first frame update
    void Awake()
    {
        pausePanel.SetActive(false);
        controls = new PlayerControls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable()
    {
        controls.Enable();
        controls.UI.Pause.performed += ctx => OnPause(ctx);

    }

    void OnDisable()
    {
        controls.Disable();

    }
    public void OnPause(InputAction.CallbackContext context)
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        Time.timeScale = pausePanel.activeSelf ? 0f : 1f;
    }
}
