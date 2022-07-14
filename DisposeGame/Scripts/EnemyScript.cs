using GameEngine.Collisions;
using GameEngine.Graphics;
using GameEngine.Scripts;
using GameLibrary.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuckGame.Scripts
{
    public class EnemyScript : Script
    {
        private Game3DObject _player;

        public EnemyScript(Game3DObject player)
        {
            _player = player;
        }

        public override void Update(float delta)
        {
            if (ObjectCollision.Intersects(_player.Collision, GameObject.Collision))
            {
                _player.GetComponent<HealthComponent>().DealDamage(100);
            }
        }
    }
}