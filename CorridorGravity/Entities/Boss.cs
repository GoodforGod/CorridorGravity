using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CorridorGravity.Animations;
using CorridorGravity.Services;

namespace CorridorGravity.Entities
{
    class Boss : Entity, IAnimated<AnimationState>
    {
        public AnimationManager<AnimationState> AniManager { get; set; }

        GlideAnimation GlideAnim    { get; set; }
        ServiceLocator SLocator     { get; set; }

        /// <summary>
        /// Invisible cooldown time
        /// </summary>
        DateTime ICooldown;

        /// <summary>
        /// Appear cooldown time
        /// </summary>
        DateTime ACooldown;

        /// <summary>
        /// Offset for boss body
        /// </summary>
        Vector2 RelocateOffset;
        Vector2 OPosition;

        Rectangle EyeWhiteSource    { get; set; }
        Rectangle EyeRedSource      { get; set; }
        Rectangle EyeBlackSource    { get; set; }

        Vector2 EyeWhitePosition    { get; set; }
        Vector2 EyeRedPosition      { get; set; }
        Vector2 EyeBLackPosition    { get; set; }

        Vector2 EyeLookAtTagetOffset;

        float   Alpha;
        float   FadeRatio;
        float   AlphaTarget { get; set; }
        bool    FadeTarget  { get; set; }

        bool    IsSummoned  { get; set; }

        /// <summary>
        /// Boss delay appear time
        /// </summary>
        double  ADelayTime = 5;

        /// <summary>
        /// Boss invisible delay time
        /// </summary>
        double  IDelayTime = 3;

        /// <summary>
        /// Random generator for next position & magic <see cref="Magic"/>
        /// </summary>
        Random PRandomizer = new Random(Guid.NewGuid().GetHashCode());
        Random SRandomizer = new Random(Guid.NewGuid().GetHashCode());

        public override GroundState GState
        {
            get { return base.GState; }
            set
            {
                base.GState = value;
                RecalculateEyeCoordinates();
            } }

        public Boss(World world, Vector2 position, params object[] param) : base(world, position, param)
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();

            this.SLocator = ServiceLocator.Instance;

            this.AniManager = new AnimationManager<AnimationState>();
            Load(this.GetType().Name);

            this.GlideAnim = new GlideAnimation(5);

            this.RelocateOffset     = new Vector2(AniManager.CSource.Width, AniManager.CSource.Height);

            this.EyeWhiteSource     = SLocator.PLManager.TSources[TTypes.EYE_WHITE];
            this.EyeRedSource       = SLocator.PLManager.TSources[TTypes.EYE_RED];
            this.EyeBlackSource     = SLocator.PLManager.TSources[TTypes.EYE_BLACK];

            this.Alpha = 0f;
            this.AlphaTarget = 1f;
            this.FadeRatio = 0.02f;
            this.FadeTarget = true;

            this.Origin = new Vector2(AniManager.CSource.Width / 2, AniManager.CSource.Height / 2);
            this.EState = EntityState.INVISIBLE;
            this.AState = AnimationState.IDLE;

            this.ICooldown = ACooldown = DateTime.Now + TimeSpan.FromSeconds(6);
            this.Position = OPosition = new Vector2(WCore.LLimits.Width / 2, WCore.LLimits.Height / 2);

            this.RecalculateEyeCoordinates();
        }

        /// <summary>
        /// Recalculates next eye position depening on dimention
        /// </summary>
        public void RecalculateEyeCoordinates()
        {
            Vector2 EyeWhiteOffset  = Vector2.Zero;
            Vector2 EyeRedOffset    = Vector2.Zero;
            Vector2 EyeBlackOffset  = Vector2.Zero;

            switch (GState)
            {
                case GroundState.BOTTOM:
                    EyeWhiteOffset  = new Vector2(0, 28);
                    EyeRedOffset    = new Vector2(0, 7);
                    break;
                case GroundState.RIGHT:
                    EyeWhiteOffset  = new Vector2(-21 * 6, -24);
                    EyeRedOffset    = new Vector2(-32, 8);
                    EyeBlackOffset  = new Vector2(-18, 2);
                    break;
                case GroundState.LEFT:
                    EyeWhiteOffset  = new Vector2(-23 * 8, -24);
                    EyeRedOffset    = new Vector2(-39, 8);
                    EyeBlackOffset  = new Vector2(-18, 2);
                    break;
                case GroundState.TOP:
                    EyeWhiteOffset  = new Vector2(0, -28);
                    EyeRedOffset    = new Vector2(0, -7);
                    break;
                default: break;
            }

            this.EyeWhitePosition = new Vector2(Origin.X - EyeWhiteSource.Width / 2 + EyeWhiteOffset.X,
                                                Origin.Y - EyeWhiteSource.Height / 2 + EyeWhiteOffset.Y);
            this.EyeRedPosition = new Vector2(EyeWhitePosition.X + EyeWhiteSource.Width / 2 - EyeRedSource.Width / 2 + EyeRedOffset.X,
                                                EyeWhitePosition.Y + EyeWhiteSource.Height / 2 - EyeRedSource.Height / 2 + EyeRedOffset.Y);
            this.EyeBLackPosition = new Vector2(EyeRedPosition.X + EyeRedSource.Width / 2 - EyeBlackSource.Width / 2 + EyeBlackOffset.X,
                                                EyeRedPosition.Y + EyeRedSource.Height / 2 - EyeBlackSource.Height / 2 + EyeBlackOffset.Y);

            this.EyeLookAtTagetOffset = Vector2.Zero;
        }

