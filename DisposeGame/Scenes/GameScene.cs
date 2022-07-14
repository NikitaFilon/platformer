using System;
using System.Collections.Generic;
using UILibrary;
using UILibrary.Backgrounds;
using UILibrary.Containers;
using UILibrary.Drawing;
using UILibrary.Elements;
using GameEngine.Animation;
using GameEngine.Collisions;
using GameEngine.Game;
using GameEngine.Graphics;
using GameLibrary.Components;
using GameLibrary.Scripts;
using GameLibrary.Scripts.Character;
using SharpDX;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using Sound;
using FuckGame.Scripts;
using FuckGame.Scripts.Bonus;

namespace GameLibrary.Scenes
{
    class MainGameScene : Scene
    {
        private Camera camera;

        private SharpAudioVoice _bulletPlayer;
        private SharpAudioVoice _die;
        private SharpAudioVoice _duck;
        private SharpAudioVoice _jump;
        private SharpAudioVoice _bonus;
        private SharpAudioVoice _music;
        public Game3DObject _player;
        private UIText _ammoCounter;
        private float _maxXAndY;
        List<Game3DObject> _platforms = new List<Game3DObject>();
        List<Game3DObject> _hostile = new List<Game3DObject>();

        Random random = new Random();

        protected override Camera CreateCamera()
        {
            return camera;
        }

        protected override void InitializeObjects(Loader loader, SharpAudioDevice audioDevice)
        {
            _bulletPlayer = new SharpAudioVoice(audioDevice, @"Sounds\cake.wav");
            _die = new SharpAudioVoice(audioDevice, @"Sounds\die.wav");
            _duck = new SharpAudioVoice(audioDevice, @"Sounds\duck.wav");
            _jump = new SharpAudioVoice(audioDevice, @"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Sounds\jump.wav");
            _bonus = new SharpAudioVoice(audioDevice, @"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Sounds\bonus.wav");
            _music = new SharpAudioVoice(audioDevice, @"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Sounds\music.wav");
            camera = new OrthoCamera(new Vector3(0, 0, -10f), rotY: 0.5f, fovY: 0.03f);

            var bullet = loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\ball7.fbx", new Vector3(0, 0, 0), new Vector3(0));
            bullet.Collision = new BoxCollision(0.03f, 0.03f);
            var player = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\player.fbx", new Vector3(0, 1, 0), new Vector3(0)));
            player.Collision = new BoxCollision(0.3f, 0.7f);
            player.AddChild(camera);
            var physics = new PhysicsComponent();
            physics.Jumped += () =>
            {
                _jump.Stop();
                _jump.Play();
            };
            player.AddComponent(physics);
            player.AddScript(new PhysicsScript(physics));
            player.AddScript(new PlayerMovementScript(camera, physics));
            _player = player;
            var eggBullet = new PlayerGunScript(bullet);
            player.AddScript(eggBullet);
            eggBullet.OnCakeShoot += () =>
            {
                _bulletPlayer.Stop();
                _bulletPlayer.Play();
            };

            var health = new HealthComponent(1);
            health.OnDeath += () =>
            {
                _music.Stop();
                _die.Stop();
                _die.Play();
                player.Scene.Game.ChangeScene(new LoseScene(_ammoCounter.Text));
            };
            health.OnDeath += () => Game.ChangeScene(new LoseScene(_ammoCounter.Text));
            player.AddComponent(health);

            var platform = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(0, 0, 0), new Vector3(0)));
            platform.Collision = new BoxCollision(2.2f, 0.7f);
            platform.IsPlatform = true;

