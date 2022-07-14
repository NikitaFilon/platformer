using GameEngine.Collisions;
using GameEngine.Graphics;
using GameEngine.Scripts;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Scripts
{
    class PlatformsMovementScript : Script
    {
        private float _speedX;
        private float _maxX;
        private float _minX;
        public Game3DObject _player;

        public PlatformsMovementScript(Game3DObject player, float speedX, float maxX, float minX)
        {

            _speedX = speedX;
            _maxX = maxX;
            _minX = minX;
            _player = player;
        }

        public override void Update(float delta)
        {
            var positionToMove = new Vector3(_speedX, 0, 0);
            GameObject.MoveBy(positionToMove);

            if (ObjectCollision.Intersects(GameObject.Collision, _player.Collision))
            {
                _player.MoveBy(positionToMove);
            }

            if (GameObject.Position.X >= _maxX || GameObject.Position.X <= _minX)
            {
                _speedX *= -1;
            }
        }
    }
}
