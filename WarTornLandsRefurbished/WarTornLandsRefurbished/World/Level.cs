﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World.Layers;
using Microsoft.Xna.Framework;
using WarTornLands;
using WarTornLands.Entities;
using System.Xml;
using WarTornLands.Entities.Modules.Draw;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Entities.Modules.Die;
using WarTornLands.PlayerClasses;
using WarTornLands.Entities.Modules.Interact;
using WarTornLands.Infrastructure.Systems.DialogSystem;
using System.Xml.Linq;
using WarTornLands.Entities.Modules.Draw.ParticleSystem;
using WarTornLands.Infrastructure.Systems.SkyLight;

namespace WarTornLands.World
{
    public class Level
    {
        private Game _game;
        private Dictionary<string, Area> _areas;

        public Level(Game game)
        {
            _game = game;
            _areas = new Dictionary<string, Area>();
        }

        public bool AddArea(string name, Area area)
        {
            if (!_areas.ContainsKey(name))
            {
                _areas.Add(name, area);
                area.Add();
                return true;
            }

            return false;
        }

        public bool RemoveArea(string name)
        {
            if (_areas.ContainsKey(name))
            {
                _areas[name].Remove();
                _areas.Remove(name);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Queries the Tile Map whether a given pixel position is accessible.
        /// Entities are not considered in the collision check. This is done
        /// via GetEntitiesAt in the CollisionManager.
        /// </summary>
        /// <param name="position">Pixel position to check.</param>
        /// <returns></returns>
        public bool IsPositionAccessible(Vector2 position)
        {
            // TODO only check areas near the player
            foreach (KeyValuePair<string, Area> pair in _areas)
            {
                if (pair.Value.IsPositionAccessible(position) == false)
                    return false;
            }

            return true;
        }

        public List<Entity> GetEntitiesAt(Vector2 position)
        {
            List<Entity> result = new List<Entity>();

            // TODO only check areas near the player
            foreach (KeyValuePair<string, Area> pair in _areas)
            {
                result.AddRange(pair.Value.GetEntitiesAt(position));
            }

            return result;
        }

        public List<Entity> GetEntitiesAt(Vector2 position, float radius)
        {
            List<Entity> result = new List<Entity>();

            // TODO only check areas near the player
            foreach (KeyValuePair<string, Area> pair in _areas)
            {
                result.AddRange(pair.Value.GetEntitiesAt(position, radius));
            }

            return result;
        }

        /// <summary>
        /// Loads a test level.
        /// </summary>
        public void LoadTestLevel()
        {
            Area area1 = new Area(new Rectangle(0, 0, 10, 10));

            TileLayer layer1 = new TileLayer(_game, 0);
            Tile[,] grid1 = new Tile[10, 10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (x == 3 || y == 5)
                        grid1[x, y].TileNum = 3;
                    else
                        grid1[x, y].TileNum = 2;
                }
            }
            grid1[0, 0].TileNum = 0;
            layer1.LoadGrid(grid1, false, "grass", false);
            area1.AddLayer(layer1);

            TileLayer layer2 = new TileLayer(_game, 90);
            Tile[,] grid2 = new Tile[10, 10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    grid2[x, y].TileNum = 0;
                }
            }
            grid2[3, 3].TileNum = 6;
            grid2[4, 3].TileNum = 6;
            grid2[5, 3].TileNum = 6;
            grid2[5, 4].TileNum = 6;
            grid2[5, 5].TileNum = 6;
            grid2[4, 5].TileNum = 6;
            grid2[3, 5].TileNum = 6;

            layer2.LoadGrid(grid2, false, "grass", true);
            area1.AddLayer(layer2);

            EntityLayer layer3 = new EntityLayer(_game, 99);
            StaticDrawer sd = new StaticDrawer();
            sd.Texture = _game.Content.Load<Texture2D>("Schatztruhe");
            Entity staticTest = new Entity((_game as Game1), new Vector2(50, 50), "loch");
            staticTest.AddModule(sd);
            staticTest.AddModule(new ExplodeAndLoot(new PlayerClasses.Items.Item(PlayerClasses.Items.ItemTypes.Potion)));
            staticTest.Health = 100;
            layer3.AddEntity(staticTest);
            area1.AddLayer(layer3);

            StaticDrawer dialogTestDrawer = new StaticDrawer();
            dialogTestDrawer.Texture = _game.Content.Load<Texture2D>("Schatztruhe");
            Entity dialogTest = new Entity((_game as Game1), new Vector2(-20, 150), "chest");
            dialogTest.AddModule(dialogTestDrawer);

            List<Conversation> cons = new List<Conversation>();
            Conversation con = new Conversation("1");
            con.Add(new TextLine("Once upon a time there was a man named Jason"));
            con.Add(new TextLine("He lived in a camp near his place of death, Crystal~Lake"));
            con.Add(new TextLine("and he wore a Hockey~Mask."));
            con.Add(new TextLine("From time to time Jason went out to kill random people with his machete"));
            con.Add(new TextLine("The end."));
            List<Option> ops = new List<Option>();
            ops.Add(new Option("option1", "#this"));
            ops.Add(new Option("option2", "#this"));
            ops.Add(new Option("option3", "#this"));
            ops.Add(new Option("option4", "#this"));
            Options op = new Options(ops);
            con.Add(op);
            cons.Add(con);
            dialogTest.AddModule(new Dialog(cons, dialogTest));
            layer3.AddEntity(dialogTest);

            //torch
            List<Texture2D> pL = new List<Texture2D>();
            pL.Add(Game1.Instance.Content.Load<Texture2D>("flame3"));
            Entity torch = new Entity(Game1.Instance, new Vector2(50, 150), "torch");
            ParticleSystem pSystem = new ParticleSystem(
                new EmitterSetting()
                {
                    DirectionX = new Range() { Min = -1, Max = 1 },
                    DirectionY = new Range() { Min = -1, Max = -3 },
                    AnglePermutation = new Range() { Min = -1, Max = 1 },
                    Lifetime = new Range() { Min = 1000, Max = 1500 },
                    MaxParticles = new Range(150),
                    Size = new Range() { Min = 0.1f, Max = 0.3f },
                    SpeedX = new Range() { Min = -1, Max = 1 },
                    SpeedY = new Range() { Min = -0.5f, Max = -1.5f },
                    Alpha = new Range(1),
                    AlphaDecay = new Range(0.01f, 0.1f)

                },
        pL);
            StaticDrawer torchlight = new StaticDrawer();
            torchlight.IsLight = true;

            torchlight.Texture = Game1.Instance.Content.Load<Texture2D>("flame3");
            
            torch.AddModule(new DualDraw(torchlight,pSystem));
     //       torch.AddModule(pSystem);
            layer3.AddEntity(torch);
            Lightmanager.AddLight(torch);
            //endtorch

            AddArea("Entenhausen", area1);
        }

        public void LoadLevel(string fileName)
        {
            // TODO TODO TODO TODO TODO TODO TODO TODO
            // TODO use XDocument because XmlTextReader sucks balls

            XmlTextReader reader = new XmlTextReader(fileName);
            reader.ReadToFollowing("world");
            reader = (XmlTextReader)reader.ReadSubtree();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "area")
                {
                    string isbn = reader.GetAttribute("ISBN");
                }
            }
        }


    }
}
