using UnityEngine;

namespace Platformer.Mechanics
{
	/// version 0.01.2
    /// <summary>
    /// This class animates all interactable instances in a scene.
    /// This allows a single update call to animate hundreds of sprite 
    /// animations.
    /// If the interactables property is empty, it will automatically find and load 
    /// all interactable instances in the scene at runtime.
    /// </summary>
    public class InteractableController : MonoBehaviour
    {/*
        //[Tooltip("Frames per second at which interactables are animated.")]
        public float frameRate = 12;
        //[Tooltip("Instances of interactables which are animated. If empty, interactable instances are found and loaded at runtime.")]
        public InteractableInstance[] interactables;

        float nextFrameTime = 0;

        [ContextMenu("Find All interactables")]
        void FindAllInteractablesInScene()
        {
            interactables = UnityEngine.Object.FindObjectsOfType<InteractableInstance>();
        }

        void Awake()
        {
            //if interactables are empty, find all instances.
            //if interactables are not empty, they've been added at editor time.
            //if (interactables.Length == 0)
                FindAllInteractablesInScene();
            //Register all interactables so they can work with this controller.
            for (var i = 0; i < interactables.Length; i++)
            {
                interactables[i].interactableIndex = i;
                interactables[i].controller = this;
            }
        }

        void Update()
        {
            //if it's time for the next frame... 
			
            if (Time.time - nextFrameTime > (1f / frameRate))
            {
                //update all interactables with the next animation frame.
                for (var i = 0; i < interactables.Length; i++)
                {
                    var interactable = interactables[i];
                    //if interactable is null, it has been disabled and is no longer animated.
                    if (interactable != null)
                    {
                      //  interactable._renderer.sprite = interactable.sprites[interactable.frame];
                        if (interactable.collected && interactable.frame == interactable.sprites.Length - 1)
                        {
                            interactable.gameObject.SetActive(false);
                            interactables[i] = null;
                        }
                        else
                        {
                            interactable.frame = (interactable.frame + 1) % interactable.sprites.Length;
                        }
                    }
                }
                //calculate the time of the next frame.
                nextFrameTime += 1f / frameRate;
            }
			
        }
*/
    }
}