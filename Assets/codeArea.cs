using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

public class codeArea : MonoBehaviour
{
    [SerializeField] private TMP_Text m_LineCountText;
    [SerializeField] private customInputField m_CodeArea;

    private int m_CurrentLineCount = 1;

    private void Start()
    {
        CountLines();
    }

    void Update()
    {
    }

    public void CountLines()
    {
        string[] _lines = m_CodeArea.text.Split('\n');

        m_CurrentLineCount = _lines.Length + 1;
        UpdateLineCount();
    }

    private void UpdateLineCount()
    {
        string _finalText = "";

        for (int i = 1; i < m_CurrentLineCount; i++)
        {
            _finalText += i + "\n";
        }

        m_LineCountText.text = _finalText;
    }

    void SetCaretPosition(TMP_InputField inputField, int caretPos) {
        string tempText = inputField.text;
        inputField.text = inputField.text.Substring(0, caretPos);
        inputField.MoveTextEnd(false);
        inputField.text = tempText;
    }
}
