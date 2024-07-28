using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChoiceView : MonoBehaviour
{
    [SerializeField] private Button m_Button;
    [SerializeField] private TMP_Text m_Text;

    public void SetText(string text)
    {
        m_Text.text = text; 
    }

    public void BindSelectedEvent(UnityAction<int> action, int parameter)
    {
        m_Button.onClick.AddListener(() => action(parameter));
    }

}
