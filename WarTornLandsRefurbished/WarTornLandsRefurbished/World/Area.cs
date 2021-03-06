﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World.Layers;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;
using WarTornLands.Infrastructure;

namespace WarTornLands.World
{
    public class Area
    {
        public Rectangle Bounds { get; private set; }
        LinkedList<Layer> _layers;

        public Area(Rectangle bounds)
        {
            Bounds = bounds;
            _layers = new LinkedList<Layer>();
        }

        public void AddLayer(Layer layer)
        {
            _layers.AddLast(layer);
        }

        /// <summary>
        /// Adds the area's layers to the Game's component list, so they will be
        /// automatically updated and drawn by the game loop.
        /// </summary>
        internal void Add()
        {
            foreach (Layer layer in _layers)
            {
                layer.Add();
            }
        }

        /// <summary>
        /// Removes the area's layers from the Game's component list, so they won't be
        /// automatically updated or drawn by the game loop.
        /// </summary>
        internal void Remove()
        {
            foreach (Layer layer in _layers)
            {
                layer.Remove();
            }
        }

        private bool Contains(Vector2 position)
        {
            if (Bounds.Left * Constants.TileSize < position.X &&
                Bounds.Top * Constants.TileSize < position.Y &&
                Bounds.Right * Constants.TileSize > position.X &&
                Bounds.Bottom * Constants.TileSize > position.Y)
            {
                return true;
            }

            return false;
        }

        internal bool IsPositionAccessible(Vector2 position)
        {
            // Check whether the area even contains the pixel position
            if (!Contains(position))
            {
                return true;
            }

            Vector2 localPos = new Vector2(position.X, position.Y);

            // Iterate through layers, check all TileLayers
            foreach (Layer layer in _layers)
            {
                if (layer is TileLayer && (layer as TileLayer).IsPositionAccessible(position) == false)
                    return false;
            }

            // Accessible if all TileLayers had empty tiles
            return true;
        }

        public List<Entity> GetEntitiesAt(Vector2 position)
        {
            List<Entity> result = new List<Entity>();

            foreach (Layer layer in _layers)
            {
                if (layer is EntityLayer)
                {
                    result.AddRange((layer as EntityLayer).GetEntitiesAt(position));
                }
            }

            return result;
        }

        public List<Entity> GetEntitiesAt(Vector2 position, float radius)
        {
            List<Entity> result = new List<Entity>();

            foreach (Layer layer in _layers)
            {
                if (layer is EntityLayer)
                {
                    result.AddRange((layer as EntityLayer).GetEntitiesAt(position, radius));
                }
            }

            return result;
        }
    }
}
