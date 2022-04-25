
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{


[RequireComponent(typeof(Collider2D))]
	public class InteractableInstance : MonoBehaviour
	{
		//public ZoneType ztype;
		//internal int type;
		//internal Vector2 vel;
		//internal int push;
		internal SpriteRenderer _renderer;
		public bool inside = false;
		void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
			_renderer.color = new Color(200f,1f,1f,255f);// is about 50% transparent
			
           // if (randomAnimationStartTime)
           //    frame = Random.Range(0, sprites.Length);
           // sprites = idleAnimation;
		   ///push = 2;
		   //vel = new Vector2(0, -18.6f);
		   

        }
		
		void OnTriggerEnter2D(Collider2D other)
        {
            //only exectue OnPlayerEnter if the player collides with this token.
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null){
				OnPlayerEnter(player);// is about 50% transparent
				if (Input.GetButtonUp("Fire1"))
					_renderer.color = new Color(1f,200f,1f,.5f);// is about 50% transparent
			
        }
		}
		void OnTriggerExit2D(Collider2D other)
        {
            //only exectue OnPlayerEnter if the player collides with this token.
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) OnPlayerExit(player);
			
        }
		
		void OnPlayerExit(PlayerController player)
        {
			inside = false;
			_renderer.color = new Color(1f,1f,255f,255f);
			var ev = Schedule<PlayerExitsActiveZone>();
			ev.player = player;
			ev.act = this;
			
			
        }
		void OnPlayerEnter(PlayerController player)
        {
			var ev = Schedule<PlayerEntersActiveZone>();
			ev.player = player;
			ev.act = this;
        }

    }
}
