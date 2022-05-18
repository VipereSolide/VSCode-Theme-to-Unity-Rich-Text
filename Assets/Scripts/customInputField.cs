using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

public class customInputField : TMP_InputField
{
    [SerializeField] private string m_selectedText = "";

    void Update()
    {
        if (Application.isPlaying)
        {
            UpdateSelectedText();
        }
    }

    void UpdateSelectedText()
    {
        if (m_CaretSelectPosition < m_CaretPosition)
        {
            m_selectedText = text.Substring(m_CaretSelectPosition, m_CaretPosition - m_CaretSelectPosition);
        }
        else
        {
            m_selectedText = text.Substring(m_CaretPosition, m_CaretSelectPosition - m_CaretPosition);
        }
    }

    private void SetRichText(string _RichTextTag, bool _CustomEndTag = false, string _EndTag = "")
    {
        string _afterText = "";
        string _beforeText = "";
        string _replacedText = text;

        if (m_CaretSelectPosition < m_CaretPosition)
        {
            _afterText = text.Substring(m_CaretPosition, text.Length - m_CaretPosition);
        }
        else
        {
            _afterText = text.Substring(m_CaretSelectPosition, text.Length - m_CaretSelectPosition);
        }

        if (m_CaretSelectPosition < m_CaretPosition)
        {
            _beforeText = text.Substring(0, m_CaretSelectPosition);
        }
        else
        {
            _beforeText = text.Substring(0, m_CaretPosition);
        }

        _replacedText = _replacedText.Replace(_beforeText, _beforeText + "<" + _RichTextTag + ">");
        _replacedText = _replacedText.Replace(_afterText, "</" + ( (_CustomEndTag) ? _EndTag : _RichTextTag ) + ">" + _afterText);

        text = _replacedText;
    }

    public void BoldSelection()
    {
        SetRichText("b");
    }

    public void ItalicSelection()
    {
        SetRichText("i");
    }

    public void UnderlineSelection()
    {
        SetRichText("u");        
    }

    public void ColorSelection(Color32 _Color)
    {
        int _red = (int)_Color.r;
        int _green = (int)_Color.g;
        int _blue = (int)_Color.b;
        int _alpha = (int)_Color.a;

        string _result = _red.ToString("X2") + _green.ToString("X2") + _blue.ToString("X2") + _alpha.ToString("X2");
        SetRichText("#" + _result, true, "color");
    }
}
