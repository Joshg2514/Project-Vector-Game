using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
	
	public class PlayerExitsForceZone : Simulation.Event<PlayerExitsForceZone>
	{
		public PlayerController player;
        public ForceZone zone;
		private Vector2 vel;
		PlatformerModel model = Simulation.GetModel<PlatformerModel>();
		
		       public override void Execute()
        {
           //AudioSource.PlayClipAtPoint(token.tokenCollectAudio, token.transform.position);
		   //model.player.GravDir = -1;
		   //model.player.jumpState = JumpState.jummping;
		   vel.x = 0;
		   vel.y = -9.8f;
		   player.stopForce(vel);
               	
        }
	}
	
}
