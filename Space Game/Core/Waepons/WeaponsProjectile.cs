using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Galaxy_Explovive.Game.GameObjects.Spacecraft;
using MonoGame.Extended;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Game.GameLogik;

namespace Galaxy_Explovive.Core.Waepons
{
    public class WeaponsProjectile
    {
        private readonly Spacecraft OriginShip;
        private Vector2 Position;
        private readonly Vector2 Direction;
        private readonly Color ProjectileColor;
        private readonly float Velocity;
        private readonly float Rotation;
        private float TravelDistance;
        private readonly float MaxTravelDistance;

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

        public void Update(GameTime gameTime, SoundManager soundManager, SpatialHashing<GameObject.GameObject> spatial)
        {
            var direction = Direction * Velocity * gameTime.ElapsedGameTime.Milliseconds;
            CheckForDamage(soundManager, spatial);
            TravelDistance += direction.Length();
            if (TravelDistance >= MaxTravelDistance) { Remove = true; }
            Position += direction;
        }

        public void Draw(TextureManager textureManager)
        {
            Texture2D projectile = textureManager.GetTexture("projectile");
            textureManager.SpriteBatch.Draw(projectile, Position, null,
                ProjectileColor, Rotation, new Vector2(16, 16), 0.25f, SpriteEffects.None, 0);
        }

        private void CheckForDamage(SoundManager soundManager, SpatialHashing<GameObject.GameObject> spatial)
        {
            var ships = ObjectLocator.GetObjectsInRadius<Spacecraft>(spatial, Position, 40);
            ships.Remove(OriginShip);
            if (ships.Count <= 0) { return; }
            soundManager.PlaySound("hit", (float)MyUtility.Random.NextDouble());
            ships[0].Hit(10);
            Remove = true;
        }

    }
}
