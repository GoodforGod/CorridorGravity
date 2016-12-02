using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using CorridorGravity.Services;

namespace CorridorGravity.Entities
{
    /// <summary>
    /// Types of entities which could be in the world
    /// </summary>
    enum ETypes
    {
        Player,
        Enemy,
        Boss,
        Magic
    }

    class World
    {
        public readonly Game CoreGame;
        ServiceLocator       SLocator { get; }

        public int FWidth   { get; set; }
        public int FHeight  { get; set; }

        public readonly float   Gravity = 9.8f;
        public readonly int     Offset = 20;

        private GroundState _WGState;
        public GroundState WGState {
            get { return _WGState; }
            set {
                _WGState = value;
                foreach(var entity in Entities)
                    entity.GState = _WGState;
            } }

        /// <summary>
        /// Entities who currently live in the world
        /// </summary>
        List<Entity> Entities;

        /// <summary>
        /// Entities who will be killed
        /// </summary>
        List<Entity> EntitiesToKill;

        /// <summary>
        /// Entities who will spawn in game
        /// </summary>
        List<Tuple<string, object[]>> EntitiesToSpawn;

        /// <summary>
        /// Current Level in the world
        /// </summary>
        Level CLevel { get; set; }

        /// <summary>
        /// Current level's limits 
        /// </summary>
        public Rectangle LLimits { get { return CLevel.Limits; } }

        /// <summary>
        /// Charater entity
        /// </summary>
        Player EPlayer { get; set; }
        public Vector2  PPosition   { get { return EPlayer.Position; } }
        public int      PHealth     { get { return EPlayer.Health; } }
        public int      PSCore      { get { return EPlayer.Score; } }
        public bool     IsPlayerAlive {
            get {
                if (EPlayer.EState == EntityState.DEAD)
                    return false;
                else return true;
            } }

        public World(Game game, Viewport viewport)
        {
            this.CoreGame   = game;
            this.FWidth     = viewport.Width;
            this.FHeight    = viewport.Height;

            this.SLocator = ServiceLocator.Instance;
            this.CLevel = new Level(FWidth, FHeight);

            Initialize();
        }

        public void Initialize()
        {
            this.Entities        = new List<Entity>();
            this.EntitiesToKill  = new List<Entity>();
            this.EntitiesToSpawn = new List<Tuple<string, object[]>>();

            this.WGState = GroundState.BOTTOM;

            this.EPlayer = (Player)Spawn(typeof(Player).FullName);
            this.Spawn(typeof(Enemy).FullName);
            this.Spawn(typeof(Boss).FullName);
        }

        protected Entity Spawn(string className) { return Spawn(className, new Vector2(LLimits.Width / 2, LLimits.Height / 2)); }
        protected Entity Spawn(string className, params object[] param)
        {
            var initParam = new object[1 + param.Length];
            initParam[0] = this;
            param.CopyTo(initParam, 1);

            var entity = (Entity)Activator.CreateInstance(Type.GetType(className), initParam);

            ETypes entType = (ETypes)Enum.Parse(typeof(ETypes), entity.GetType().Name);

            switch (entType)
            {
                case ETypes.Player: entity.Handler  = SLocator.CharHandler;
                                    entity.Sprite   = SLocator.PLManager.TLoad(SLocator.PLManager.TList[TTypes.PLAYER]);
                                    break;

                case ETypes.Enemy:  entity.Handler  = SLocator.AIHandler;
                                    entity.Sprite   = SLocator.PLManager.TLoad(SLocator.PLManager.TList[TTypes.ENEMY]);
                                    break;

                case ETypes.Boss:   entity.Sprite   = SLocator.PLManager.TLoad(SLocator.PLManager.TList[TTypes.BOSS]);
                                    break;

                case ETypes.Magic:  entity.Sprite   = SLocator.PLManager.TLoad(SLocator.PLManager.TList[TTypes.MAGIC]);
                                    break;
                default:            break;
            }
            entity.GState = WGState;

            Entities.Add(entity);

            return entity;
        }

        public void AddToSpawnQueue(string className, params object[] param)
        {
            EntitiesToSpawn.Add(new Tuple<string, object[]>(className, param));
        }

        protected void SpawnQueue()
        {
            // Spawn all entities in the list
            foreach (var entityTuple in EntitiesToSpawn)
                Spawn(entityTuple.Item1, entityTuple.Item2);

            // Wipe list
            EntitiesToSpawn.Clear();
        }

        public void Update(GameTime gameTime)
        {
            // Check for the need in spawner
            if (EntitiesToSpawn.Count != 0)
                SpawnQueue();

            // Updates all entities in world
            foreach (var entity in Entities)
                entity.Update(gameTime);

            // Collide all entities
            foreach (var a in Entities)
                foreach (var b in Entities)
                    if (!ReferenceEquals(a, b))
                    {
                        if (a.BoundingBox.Intersects(b.BoundingBox))
                            a.Touch(b);
                    }

            //Wipe all dead entities
            Entities.RemoveAll(e => EntitiesToKill.Contains(e));

            foreach(var entity in EntitiesToKill)
                if(entity is Enemy)
                    AddToSpawnQueue(typeof(Magic).FullName, entity.Position, MagicState.DEAD, WGState, 0f, 0.95f);

            EntitiesToKill.Clear();
        }

        /// <summary>
        /// Wipe entity from world
        /// </summary>
        public void Kill(Entity entity) { EntitiesToKill.Add(entity); }

        public void UnloadContent()
        {

        }

        /// <summary>
        /// Draw all entities in the world
        /// </summary>
        public void Draw(SpriteBatch batcher, GameTime gameTime)
        {
            CLevel.Draw(batcher);

            foreach (var entity in Entities)
                entity.Draw(batcher, gameTime);
        }
    }
}