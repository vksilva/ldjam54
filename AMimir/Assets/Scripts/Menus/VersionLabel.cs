using TMPro;
using UnityEngine;

public class VersionLabel : MonoBehaviour
{
    void Start()
    {
        var label = gameObject.GetComponent<TMP_Text>();
        label.text = Application.version;
    }
}
