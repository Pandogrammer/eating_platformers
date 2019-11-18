using UnityEngine;

namespace Eaters
{
    public class EaterDummy : MonoBehaviour
    {
        private UnityEater eater;
    
        private int movingTime = 2;
        private float movingTimer = 0;
        private bool moving;
        private int movingDirection;
        void Start()
        {
            eater = GetComponent<UnityEater>();
        }

        // Update is called once per frame
        void Update()
        {
            RandomMoving();
            if(!moving) RandomEating();
        }

        private void RandomEating()
        {
            eater.DetectFood();
            if(eater.detectedFood != null)
                eater.TryToEatFood();
        }

        private void RandomMoving()
        {
            if (movingTimer > movingTime)
            {
                if (moving)
                {
                    moving = false;
                    movingTimer = -2;
                }
                else
                {
                    moving = true;
                    movingDirection = Random.Range(1, 3);
                    movingTimer = 0;
                }
            }

            if (moving)
            {
                switch (movingDirection)
                {
                    case 1:
                        if(Random.Range(0, 2) > 0)
                            eater.Rotate(Random.Range(-1, 2));
                        eater.Move(-1);
                        break;
                    case 2:
                        if(Random.Range(0, 2) > 0)
                            eater.Rotate(Random.Range(-1, 2));
                        eater.Move(1);
                        break;
                }
            }

            movingTimer += Time.deltaTime;
        }
    }
}
