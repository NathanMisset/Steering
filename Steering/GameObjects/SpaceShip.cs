﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Opdracht3_Steering {
    internal class SpaceShip : SpriteGameObject {
        protected float mass;
        private Vector2 target;
        private float rotation;
        
        // add maxSteering, maxSpeed and arrivingRadius
        private float maxSteering;
        private float maxSpeed;
        private float arrivingRadius;
        private Vector2 desiredVelocity;
        private double distanceToTarget;
        private float speed;

        public SpaceShip(string assetName, Vector2 position, float scale, float maxSteering, float maxSpeed, float arrivingRadius, float mass)
            : base(assetName, 1, "spaceship") {
            layer = 2;
            this.position = position;
            velocity = Vector2.Zero;
            this.mass = mass;
            rotation = 0;
            origin = new Vector2(Width / 2f, Height / 2f);
            this.maxSteering = maxSteering;
            this.maxSpeed = maxSpeed;
            this.arrivingRadius = arrivingRadius;
            speed = maxSpeed;

        }

        public override void Update(GameTime gameTime) {
            // get target
            target = GameWorld.Find("target").Position;
            // calculate steering direction
            // 1
            desiredVelocity = target - position;
            // 2
            desiredVelocity = Truncate(desiredVelocity, speed);
            // 3
            desiredVelocity = desiredVelocity - velocity;
            // 4
            desiredVelocity = Truncate(desiredVelocity, maxSteering);
            // 5
            desiredVelocity = desiredVelocity / mass;
            // 6
            desiredVelocity = Truncate(desiredVelocity, speed);


            velocity += desiredVelocity;

            // arriving and stopping
            // 1
            desiredVelocity = target - position;
            desiredVelocity.X = desiredVelocity.X * desiredVelocity.X;
            desiredVelocity.Y = desiredVelocity.Y * desiredVelocity.Y;
            // 2
            distanceToTarget = Math.Sqrt(desiredVelocity.X + desiredVelocity.Y);
            // 3
            if (distanceToTarget < 5)
            {
                velocity = Vector2.Zero;
            }//4
            else if (distanceToTarget > arrivingRadius)
            {
                speed = maxSpeed;
            }
            else
            {//5
                speed *= (float)distanceToTarget / arrivingRadius;
            }
            // apply rotation
            if (velocity != Vector2.Zero) {
                var angle = (float)Math.Atan2(velocity.Y, velocity.X);
                rotation = angle + (float)Math.PI / 2;
            }
           

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (!visible || sprite == null)
                return;

            spriteBatch.Draw(sprite.Sprite, GlobalPosition, null, Color.White,
            rotation, origin, scale, SpriteEffects.None, 0.0f);
        }

        private Vector2 Truncate(Vector2 vector, float length) {
            if (!(vector.LengthSquared() > length*length)) return vector;
            vector.Normalize();
            vector *= length;
            return vector;
        }
    }
}