        public void Load(string name)
        {
            this.AniManager.Load(name);
        }

        /// <summary>
        /// Relocates boss posotion
        /// </summary>
        protected void Relocate()
        {
            if (FadeTarget)
            {
                if (Alpha <= 1f)
                    Alpha += FadeRatio;
                else
                {
                    EState = EntityState.ACTIVE;
                    ACooldown = DateTime.Now;
                }
            }
            else
            {
                if (Alpha >= 0f)
                    Alpha -= FadeRatio;
                else
                {
                    EState = EntityState.INVISIBLE;
                    ICooldown = DateTime.Now;

                    this.Position.X = OPosition.X = PRandomizer.Next(WCore.LLimits.X + (int)RelocateOffset.X, 
                                                                    WCore.LLimits.Width - (int)RelocateOffset.X);
                    this.Position.Y = OPosition.Y = PRandomizer.Next(WCore.LLimits.Y + (int)RelocateOffset.X,
                                                                    WCore.LLimits.Height - (int)RelocateOffset.Y);
                }
            }
        }

        /// <summary>
        /// Summons magic entities
        /// </summary>
        protected void Summon()
        {
            var magicVelocity = 200f;
            var nextMagic = MagicState.FIST;
            var magicPosition = Vector2.One;
            var magicDirection = SRandomizer.Next(2);
            var offset = 72;
            var alpha = 0.85f;

            var RHLimit = WCore.LLimits.Height - offset;
            var RWLimit = WCore.LLimits.Width - offset;
            var LHLimit = WCore.LLimits.Y + offset;
            var LWLimit = WCore.LLimits.X + offset;

            var randSpellCount = PRandomizer.Next(1, 1+ WCore.PSCore/100);

            for (int i = 0; i < randSpellCount; i++)
            {
                // Choose which magic to spawn
                switch (SRandomizer.Next(3))
                {
                    case 0:
                        nextMagic = MagicState.FIST;
                        magicVelocity = 200; break;
                    case 1: nextMagic = MagicState.PORTAL; break;
                    case 2: nextMagic = MagicState.POOL; break;
                    case 3: WCore.WGState = (GroundState)SRandomizer.Next(0, 3); continue;
                    default: continue;
                }

                // Correct dicrection and velocity
                if (magicDirection == 0)
                {
                    magicDirection = -1;
                    magicVelocity = -magicVelocity;
                }

                // Sets magic coordinates depending on the current ground
                switch (GState)
                {
                    case GroundState.TOP:
                        if (nextMagic == MagicState.FIST || nextMagic == MagicState.PORTAL)
                            magicPosition.Y = PRandomizer.Next(LHLimit, RHLimit / 3);
                        else
                            magicPosition.Y = LHLimit;
                        magicPosition.X = PRandomizer.Next(LWLimit, RWLimit);
                        break;
                    case GroundState.LEFT:
                        if (nextMagic == MagicState.FIST || nextMagic == MagicState.PORTAL)
                            magicPosition.X = PRandomizer.Next(LWLimit, RWLimit / 3);
                        else
                            magicPosition.X = LWLimit;
                        magicPosition.Y = PRandomizer.Next(LHLimit, RHLimit);
                        break;
                    case GroundState.RIGHT:
                        if (nextMagic == MagicState.FIST || nextMagic == MagicState.PORTAL)
                            magicPosition.X = PRandomizer.Next((int)(RWLimit / 1.5), RWLimit);
                        else
                            magicPosition.X = RWLimit;
                        magicPosition.Y = PRandomizer.Next(LHLimit, RHLimit);
                        break;
                    case GroundState.BOTTOM:
                        if (nextMagic == MagicState.FIST || nextMagic == MagicState.PORTAL)
                            magicPosition.Y = PRandomizer.Next((int)(RHLimit / 1.5), RHLimit);
                        else
                            magicPosition.Y = RHLimit;
                        magicPosition.X = PRandomizer.Next(LWLimit, RWLimit);
                        break;
                    default: break;
                }

                // Corrects moving magic position
                if (nextMagic == MagicState.FIST)
                {
                    switch (GState)
                    {
                        case GroundState.TOP:
                        case GroundState.BOTTOM:
                            if (magicDirection == 1)
                                magicPosition.X = WCore.LLimits.X;
                            else
                                magicPosition.X = RWLimit;
                            break;
                        case GroundState.LEFT:
                        case GroundState.RIGHT:
                            if (magicDirection == 1)
                                magicPosition.Y = WCore.LLimits.Y;
                            else
                                magicPosition.Y = RHLimit;
                            break;
                        default: break;
                    }
                }

                WCore.AddToSpawnQueue(typeof(Magic).FullName, magicPosition, nextMagic, GState, magicVelocity, alpha);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void LootAtTarget()
        {
            var offset = 2;

            var posX = Position.X + EyeBLackPosition.X;
            var posY = Position.Y + EyeBLackPosition.Y;

            if (WCore.PPosition.X > posX + EyeRedSource.Width)
                EyeLookAtTagetOffset.X = offset;
            else if (WCore.PPosition.X < posX - EyeRedSource.Width)
                EyeLookAtTagetOffset.X = -offset;
            else
                EyeLookAtTagetOffset.X = 0;

            if (WCore.PPosition.Y > posY + EyeRedSource.Height * 2)
                EyeLookAtTagetOffset.Y = offset;
            else if (WCore.PPosition.Y < posY - EyeRedSource.Height)
                EyeLookAtTagetOffset.Y = -offset;
            else
                EyeLookAtTagetOffset.Y = 0;
        }

        public override void Update(GameTime gameTime)
        {
            AniManager.UpdateOnce(AState, gameTime);

            GlideAnim.Update(gameTime);

            LootAtTarget();

            // Update position via glie animation position
            switch(GState)
            {
                case GroundState.BOTTOM:
                case GroundState.TOP:
                    Position.Y = OPosition.Y + GlideAnim.COffset;
                    break;
                case GroundState.RIGHT: 
                case GroundState.LEFT:
                    Position.X = OPosition.X + GlideAnim.COffset;
                    break;
                default: break;
            }

            // If is ready to apper/disapper then relocate
            if (EState == EntityState.PASSIVE)
                Relocate();

            if( EState == EntityState.INVISIBLE && (DateTime.Now - ICooldown).TotalSeconds > IDelayTime)
            {
                // When invisiable and cooldown is gone then set ready to apper
                EState = EntityState.PASSIVE;
                FadeTarget = true;
            }
            else if (EState == EntityState.ACTIVE && (DateTime.Now - ACooldown).TotalSeconds > ADelayTime)
            {
                // When visiable and cooldown is gone then set ready to disapper
                EState = EntityState.PASSIVE;
                FadeTarget = false;
            }
            else if(EState == EntityState.ACTIVE && !IsSummoned)
            {
                // Do stuff when appeared and have time till cooldown
                IsSummoned = true;
                Summon();
            }
            else if(EState == EntityState.INVISIBLE)
            {
                // Do stuff when invisiable and have time till cooldown
                IsSummoned = false;
            }
        }

        public override void Draw(SpriteBatch batcher, GameTime gameTime)
        {
            // Eye white
            batcher.Draw(Sprite,
                          Position + EyeWhitePosition,
                          EyeWhiteSource,
                          TColor * Alpha,
                          Angle,
                          Origin,
                          Scale,
                          CEffect,
                          .0f);
            // Eye red
            batcher.Draw(Sprite,
                          Position + EyeRedPosition + EyeLookAtTagetOffset,
                          EyeRedSource,
                          TColor * Alpha,
                          Angle,
                          Origin,
                          Scale,
                          CEffect,
                          .0f);
            // Eye black
            batcher.Draw(Sprite,
                          Position + EyeBLackPosition + EyeLookAtTagetOffset,
                          EyeBlackSource,
                          TColor * Alpha,
                          Angle,
                          Origin,
                          Scale,
                          CEffect,
                          .0f);
            // Body
            batcher.Draw(Sprite,
                            Position,
                            AniManager.CSource,
                            TColor * Alpha,
                            Angle,
                            Origin,
                            Scale,
                            CEffect,
                            .0f);
        }
    }
}