            var platform1 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(3, 4, 0), new Vector3(0)));
            platform1.Collision = new BoxCollision(2.2f, 0.7f);
            //platform1.AddScript(new PlatformsMovementScript(player, 0.1f, 16, -16));
            platform1.IsPlatform = true;

            var platform2 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(0, 8, 0), new Vector3(0)));
            platform2.Collision = new BoxCollision(2.2f, 0.7f);
            platform2.IsPlatform = true;

            var platform3 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(6, 12, 0), new Vector3(0)));
            platform3.Collision = new BoxCollision(2.2f, 0.7f);
            platform3.IsPlatform = true;

            var platform4 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(-6, 12, 0), new Vector3(0)));
            platform4.Collision = new BoxCollision(2.2f, 0.7f);
            platform4.AddScript(new PlatformsMovementScript(player, 0.1f, 16, -16));
            platform4.IsPlatform = true;

            var platform5 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(4, 16, 0), new Vector3(0)));
            platform5.Collision = new BoxCollision(2.2f, 0.7f);
            platform5.IsPlatform = true;

            var platform6 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(8, 20, 0), new Vector3(0)));
            platform6.Collision = new BoxCollision(2.2f, 0.7f);
            platform6.IsPlatform = true;

            var platform7 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(10, 22, 0), new Vector3(0)));
            platform7.Collision = new BoxCollision(2.2f, 0.7f);
            platform7.AddScript(new PlatformsMovementScript(player, 0.1f, 16, -16));
            platform7.IsPlatform = true;

            var platform8 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(4, 26, 0), new Vector3(0)));
            platform8.Collision = new BoxCollision(2.2f, 0.7f);
            platform8.IsPlatform = true;

            var platform9 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(8, 30, 0), new Vector3(0)));
            platform9.Collision = new BoxCollision(2.2f, 0.7f);
            platform9.AddScript(new PlatformsMovementScript(player, 0.1f, 16, -16));
            platform9.IsPlatform = true;

            var platform10 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(12, 34, 0), new Vector3(0)));
            platform10.Collision = new BoxCollision(2.2f, 0.7f);
            platform10.IsPlatform = true;

            var platform11 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(8, 36, 0), new Vector3(0)));
            platform11.Collision = new BoxCollision(2.2f, 0.7f);
            platform11.IsPlatform = true;

            var platform12 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(10, 37, 0), new Vector3(0)));
            platform12.Collision = new BoxCollision(2.2f, 0.7f);
            platform12.IsPlatform = true;

            var platform13 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(12, 40, 0), new Vector3(0)));
            platform13.Collision = new BoxCollision(2.2f, 0.7f);
            platform13.IsPlatform = true;

            var platform14 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(4, 40, 0), new Vector3(0)));
            platform14.Collision = new BoxCollision(2.2f, 0.7f);
            platform14.IsPlatform = true;

            var platform15 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\platformScale3.fbx", new Vector3(0, 42, 0), new Vector3(0)));
            platform15.Collision = new BoxCollision(2.2f, 0.7f);
            platform15.IsPlatform = true;

            _platforms.Add(platform);
            _platforms.Add(platform1);
            _platforms.Add(platform2);
            _platforms.Add(platform3);
            _platforms.Add(platform4);
            _platforms.Add(platform5);
            _platforms.Add(platform6);
            _platforms.Add(platform7);
            _platforms.Add(platform8);
            _platforms.Add(platform9);
            _platforms.Add(platform10);
            _platforms.Add(platform11);
            _platforms.Add(platform12);
            _platforms.Add(platform13);
            _platforms.Add(platform14);
            _platforms.Add(platform15);

            var hostile1 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\bird.fbx", new Vector3(5, 25, 0), new Vector3(0, 0, 1f)));
            var healthEnemy1 = new HealthComponent(1);
            healthEnemy1.OnDeath += () =>
            {
                _duck.Stop();
                _duck.Play();
                healthEnemy1.GameObject.MoveBy(random.Next(-7, 6), healthEnemy1.GameObject.Position.Y + random.Next(0, 4), 0);
                healthEnemy1.Health = 1;
            };
            hostile1.Collision = new BoxCollision(1f, 1f);
            hostile1.AddComponent(healthEnemy1);
            var enemyScript = new EnemyScript(_player);
            hostile1.AddScript(enemyScript);

            _hostile.Add(hostile1);

            //bonus1
            var bonus1 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\speedup.fbx", new Vector3(2, 12, 0), new Vector3(0, 0, 1f)));
            bonus1.Collision = new BoxCollision(0.9f, 1f);
            var jumpBonusScript = new IncreasedJumpBonus(_player);
            jumpBonusScript.IsPicked += () =>
            {
                jumpBonusScript.GameObject.MoveBy(random.Next(-7, 6), jumpBonusScript.GameObject.Position.Y + random.Next(0, 4), 0);
                _bonus.Stop();
                _bonus.Play();
            };
            bonus1.AddScript(jumpBonusScript);

            _hostile.Add(bonus1);

            //bonus2
            var bonus2 = AddGameObject(loader.LoadGameObjectFromFile(@"C:\Учеба\3 курс\2 семестр\курсач\Platformer\DisposeGame\Models\speeddown.fbx", new Vector3(6, 35, 0), new Vector3(0, 0, 1f)));
            bonus2.Collision = new BoxCollision(1.2f, 1f);
            var speedBonusScript = new IncreasedSpeedBonus(_player);
            speedBonusScript.IsPicked += () =>
            {
                speedBonusScript.GameObject.MoveBy(random.Next(-7, 6), jumpBonusScript.GameObject.Position.Y + random.Next(0, 4), 0);
                _bonus.Stop();
                _bonus.Play();
            };
            bonus2.AddScript(speedBonusScript);

            _hostile.Add(bonus2);


           

            _music.Play();
        }

        protected override UIElement InitializeUI(Loader loader, DrawingContext context, int screenWidth, int screenHeight)
        {

            context.NewSolidBrush("neonBrush", new RawColor4(144f / 255f, 238f / 255f, 233f / 255f, 1f));
            context.NewTextFormat("ammoFormat",
                fontWeight: SharpDX.DirectWrite.FontWeight.Black,
                textAlignment: SharpDX.DirectWrite.TextAlignment.Center,
                paragraphAlignment: SharpDX.DirectWrite.ParagraphAlignment.Center);

            var ui = new UISequentialContainer(Vector2.Zero, new Vector2(screenWidth, screenHeight))
            {
                MainAxis = UISequentialContainer.Alignment.End,
                CrossAxis = UISequentialContainer.Alignment.Start
            };

                 _ammoCounter = new UIText("0", new Vector2(55, 10), "ammoFormat", "neonBrush");

            var ammoContainer = new UISequentialContainer(Vector2.Zero, new Vector2(0, 1100), false)
            {
                MainAxis = UISequentialContainer.Alignment.Start,
                CrossAxis = UISequentialContainer.Alignment.Center
            };
            ammoContainer.Add(_ammoCounter);

            ui.Add(ammoContainer);

            return ui;
        }

        public override void Update(float delta) 
        {
            base.Update(delta);
            if ( _player.Position.Y > _maxXAndY)
            {
                _ammoCounter.Text = ((int)_player.Position.Y).ToString();

            }
            if (_player.Position.Y > _maxXAndY)
            { 
                _maxXAndY = _player.Position.Y;
            }

            foreach (var platform in _platforms)
            {
                if (_player.Position.Y > platform.Position.Y + 11)
                {

                    platform.MoveBy( random.Next(-3,3), platform.Position.Y + random.Next(0, 4), 0);
                }
            }

            foreach (var enemy in _hostile)
            {
                if (_player.Position.Y > enemy.Position.Y + 11)
                {

                    enemy.MoveBy(random.Next(-7, 6), enemy.Position.Y + random.Next(0, 4), 0);
                }
            }

            if (_player.Position.Y < _maxXAndY - 15)
            {
                _music.Stop();
                _die.Stop();
                _die.Play();
                _player.Scene.Game.ChangeScene(new LoseScene(_ammoCounter.Text));
            }


            if (_player.Position.Y < -20)
            {
                _music.Stop();
                _die.Stop();
                _die.Play();
                this.Game.ChangeScene(new LoseScene(_ammoCounter.Text));
            }
        }

        public override void Dispose()
        {

            _bulletPlayer.Dispose();
            _die.Dispose();
            _duck.Dispose();
            base.Dispose();
        }

    }
}

