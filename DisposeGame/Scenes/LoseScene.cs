﻿using UILibrary;
using UILibrary.Backgrounds;
using UILibrary.Containers;
using UILibrary.Drawing;
using UILibrary.Elements;
using GameEngine.Game;
using SharpDX;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace GameLibrary.Scenes
{
    public class LoseScene : Scene
    {
        private string _scores;

        public LoseScene(string scores)
        {
            _scores = scores;
        }

        protected override UIElement InitializeUI(Loader loader, DrawingContext context, int screenWidth, int screenHeight)
        {
            context.NewNinePartsBitmap("buttonBitmap", loader.LoadBitmapFromFile(@"Textures\button.png"), 15, 121, 15, 46);
            context.NewNinePartsBitmap("buttonPressedBitmap", loader.LoadBitmapFromFile(@"Textures\buttonPressed.png"), 15, 121, 15, 46);
            context.NewBitmap("backgroundBitmapLose", loader.LoadBitmapFromFile(@"Textures\Lose.png"));
            context.NewSolidBrush("whiteBrush", new RawColor4(1f, 1f, 1f, 1f));
            context.NewTextFormat("textFormat", textAlignment: TextAlignment.Center, paragraphAlignment: ParagraphAlignment.Center);

            var ui = new UISequentialContainer(Vector2.Zero, new Vector2(screenWidth, screenHeight))
            {
                MainAxis = UISequentialContainer.Alignment.Center,
                CrossAxis = UISequentialContainer.Alignment.End,
                Background = new TextureBackground("backgroundBitmapLose")
            };
            var menu = new UISequentialContainer(Vector2.Zero, new Vector2(200, 200))
            {
            };
            ui.Add(new UIMarginContainer(menu, 0, 640, -100, 0));

            var scoresText = new UIText("Scores: " + _scores, new Vector2(160, 42), "textFormat", "whiteBrush");
            var startText = new UIText("Start", new Vector2(120, 42), "textFormat", "whiteBrush");
            var quitText = new UIText("Quit", new Vector2(120, 42), "textFormat", "whiteBrush");

            var startButton = new UILibrary.Elements.UILibrary(startText)
            {
                ReleasedBackground = new NinePartsTextureBackground("buttonBitmap"),
                PressedBackground = new NinePartsTextureBackground("buttonPressedBitmap")
            };
            var quitButton = new UILibrary.Elements.UILibrary(quitText)
            {
                ReleasedBackground = new NinePartsTextureBackground("buttonBitmap"),
                PressedBackground = new NinePartsTextureBackground("buttonPressedBitmap")
            };

            startButton.OnClicked += () =>
            {
                Game.ChangeScene(new MainGameScene());
            };
            quitButton.OnClicked += () =>
            {
                Game.CloseProgramm();
            };

            menu.Add(scoresText);
            menu.Add(new UIMarginContainer(startButton, 4f));
            menu.Add(new UIMarginContainer(quitButton, 4f));

            return ui;
        }
    }
}
