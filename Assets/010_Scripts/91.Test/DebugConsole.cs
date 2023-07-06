using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugConsole : MonoBehaviour
{
    public GameObject consolePanel;
    public Transform consoleTextArea;
    public GameObject _textPrefab;

    #region Singleton
    private static DebugConsole _instance;
    private void Awake() {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static DebugConsole GetInstance()
    {
        if(_instance == null)
        {
            _instance = new DebugConsole();
        }
        return _instance;
    }
    #endregion

    private bool isConsoleVisible = false;

    private string initialActionMap;

    private void Update()
    {
        if (InputManager.GetInstance().OpenConsole)
        {
            if(!isConsoleVisible) initialActionMap = InputManager.GetInstance().GetCurrentActionMap();
            isConsoleVisible = !isConsoleVisible;
            CheckActionMap();
            consolePanel.SetActive(isConsoleVisible);
        }

        // if(InputManager.GetInstance().LeftClick)
        // {
        //     LogMessage("Left Click Pressed at " + Time.time.ToString());
        // }

        if(InputManager.GetInstance().Submit)
        {
            LogMessage("Submit Pressed at " + Time.time.ToString());
        }

    }

    public void LogMessage(string message)
    {
        //consoleText.text += message + "\n";
        Instantiate(_textPrefab, consoleTextArea).GetComponent<TextMeshProUGUI>().SetText(message);
    }

    public void CheckActionMap()
    {
        if(isConsoleVisible && InputManager.GetInstance().GetCurrentActionMap() != "UI")
        {
            InputManager.GetInstance().SwitchToUI();
        }
        else if(!isConsoleVisible && InputManager.GetInstance().GetCurrentActionMap() != initialActionMap)
        {
            InputManager.GetInstance().SwitchToGameplay();
        }
    }
}
