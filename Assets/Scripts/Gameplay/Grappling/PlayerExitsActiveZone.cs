using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
	
	public class PlayerExitsActiveZone : Simulation.Event<PlayerExitsActiveZone>
	{
		public PlayerController player;
		public InteractableInstance act;
		
		

		
        public override void Execute()
        {
           //AudioSource.PlayClipAtPoint(token.tokenCollectAudio, token.transform.position);
		   //model.player.GravDir = -1;
		   //model.player.jumpState = JumpState.jummping;
		   
		   player.InZone(false, act.transform.position);
		   
               	
        }
	}
}
