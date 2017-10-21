using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallScript : MonoBehaviour {

    private Vector3 Velocity;
    public Vector2 multfactor;
    public Text score;
    private Vector3 G;
    private Rigidbody2D rb;
    public bool isFired = false;
    private int m_basketPhase;
    private int m_score;
    private bool m_scored;

    private Vector3 m_firstPos;

    void Start()
    {
        m_firstPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        m_basketPhase = 0;
        m_score = 0;
        m_scored = false;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            //Reset
            rb.isKinematic = true;
            this.transform.position = m_firstPos;
            Velocity = Vector3.zero;
            isFired = false;
            m_basketPhase = 0;
            m_scored = false;
        }

        if(m_basketPhase == 4)
        {
            if(!m_scored)
            {
                m_scored = true;
                m_score++;
                score.text = m_score.ToString();
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Col1")
        {
            if (m_basketPhase == 0) m_basketPhase = 1;
        }
        else if (other.name == "Col2")
        {
            if (m_basketPhase == 1) m_basketPhase = 2;
        }
        else if (other.name == "Col3")
        {
            if (m_basketPhase == 2) m_basketPhase = 3;
        }
        else if (other.name == "Col4")
        {
            if (m_basketPhase == 3) m_basketPhase = 4;
        }
    }



    public void setForce(Vector3 _force)
    {
        Velocity = new Vector3(_force.x * multfactor.x, _force.y * multfactor.y,0);
    }

    public void shot()
    {
        rb.isKinematic = false;
        rb.AddForce(new Vector2(Velocity.x, Velocity.y));
        isFired = true;
    }

    public Vector3 getVel()
    {
        return Velocity;
    }
}
