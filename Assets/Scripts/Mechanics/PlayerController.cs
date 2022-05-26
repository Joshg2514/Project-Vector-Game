using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
	/// version 0.01.3
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
		public GrappleRope rope1;
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
		public Vector2 direction1;
		public Vector2 grappleDistanceVector;
		public LineRenderer m_lineRenderer;

		
        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;
		 
        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;
		Transform ttransform;
        bool jump;
        public Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;
		
		private Rigidbody2D rb;
		
		internal bool inZone = false;
		internal bool grappling = false;
		
		public float teledis = .5f;
		public bool blink;
		public Vector3 opos;

        void Awake()
        {
			blink = true;
			rope1.enabled = false;
			defGravity = new Vector2(0, -9.8f);
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
			ttransform = GetComponent<Transform>();
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
			if(jumpState != JumpState.Frozen && jumpState != JumpState.Grappling){
			if (inZone){
					
					if(Input.GetButtonDown("Fire1")) {
						var ev = Schedule<PlayerStartsGrapple>();
						ev.player = this;
						ev.opos = opos;
						//grappling = true;
						//jumpState = JumpState.Grappling;
						direction1 = GravDir * convertAbsVectorToRelativeVector((opos - transform.position).normalized);
					
					}
						
				}
			//if (jumpState != JumpState.Grappling){
            if (controlEnabled)
            {
				//move.x = 
				move.y = Input.GetAxis("Vertical");
                move.x = Input.GetAxis("Horizontal") * controlswap; //move.x should be relative move.x
                animator.SetFloat("Speed", Mathf.Abs(move.x));
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump")) 
                {
                    jumpState = JumpState.PrepareToJump;
                }
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
				
				if(Input.GetButtonDown("Fire2") && blink)
				{
					move.x = Input.GetAxis("Horizontal");
					Vector3 mve = move.normalized;
					Blink(mve);
				}
				
				
            }
            else
            {
                move.x = 0;
            }
			//}
			}
            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
					affectedGrav = true;
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
					affectedGrav = true;
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
					affectedGrav = true;
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
					affectedGrav = true;
                    jumpState = JumpState.Grounded;
					blink = true;
                    break;
				case JumpState.Grappling:
					affectedGrav = false;
					//grappling = true;
					break;
				case JumpState.Frozen:
					affectedGrav = false;
					//grappling = true;
					break;
				
            }
        }

        protected override void ComputeVelocity()
        {
			if(jumpState != JumpState.Frozen){
			if(jumpState != JumpState.Grappling){
				
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

          // animator.SetBool("grounded", IsGrounded);
          // animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
			}
			else{
				jump = true;
				if(grappling){
					
					velocity.x = 0;
					velocity.y = 0;
					grappling = false;
				}
					
				Vector2 direction = GravDir * convertAbsVectorToRelativeVector((opos - transform.position).normalized);
				
				if(Vector3.Dot(velocity, direction) < 0){
					rope1.enabled = false;
					jumpState = JumpState.InFlight;
					velocity = (10f * direction1);
					
				}
				//velocity += (30f * direction) * Time.deltaTime * 1.5f;
				else if(velocity.magnitude < (10f * direction.magnitude))
					velocity += (5f * direction); //* Time.deltaTime * 1.5f;
				//velocity = convertAbsVectorToRelativeVector(Vector2 velocity);
				//transform.position += velocity * Time.deltaTime;
				
			}
			}
			else{
				velocity.x = 0;
				velocity.y = 0;
			}
        }
		
		public void Blink(Vector3 blinkdir)
		{
			//Vector3
			//if (Physics.Raycast(transform.position, blinkdir, teledis))
			
			RaycastHit2D hit = Physics2D.Raycast(transform.position, blinkdir, teledis, 3);
			
			//if (Physics.Raycast(transform.position, transform.TransformDirection(blinkdir), out hit, teledis, 3))
			//{
			//	Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
			//	Teleport(((blinkdir * teledis) + transform.position));
			//	Debug.Log("Did Hit");
			//}
			//else
			//{
				Debug.DrawRay(transform.position, blinkdir * teledis, Color.white, .1f);
			//	Debug.Log("Did not Hit");
			//}
			

			if (hit.collider != null) {
				Teleport(((blinkdir * (hit.distance - .5f) + transform.position)));
				Debug.Log("Hitting: " + hit.collider.tag);
			}
			else
				Teleport(((blinkdir * (teledis) + transform.position)));
				
				jumpState = JumpState.InFlight;
				blink = false;
			//	move.x = 0;
            //else
			//	Teleport(((blinkdir * teledis) + transform.position));
		}
		
		public void changeGravyState(int a)
        {
			transform.eulerAngles = new Vector3 (0, 0, 0);
			if(a == 1)
				transform.Rotate(new Vector3(0, 0, 180));
			if(a == 2)
				transform.Rotate(new Vector3(0, 0, 0));
			if(a == 3)
				transform.Rotate(new Vector3(0, 0, 270));
			if(a == 4)
				transform.Rotate(new Vector3(0, 0, 90));
			changeGravState(a);
        }
		public void addForce(Vector2 dir, int i)
        {
			push(dir, i);
        }
		public void stopForce(Vector2 dir)
        {
			stoppush(dir);
        }
		public void startGrapple(Vector3 grappleposition)
		{
			grappling = true;
			jumpState = JumpState.Frozen;
			rope1.enabled = true;
		}
		public void Grapple()
		{
			jumpState = JumpState.Grappling;
		}
        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed,
			Grappling,
			Frozen
        }
		
		public void InZone(bool stat, Vector3 pos){
			inZone = stat;
			opos = pos;
			grappleDistanceVector = pos - transform.position;
			
		}
    }
}