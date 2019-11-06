using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public string m_materialColorField;
    
    Renderer m_renderer;
    MaterialPropertyBlock m_mpb;


    public bool InRange
    {
        get { return m_inRange; }
        set
        {
            m_inRange = value;
            RefreshCrosshair();
        }
    }
    bool m_inRange;

    public bool Targeted
    {
        get { return m_targeted; }
        set
        {
            m_targeted = value;
            RefreshCrosshair();
        }
    }
    bool m_targeted;

    void Start()
    {
        m_renderer = gameObject.GetComponent<Renderer>();
        m_mpb = new MaterialPropertyBlock();
        RefreshCrosshair();
        m_mpb.SetColor(m_materialColorField, Color.white);
        m_renderer.SetPropertyBlock(m_mpb);
    }

    void RefreshCrosshair()
    {
        if (m_materialColorField != null)
        {
            m_renderer.GetPropertyBlock(m_mpb);
            if (!InRange) m_mpb.SetColor(m_materialColorField, Color.white);
            else if (Targeted) m_mpb.SetColor(m_materialColorField, Color.blue);
            else m_mpb.SetColor(m_materialColorField, Color.blue);
            m_renderer.SetPropertyBlock(m_mpb);
        }
    }

    public void SetColor(Color focusColor)
    {
        m_mpb.SetColor(m_materialColorField, focusColor);
        m_renderer.SetPropertyBlock(m_mpb);
    }

    public void ClearColor()
    {
        m_mpb.SetColor(m_materialColorField, Color.white);
        m_renderer.SetPropertyBlock(m_mpb);
    }
}
