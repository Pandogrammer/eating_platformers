using UnityEngine;

namespace Chefs
{
    public class ChefDummy : MonoBehaviour
    {
        private UnityChef chef;
    
        private int movingTime = 2;
        private float movingTimer = 0;
        private bool moving;
        private int movingDirection;
        void Start()
        {
            chef = GetComponent<UnityChef>();
        }

        // Update is called once per frame
        void Update()
        {
            RandomMoving();
            if(!moving) RandomDropping();
        }

        private void RandomDropping()
        {
            chef.DropFood();
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
                            chef.Rotate(Random.Range(-1, 2));
                        chef.Move(-1);
                        break;
                    case 2:
                        if(Random.Range(0, 2) > 0)
                            chef.Rotate(Random.Range(-1, 2));
                        chef.Move(1);
                        break;
                }
            }

            movingTimer += Time.deltaTime;
        }
    }
}
