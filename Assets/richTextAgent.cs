using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class richTextAgent : MonoBehaviour
{
    public enum RichTextAction { Bold, Italic, Underline, Color }

    [SerializeField] private RichTextAction m_Action;
}
