using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
	
	public class PlayerEntersForceZoneRight : Simulation.Event<PlayerEntersForceZoneRight>
	{
		public PlayerController player;
        public ForceZone zone;
		public Vector2 vel;
		public int push;
		PlatformerModel model = Simulation.GetModel<PlatformerModel>();
		
		       public override void Execute()
        {
           //AudioSource.PlayClipAtPoint(token.tokenCollectAudio, token.transform.position);
		   //model.player.GravDir = -1;
		   //model.player.jumpState = JumpState.jummping;
		   vel.x = -6.8f;
		   vel.y = 0;
		   push = 3;
		   player.addForce(vel, push);
		   
               	
        }
	}
	public class PlayerEntersForceZoneLeft : Simulation.Event<PlayerEntersForceZoneLeft>
	{
		public PlayerController player;
        public ForceZone zone;
		public Vector2 vel;
		public int push;
		PlatformerModel model = Simulation.GetModel<PlatformerModel>();
		
		       public override void Execute()
        {
           //AudioSource.PlayClipAtPoint(token.tokenCollectAudio, token.transform.position);
		   //model.player.GravDir = -1;
		   //model.player.jumpState = JumpState.jummping;
		   vel.x = 6.8f;
		   vel.y = 0;
		   push = 1;
		   player.addForce(vel, push);
		   
               	
        }
	}
	public class PlayerEntersForceZoneUp : Simulation.Event<PlayerEntersForceZoneUp>
	{
		public PlayerController player;
        public ForceZone zone;
		public Vector2 vel;
		public int push;
		PlatformerModel model = Simulation.GetModel<PlatformerModel>();
		
		       public override void Execute()
        {
           //AudioSource.PlayClipAtPoint(token.tokenCollectAudio, token.transform.position);
		   //model.player.GravDir = -1;
		   //model.player.jumpState = JumpState.jummping;
		   vel.x = 0;
		   vel.y = 6.8f;
		   push = 0;
		   player.addForce(vel, push);
		   
               	
        }
	}
	public class PlayerEntersForceZoneDown : Simulation.Event<PlayerEntersForceZoneDown>
	{
		public PlayerController player;
        public ForceZone zone;
		public Vector2 vel;
		public int push;
		PlatformerModel model = Simulation.GetModel<PlatformerModel>();
		
		       public override void Execute()
        {
           //AudioSource.PlayClipAtPoint(token.tokenCollectAudio, token.transform.position);
		   //model.player.GravDir = -1;
		   //model.player.jumpState = JumpState.jummping;
		   vel.x = 0;
		   vel.y = -6.8f;
		   push = 2;
		   player.addForce(vel, push);
		   
               	
        }
	}
	
}
