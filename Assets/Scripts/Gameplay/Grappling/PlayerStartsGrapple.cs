using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
	public class PlayerStartsGrapple : Simulation.Event<PlayerStartsGrapple>
	{
		public PlayerController player;
		public Vector3 opos;
		
		public override void Execute()
        {
           //AudioSource.PlayClipAtPoint(token.tokenCollectAudio, token.transform.position);
		   //model.player.GravDir = -1;
		   //model.player.jumpState = JumpState.jummping;
		  
		   player.startGrapple(opos);
               	
        }
	}
}