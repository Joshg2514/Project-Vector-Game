using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
	/// version 0.01.3
    /// <summary>
    /// Implements game physics for some in game entity.
    /// </summary>
    public class KinematicObject : MonoBehaviour
    {
        /// <summary>
        /// The minimum normal (dot product) considered suitable for the entity sit on.
        /// </summary>
        public float minGroundNormalY = .65f;
		public int GravDir = 1;
        /// <summary>
        /// A custom gravity coefficient applied to this entity.
        /// </summary>
        public float gravityModifier = 1f;
		public Vector2 defGravity;
		public Vector2 fGravity = new Vector2(0,0);
		//public Vector2 addvelocity = new Vector2(0, 0);
		public float maxFallVelocity = 20f;
        /// <summary>
        /// The current velocity of the entity.
        /// </summary>
        public Vector2 velocity;
		
        /// <summary>
        /// Is the entity currently sitting on a surface?
        /// </summary>
        /// <value></value>
        public bool IsGrounded { get; private set; }
		
		public GravState gravState;
		//public GravState gravSstate;
		
		
		internal int controlswap = 1;
        protected Vector2 targetVelocity;
        protected Vector2 groundNormal;
        protected Rigidbody2D body;
        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

        protected const float minMoveDistance = 0.001f;
        protected const float shellRadius = 0.01f;
		
		public bool affectedGrav = true;
		public int gravint;
		internal int pushint;
		public int gravtemp;
		internal float maxSeed = 6f;
        /// <summary>
        /// Bounce the object's vertical velocity.
        /// </summary>
        /// <param name="value"></param>
        public void Bounce(float value)
        {
            velocity.y = value;
        }

        /// <summary>
        /// Bounce the objects velocity in a direction.
        /// </summary>
        /// <param name="dir"></param>
        public void Bounce(Vector2 dir)
        {
            velocity.y = dir.y;
            velocity.x = dir.x;
        }
		public void push(Vector2 dir, int i)
        {
			var vir = convertAbsVectorToRelativeVector(dir);
            defGravity.y = 0;
            defGravity.x = 0;
			fGravity.y = i*vir.y;
            fGravity.x = vir.x;
			maxFallVelocity = 10f;
			pushint = i;
			//gravtemp = GravDir;
			if(gravState == GravState.Right ||gravState == GravState.Right)
				fGravity.x = -fGravity.x;
			//if(i == 2 || i == 4)
			//	GravDir = 1;
			affectedGrav = true;
			
        }
		public void stoppush(Vector2 dir)
        {
			dir = convertAbsVectorToRelativeVector(dir);
			fGravity.y = 0;
            fGravity.x = 0;
            defGravity.y = -9.8f;
            defGravity.x = 0;
			maxFallVelocity = 20f;
			
			//GravDir = gravtemp;
			
			affectedGrav = false;
        }

        /// <summary>
        /// Teleport to some position.
        /// </summary>
        /// <param name="position"></param>
        public void Teleport(Vector3 position)
        {
            body.position = position;
            //velocity *= 0;
            //body.velocity *= 0;
        }

        protected virtual void OnEnable()
        {
			
            body = GetComponent<Rigidbody2D>();
            body.isKinematic = true;
			//body.transform.localEulerAngles = new Vector3(0,0,90);
			//body.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
        }

        protected virtual void OnDisable()
        {
            body.isKinematic = false;
        }

        protected virtual void Start()
        {
			
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            contactFilter.useLayerMask = true;
        }

        protected virtual void Update()
        {
            targetVelocity = Vector2.zero;
            ComputeVelocity();
        }

        protected virtual void ComputeVelocity()
        {

        }
		
		protected virtual void AddForce()
		{
			
		}

        protected virtual void FixedUpdate()
        {
            //if already falling, fall faster than the jump speed, otherwise use normal gravity.
			if(affectedGrav){
			if(maxFallVelocity > -velocity.y )
			{
            if (-velocity.y > 0 )
                velocity += gravityModifier * (defGravity + fGravity) * Time.deltaTime ;
            else
                velocity += (defGravity + fGravity) * Time.deltaTime ;
			}
			}
			else if(maxFallVelocity > -rotateVector(velocity, pushint%4).y)
			{
			//if (-rotateVector(velocity, pushint).x > 0 )
           // velocity += gravityModifier * defGravity * Time.deltaTime ;
           // else
            velocity += gravityModifier * (defGravity + fGravity) * Time.deltaTime ;
			}
			if(IsGrounded)
				velocity.x = targetVelocity.x;
			else if(Mathf.Abs(velocity.x + targetVelocity.x) < Mathf.Abs(velocity.x))
				velocity.x = targetVelocity.x*1f;
			else if(Mathf.Abs(velocity.x + targetVelocity.x) < maxSeed && affectedGrav)
				velocity.x += targetVelocity.x*1f;
			
				
            IsGrounded = false;

            var deltaPosition = (velocity) * Time.deltaTime;
			var convertedDeltaPosition = convertAbsVectorToRelativeVector(deltaPosition);

            var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

            var move = moveAlongGround * convertedDeltaPosition.x;

            PerformMovement(move, false);

			move = Vector2.up * convertedDeltaPosition.y;
			
            PerformMovement(move, true);

        }

        void PerformMovement(Vector2 move, bool yMovement)
        {
            var distance = move.magnitude;

            if (distance > minMoveDistance)
            {
                //check if we hit anything in current direction of travel
                var count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
                for (var i = 0; i < count; i++)
                {
                    var currentNormal = hitBuffer[i].normal;
					currentNormal = convertAbsVectorToRelativeVector(currentNormal);
                    //is this surface flat enough to land on?
                    if ( GravDir * currentNormal.y > minGroundNormalY)
                    {
                        IsGrounded = true;
                        // if moving up, change the groundNormal to new surface normal.
                        if (yMovement)
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    if (IsGrounded)
                    {
                        //how much of our velocity aligns with surface normal?
                        var projection = Vector2.Dot(velocity, currentNormal*GravDir);
                        if (projection < 0)
                        {
                            //slower velocity if moving against the normal (up a hill).
                            velocity = velocity - projection * currentNormal*GravDir;
                        }
                    }
                    else
                    {
                        //We are airborne, but hit something, so cancel vertical up and horizontal velocity.
                        velocity.x *= 0;
						if ( -GravDir * currentNormal.y > minGroundNormalY)
							velocity.y = Mathf.Min(GravDir*velocity.y, 0);
                    }
                    //remove shellDistance from actual move distance.
                    var modifiedDistance = hitBuffer[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            body.position = body.position + move.normalized * distance;
		
        }
		
		public Vector2 convertAbsVectorToRelativeVector(Vector2 relVec)
		{
			switch (gravState)
            {
                case GravState.Up:
					gravint = 2;
					//return rotateVector(relVec, 2);
					break;
                    
                    
                case GravState.Down:
					gravint = 0;
					//return relVec;
					break;
                    
                case GravState.Right:
					gravint = 3;
                   //return rotateVector(relVec, 3);
				   break;
                    
                case GravState.Left:
					gravint = 1;
                    //return rotateVector(relVec, 1);
					break;
					
                
			}
			return rotateVector(relVec, gravint);
		}
		
		public Vector2 rotateVector(Vector2 vec, int times)
		{
			var x = 0f;
			var y = 0f;
			
			for (var i = 1; i <= times; i++)
                {
					x = vec.x;
					y = vec.y;
					vec.x = y;
					vec.y = -x;
				}
			return vec;
		}
		
		public virtual void changeGravState(int a)
        {
             switch (a)
            {
                case 1:
					controlswap = -1;
                    gravState = GravState.Up;
					GravDir = 1;
                    break;
                case 2:
					controlswap = 1;
					gravState = GravState.Down;
					GravDir = 1;
                    break;
                case 3:
					controlswap = 1;
                    gravState = GravState.Left;
					GravDir = -1;
                    break;
                case 4:
					controlswap = 1;
                    gravState = GravState.Right;
					GravDir = -1;
                    break;
            } 
        }
	
	public enum GravState
		{
			Up,
			Down,
			Left,
			Right
		}
	
    }
}