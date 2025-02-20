using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BubbleDeviceSelector : MonoBehaviour
{
    public string controlScheme;
    public InputDevice device;

    [SerializeField] Image controls;

    [SerializeField] Sprite wasdIcon;
    [SerializeField] Sprite arrowsIcon;
    [SerializeField] Sprite gamepadIcon;

    void Update()
    {
        if (controlScheme == "WASD") { controls.sprite = wasdIcon; }
        else if (controlScheme == "Arrows") { controls.sprite = arrowsIcon; }
        else { controls.sprite = gamepadIcon; }
    }

    public void RemoveDevice()
    {
        GameManager.Instance.deviceSelectors.Remove(this);
        Destroy(this.gameObject);
    }
}
