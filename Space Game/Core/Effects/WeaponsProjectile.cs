using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Space_Game.Core.Maths;
using Space_Game.Core.TextureManagement;
using Space_Game.Game.GameObjects;
using System.Linq;

namespace Space_Game.Core.Effects
{
    public class WeaponsProjectile
    {
        private Ship OriginShip;
        private Vector2 Position;
        private Vector2 Direction;
        private Color ProjectileColor;
        private float Velocity;
        private float Rotation;
        private float TravelDistance;
        private float MaxTravelDistance;

        public bool Remove;

        public WeaponsProjectile(Ship originShip, Ship target, Color color, float maxTravelDistance)
        {
            Position = originShip.Position;
            OriginShip = originShip;
            MaxTravelDistance = maxTravelDistance;
            ProjectileColor = color;
            Velocity = 0.8f;
            Direction = (target.Position - Position).NormalizedCopy();
            Rotation = MyMathF.GetInstance().GetRotation(Direction);
        }

        public void Update(GameTime gameTime)
        {
            var direction = Direction * Velocity * gameTime.ElapsedGameTime.Milliseconds * Globals.mTimeWarp;
            GiveDamage();
            TravelDistance += direction.Length();
            if (TravelDistance >= MaxTravelDistance) { Remove = true; }
            Position += direction;
        }

        public void Draw()
        {
            Texture2D projectile = TextureManager.GetInstance().GetTexture("projectile");
            TextureManager.GetInstance().GetSpriteBatch().Draw(projectile, Position, null,
                ProjectileColor, Rotation, new Vector2(16, 16), 0.25f, SpriteEffects.None, 0);
        }

        private void GiveDamage()
        {
            var ships = Globals.mGameLayer.GetObjectsInRadius(Position, 40).OfType<Ship>().ToList();
            ships.Remove(OriginShip);
            if (ships.Count <= 0) { return; }
            Globals.mSoundManager.PlaySound("hit", (float)Globals.mRandom.NextDouble());
            ships[0].TakeDamage();
            Remove = true;
        }

    }
}
