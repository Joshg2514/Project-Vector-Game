using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{

[RequireComponent(typeof(Collider2D))]
	public class ForceZone : MonoBehaviour
	{
		public ZoneType ztype;
		//internal int type;
		internal Vector2 vel;
		internal int push;
		internal SpriteRenderer _renderer;

		void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
			_renderer.color = new Color(1f,1f,1f,.5f);// is about 50% transparent
           // if (randomAnimationStartTime)
           //    frame = Random.Range(0, sprites.Length);
           // sprites = idleAnimation;
		   push = 2;
		   //vel = new Vector2(0, -18.6f);
		   

        }
		
		void OnTriggerEnter2D(Collider2D other)
        {
            //only exectue OnPlayerEnter if the player collides with this token.
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) OnPlayerEnter(player);
			
        }
		void OnTriggerExit2D(Collider2D other)
        {
            //only exectue OnPlayerEnter if the player collides with this token.
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) OnPlayerExit(player);
			
        }
		
		void OnPlayerExit(PlayerController player)
        {
            //if (collected) return;
            //disable the gameObject and remove it from the controller update list.
            //frame = 0;
            //sprites = collectedAnimation;
            //if (controller != null)
            //    collected = DestroyOnPickUp; //changed this
            //send an event into the gameplay system to perform some behaviour.
            var ev = Schedule<PlayerExitsForceZone>();
           /// ev.zone = this;
            ev.player = player;
			
        }
		void OnPlayerEnter(PlayerController player)
        {
            //if (collected) return;
            //disable the gameObject and remove it from the controller update list.
            //frame = 0;
            //sprites = collectedAnimation;
            //if (controller != null)
            //    collected = DestroyOnPickUp; //changed this
            //send an event into the gameplay system to perform some behaviour.
			
			switch (ztype)
            {
				
                case ZoneType.UpZone:
                    var ev = Schedule<PlayerEntersForceZoneUp>();
					ev.player = player;
                    break;
                case ZoneType.DownZone:
					var ev2 = Schedule<PlayerEntersForceZoneDown>();
					ev2.player = player;
                    break;
                case ZoneType.LeftZone:
                    var ev3 = Schedule<PlayerEntersForceZoneLeft>();
					ev3.player = player;
                    break;
                case ZoneType.RightZone:
                    var ev4 = Schedule<PlayerEntersForceZoneRight>();
					ev4.player = player;
                    break;
			}
            //var ev = Schedule<PlayerEntersForceZone>();
           /// ev.zone = this;
            
			
        }
		    public enum ZoneType
        {
            UpZone,
            DownZone,
            LeftZone,
            RightZone
        }
		
		
    }
}

		
	