using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Galaxy_Explovive.Game.GameObjects.Spacecraft;
using MonoGame.Extended;
using Galaxy_Explovive.Core.GameLogik;
using Galaxy_Explovive.Core.Utility;

namespace Galaxy_Explovive.Core.Waepons
{
    public class WeaponsProjectile
    {
        private Spacecraft OriginShip;
        private Vector2 Position;
        private Vector2 Direction;
        private Color ProjectileColor;
        private float Velocity;
        private float Rotation;
        private float TravelDistance;
        private float MaxTravelDistance;

        public bool Remove;

        public WeaponsProjectile(Spacecraft originShip, Spacecraft target, Color color, float maxTravelDistance)
        {
            Position = originShip.Position;
            OriginShip = originShip;
            MaxTravelDistance = maxTravelDistance;
            ProjectileColor = color;
            Velocity = 0.5f;
            Direction = (target.Position - Position).NormalizedCopy();
            Rotation = MyUtility.GetAngle(Position, Vector2.Zero);
        }

        public void Update(GameTime gameTime)
        {
            var direction = Direction * Velocity * gameTime.ElapsedGameTime.Milliseconds;
            GiveDamage();
            TravelDistance += direction.Length();
            if (TravelDistance >= MaxTravelDistance) { Remove = true; }
            Position += direction;
        }

        public void Draw()
        {
            Texture2D projectile = TextureManager.Instance.GetTexture("projectile");
            TextureManager.Instance.GetSpriteBatch().Draw(projectile, Position, null,
                ProjectileColor, Rotation, new Vector2(16, 16), 0.25f, SpriteEffects.None, 0);
        }

        private void GiveDamage()
        {
            var ships = ObjectLocator.Instance.GetObjectsInRadius(Position, 40).OfType<Spacecraft>().ToList();
            ships.Remove(OriginShip);
            if (ships.Count <= 0) { return; }
            Globals.mSoundManager.PlaySound("hit", (float)MyUtility.Random.NextDouble());
            ships[0].Hit(10);
            Remove = true;
        }

    }
}
