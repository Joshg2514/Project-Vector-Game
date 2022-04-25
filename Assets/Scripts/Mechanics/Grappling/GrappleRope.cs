using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
public class GrappleRope : MonoBehaviour
{
    [Header("General Refernces:")]
    public PlayerController player;
	public Transform positiononplay;
    public LineRenderer m_lineRenderer;

    [Header("General Settings:")]
    [SerializeField] private int percision = 40;
    [Range(0, 20)] [SerializeField] private float straightenLineSpeed = 5;

    [Header("Rope Animation Settings:")]
    public AnimationCurve ropeAnimationCurve;
    [Range(0.01f, 4)] [SerializeField] private float StartWaveSize = 2;
    float waveSize = 0;

    [Header("Rope Progression:")]
    public AnimationCurve ropeProgressionCurve;
    [SerializeField] [Range(1, 50)] private float ropeProgressionSpeed = 1;

    float moveTime = 0;

    [HideInInspector] public bool isGrappling = true;

    bool strightLine = true;
	
	public bool Rope = false;
	public bool Waves = false;
	public bool NoWaves = false;
	
	 private void OnEnable()
    {
        moveTime = 0;
        m_lineRenderer.positionCount = percision;
        waveSize = StartWaveSize;
        strightLine = false;

        LinePointsToFirePoint();

        m_lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        m_lineRenderer.enabled = false;
        isGrappling = false;
    }
	
	 private void LinePointsToFirePoint()
    {
        for (int i = 0; i < percision; i++)
        {
            m_lineRenderer.SetPosition(i, positiononplay.position);
        }
    }

	private void Update()
		{
			moveTime += Time.deltaTime;
			DrawRope();
		}
		
	void DrawRope()
    {
		Rope = true;
        if (!strightLine)
        {
            if (m_lineRenderer.GetPosition(percision - 1).x == player.opos.x)
            {
                strightLine = true;
            }
            else
            {
                DrawRopeWaves();
            }
        }
        else
        {
            if (!isGrappling)
            {
                player.Grapple();
                isGrappling = true;
            }
            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                waveSize = 0;

                if (m_lineRenderer.positionCount != 2) { m_lineRenderer.positionCount = 2; }

                DrawRopeNoWaves();
            }
        }
    }
	
	void DrawRopeWaves()
    {
		Waves = true;
        for (int i = 0; i < percision; i++)
        {
            float delta = (float)i / ((float)percision - 1f);
            //Vector2 offset = Vector2.Perpendicular(player.grappleDistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            Vector2 targetPosition = Vector2.Lerp(positiononplay.position, player.opos, delta);// + offset;
            Vector2 currentPosition = Vector2.Lerp(positiononplay.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            m_lineRenderer.SetPosition(i, currentPosition);
        }
    }
	
	void DrawRopeNoWaves()
    {
		NoWaves = true;
        m_lineRenderer.SetPosition(0, positiononplay.position);
        m_lineRenderer.SetPosition(1, player.opos);
    }
	


	
	
	
	
	
	
	
}	
}
