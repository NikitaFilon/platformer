using GameEngine.Collisions;
using GameEngine.Components;
using SharpDX;
using System;
using System.Collections.Generic;

namespace GameLibrary.Components
{
    public class PhysicsComponent : ObjectComponent
    {
        private List<Vector3> _impulses = new List<Vector3>();
        private Vector3 _gravity;
        private Vector3 _direction;
        private bool _isPlatformCollision;

        public bool IsPlatformCollision { get => _isPlatformCollision; }

        public float Strength { get; set; }

        public event Action Jumped;

        public PhysicsComponent(float strength = 0.2f, float gravity = 0.0034f)
        {
            Strength = strength;
            _gravity = Vector3.UnitY * -gravity;
            _direction = Vector3.Zero;
        }

        public void UpdateObjectPosition(float delta)
        {
            Vector3 acceleration = _gravity;
            for (int i = 0; i < _impulses.Count; i++)
            {
                acceleration += _impulses[i];
            }
            _impulses.Clear();

            _direction += acceleration;

            GameObject.MoveBy(_direction);

            foreach (var gameObject in GameObject.Scene.GameObjects)
            {
                if (GameObject == gameObject) continue;

                if (ObjectCollision.Intersects(GameObject.Collision, gameObject.Collision) && !gameObject.WasCollision)
                {
                    if (GameObject.Position.Y > gameObject.Position.Y)
                    {
                        gameObject.WasCollision = true;
                        _isPlatformCollision = true;
                        GameObject.MoveBy(-_direction);
                        _direction.Y = 0;
                        AddImpulse(Vector3.UnitY * Strength);

                        if (gameObject.IsPlatform)
                        {
                            Jumped?.Invoke();
                        }
                    }

                    return;
                }
                else
                {
                    gameObject.WasCollision = false;
                }
                _isPlatformCollision = false;
            }
        }

        public void AddImpulse(Vector3 impulse)
        {
            _impulses.Add(impulse);
        }
    }
}