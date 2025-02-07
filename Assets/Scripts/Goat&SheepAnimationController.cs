using UnityEngine;

namespace Ursaanimation.CubicFarmAnimals
{
    public class AnimationController : MonoBehaviour
    {
        public Animator animator;
        public string walkForwardAnimation = "walk_forward";
        public string walkBackwardAnimation = "walk_backwards";
        public string turn90LAnimation = "turn_90_L";
        public string turn90RAnimation = "turn_90_R";
        public string runForwardAnimation = "run_forward";
        public string trotAnimation = "trot_forward";
        public string standtositAnimation = "stand_to_sit";
        public string sittostandAnimation = "sit_to_stand";
        public string shuffleRAnimation = "shuffle_R";
        public string shuffleLAnimation = "shuffle_L"; // not in use here but it exists too
        public string eatingAnimation = "eat";
        public string attackAnimation = "attack";
        public string hitReactionAnimation = "hit_reaction";
        public string deathAnimation = "die";

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                animator.Play(walkForwardAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                animator.Play(walkBackwardAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                animator.Play(turn90LAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                animator.Play(turn90RAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                animator.Play(runForwardAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                animator.Play(trotAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                animator.Play(standtositAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                animator.Play(sittostandAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                animator.Play(shuffleRAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                animator.Play(eatingAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                animator.Play(attackAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                animator.Play(hitReactionAnimation);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                animator.Play(deathAnimation);
            }
        }
    }
}
