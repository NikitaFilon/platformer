using GameEngine.Collisions;
using GameEngine.Graphics;
using GameEngine.Scripts;
using GameLibrary.Components;
using SharpDX;
using System;

namespace FuckGame.Scripts.Bonus
{
    public class IncreasedJumpBonus : Script
    {
        private Game3DObject _player;
        private bool _isPicked;

        public event Action IsPicked;

        public IncreasedJumpBonus(Game3DObject player)
        {
            _player = player;
        }

        public override void Update(float delta)
        {
            var playerCollision = _player.Collision as BoxCollision;
            var newPlayerCollision = new BoxCollision(playerCollision.SizeX + 0.5f, playerCollision.SizeY + 0.5f);
            _player.Collision = newPlayerCollision;

            if (ObjectCollision.Intersects(newPlayerCollision, GameObject.Collision) && !_isPicked)
            {
                _player.GetComponent<PhysicsComponent>().Strength += 0.04f;
                _isPicked = true;
                IsPicked?.Invoke();
            }
            else
            {
                _isPicked = false;
            }

            _player.Collision = playerCollision;
        }
    }
}
