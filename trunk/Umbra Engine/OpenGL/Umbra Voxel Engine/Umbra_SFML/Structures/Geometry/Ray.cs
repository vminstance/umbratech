using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Umbra.Structures.Geometry;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Console;

namespace Umbra.Structures.Geometry
{
    public class Ray
    {
        public Vector3 Origin { get; private set; }
        public Vector3 Direction { get; private set; }

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            if (direction == Vector3.Zero)
            {
                throw new Exception("Direction cannot be a zero vector");
            }
            Direction = direction;
            Direction.Normalize();
        }

        public float? Intersects(BoundingBox boundingBox)
        {
            if (boundingBox.Contains(Origin))
            {
                return 0.0F;
            }

            float shortest = float.MaxValue;

            // Intersection with faces:

            #region --Positive X--
            float planeX = boundingBox.Max.X;

            if (!((Origin.X > planeX && Direction.X > 0) || (Origin.X < planeX && Direction.X < 0)))
            {
                float normalDistance = Math.Abs(Origin.X - planeX);

                float length = normalDistance / (float)Math.Sqrt(1 - (Math.Pow(Direction.Y, 2) + Math.Pow(Direction.Z, 2)));

                Vector3 intersectionPoint = Origin + Direction * length;

                if (!((intersectionPoint.Y > boundingBox.Max.Y || intersectionPoint.Y < boundingBox.Min.Y) &&
                    (intersectionPoint.Z > boundingBox.Max.Z || intersectionPoint.Z < boundingBox.Min.Z))
                    && length < shortest)
                {
                    shortest = length;
                }
            }
            #endregion

            #region --Negative X--
            planeX = boundingBox.Min.X;

            if (!((Origin.X > planeX && Direction.X > 0) || (Origin.X < planeX && Direction.X < 0)))
            {
                float normalDistance = Math.Abs(Origin.X - planeX);

                float length = normalDistance / (float)Math.Sqrt(1 - (Math.Pow(Direction.Y, 2) + Math.Pow(Direction.Z, 2)));

                Vector3 intersectionPoint = Origin + Direction * length;

                if (!((intersectionPoint.Y > boundingBox.Max.Y || intersectionPoint.Y < boundingBox.Min.Y) &&
                    (intersectionPoint.Z > boundingBox.Max.Z || intersectionPoint.Z < boundingBox.Min.Z))
                    && length < shortest)
                {
                    shortest = length;
                }
            }
            #endregion

            #region --Positive Y--
            float planeY = boundingBox.Max.Y;

            if (!((Origin.Y > planeY && Direction.Y > 0) || (Origin.Y < planeY && Direction.Y < 0)))
            {
                float normalDistance = Math.Abs(Origin.Y - planeY);

                float length = normalDistance / (float)Math.Sqrt(1 - (Math.Pow(Direction.X, 2) + Math.Pow(Direction.Z, 2)));

                Vector3 intersectionPoint = Origin + Direction * length;

                if (!((intersectionPoint.X > boundingBox.Max.X || intersectionPoint.X < boundingBox.Min.X) &&
                    (intersectionPoint.Z > boundingBox.Max.Z || intersectionPoint.Z < boundingBox.Min.Z))
                    && length < shortest)
                {
                    shortest = length;
                }
            }
            #endregion

            #region --Negative Y--
            planeY = boundingBox.Min.Y;

            if (!((Origin.Y > planeY && Direction.Y > 0) || (Origin.Y < planeY && Direction.Y < 0)))
            {
                float normalDistance = Math.Abs(Origin.Y - planeY);

                float length = normalDistance / (float)Math.Sqrt(1 - (Math.Pow(Direction.X, 2) + Math.Pow(Direction.Z, 2)));

                Vector3 intersectionPoint = Origin + Direction * length;

                if (!((intersectionPoint.X > boundingBox.Max.X || intersectionPoint.X < boundingBox.Min.X) &&
                    (intersectionPoint.Z > boundingBox.Max.Z || intersectionPoint.Z < boundingBox.Min.Z))
                    && length < shortest)
                {
                    shortest = length;
                }
            }
            #endregion

            #region --Positive Z--
            float planeZ = boundingBox.Max.Z;

            if (!((Origin.Z > planeZ && Direction.Z > 0) || (Origin.Z < planeZ && Direction.Z < 0)))
            {
                float normalDistance = Math.Abs(Origin.Z - planeZ);

                float length = normalDistance / (float)Math.Sqrt(1 - (Math.Pow(Direction.X, 2) + Math.Pow(Direction.Y, 2)));

                Vector3 intersectionPoint = Origin + Direction * length;

                if (!((intersectionPoint.X > boundingBox.Max.X || intersectionPoint.X < boundingBox.Min.X) &&
                    (intersectionPoint.Y > boundingBox.Max.Y || intersectionPoint.Y < boundingBox.Min.Y))
                    && length < shortest)
                {
                    shortest = length;
                }
            }
            #endregion

            #region --Negative Z--
            planeZ = boundingBox.Min.Z;

            if (!((Origin.Z > planeZ && Direction.Z > 0) || (Origin.Z < planeZ && Direction.Z < 0)))
            {
                float normalDistance = Math.Abs(Origin.Z - planeZ);

                float length = normalDistance / (float)Math.Sqrt(1 - (Math.Pow(Direction.X, 2) + Math.Pow(Direction.Y, 2)));

                Vector3 intersectionPoint = Origin + Direction * length;

                if (!((intersectionPoint.X > boundingBox.Max.X || intersectionPoint.X < boundingBox.Min.X) &&
                    (intersectionPoint.Y > boundingBox.Max.Y || intersectionPoint.Y < boundingBox.Min.Y))
                    && length < shortest)
                {
                    shortest = length;
                }
            }
            #endregion


            if (shortest < float.MaxValue)
            {
                return shortest;
            }
            else
            {
                return null;
            }
        }
    }
}
