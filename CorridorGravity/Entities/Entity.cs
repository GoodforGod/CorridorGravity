using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using CorridorGravity.Animations;
using CorridorGravity.Controllers;

namespace CorridorGravity.Entities
{
    /// <summary>
    /// Represent state in which entity could be
    /// </summary>
    public enum EntityState
    {
        NONE,
        INPROGRESS,
        READY,
        ACTIVE,
        PASSIVE,
        DYING,
        DEAD,
        INVISIBLE
    }

    /// <summary>
    /// Basic ingame entity 
    /// </summary>
    class Entity
    {
        public World        WCore  { get; }
        public Color        TColor { get; set; }
        public Texture2D    Sprite { get; set; }

        /// <summary>
        /// Handles entities state
        /// </summary>
        public EntityController Handler { get; set; }

        public Vector2 Position;
        public Vector2 Origin   { get; protected set; }

        /// <summary>
        /// Sprite offset to correct draw
        /// </summary>
        public Vector2 SOffset  { get; internal set; }

        /// <summary>
        /// Used to check entities collision
        /// </summary>
        public Rectangle     BoundingBox;

        public Vector2       Velocity;
        public virtual float VLimit     { get { return 300f; } }
        public virtual float VInrease   { get { return 15f; } }

        /// <summary>
        /// Entities jump ratio/coef
        /// </summary>
        public virtual int   JRate      { get { return 20; } }

        /// <summary>
        /// OScale is to store original scale of the Source/Sprite to multiply it when resolution is changed
        /// Where "Scale" is currect resolution (OScale * Resolution coef)
        /// </summary>
        public          float   OScale  { get; set; }
        public virtual  float   Scale   { get; set; }
        public          float   Angle   { get; set; }
        public virtual  int     Health  { get; set; }
        public virtual  int     DPower  { get; set; }
        
        /// <summary>
        /// Current entity sprite effects
        /// </summary>
        public SpriteEffects        CEffect   { get; set; }
        public AnimationState       AState    { get; set; }
        public EntityState          EState    { get; set; }

        // Represent current entity ground state
        private GroundState         _GState;
        public virtual GroundState GState
        {
            get { return _GState; }

            set {
                switch (_GState = value)
                {
                    case GroundState.TOP:   Angle = 0f;                 CEffect |= SpriteEffects.FlipVertically;    break;
                    case GroundState.LEFT:  Angle = MathHelper.PiOver2; CEffect &= ~SpriteEffects.FlipVertically;   break;
                    case GroundState.RIGHT: Angle = MathHelper.PiOver2; CEffect |= SpriteEffects.FlipVertically;    break;
                    case GroundState.BOTTOM: Angle = 0f;                CEffect &= ~SpriteEffects.FlipVertically;   break;
                    default: break;
                } } }

        /// <summary>
        /// Determinates which direction entity is facing
        /// </summary>
        private int _Direction;
        public int Direction
        {
            get { return _Direction; }
            set {
                _Direction = value;
                if (_Direction == -1)
                    this.CEffect |= SpriteEffects.FlipHorizontally;
                else
                    this.CEffect = CEffect & ~SpriteEffects.FlipHorizontally;
            }
        }

        /// <summary>
        /// Check is entity still on the screen/level
        /// </summary>
        public virtual bool IsOnScreen
        { get {
                switch(GState)
                {
                    case GroundState.TOP:
                    case GroundState.BOTTOM:if (Position.X > WCore.LLimits.X 
                                                && Position.X < WCore.LLimits.Width - BoundingBox.Width)
                                                    return true; break;
                    case GroundState.LEFT:      
                    case GroundState.RIGHT: if (Position.Y > WCore.LLimits.Y 
                                                && Position.Y < WCore.LLimits.Height - BoundingBox.Width)
                                                    return true; break;
                    default:                        return false;
                }
                return false;
            } }

        /// <summary>
        /// Check is entity on the ground
        /// </summary>
        /// <returns></returns>
        public virtual bool IsGrounded
        { get {
                switch (GState)
                {
                    case GroundState.TOP:       if (Position.Y <= GetGround) return true; break;
                    case GroundState.LEFT:      if (Position.X <= GetGround) return true; break;
                    case GroundState.RIGHT:     if (Position.X >= GetGround) return true; break;
                    case GroundState.BOTTOM:    if (Position.Y >= GetGround) return true; break;
                    default:                                                 return false;
                }
                return false;
            } }

        /// <summary>
        /// Return entities ground position depening on the dimention
        /// </summary>
        public virtual int GetGround
        { get {
                // Top & Left uses the same value, as Right & Bottom use their same offset value
                switch (GState)
                {
                    case GroundState.TOP:       return WCore.LLimits.Y;
                    case GroundState.LEFT:      return WCore.LLimits.X + BoundingBox.Height;
                    case GroundState.RIGHT:     return WCore.LLimits.Width;
                    case GroundState.BOTTOM:    return WCore.LLimits.Height - BoundingBox.Height;
                    default:                    return BoundingBox.Height;
                }
            } }

        private Entity(World world)
        {
            this.WCore      = world;
            this.Sprite     = null;
            this.BoundingBox = Rectangle.Empty;
        }
        public  Entity(World world, Vector2 position) : this(world)
        {
            this.Position = position;
        }
        public  Entity(World world, Vector2 position, params object[] param) : this(world, position)
        {
            switch(param.Length)
            {
                case 1:
                    this.Angle      = (float)param[0];
                    break;
                case 2:
                    this.Angle      = (float)param[0];
                    this.Velocity   = (Vector2)param[1];
                    break;
                case 3:
                    this.Angle      = (float)param[0];
                    this.Velocity   = (Vector2)param[1];
                    this.Sprite     = (Texture2D)param[2];
                    break;
                case 4: 
                    this.Angle      = (float)param[0];
                    this.Velocity   = (Vector2)param[1];
                    this.Sprite     = (Texture2D)param[2];
                    this.BoundingBox = (Rectangle)param[3];
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Initialize default entity state
        /// </summary>
        public virtual void Initialize()
        {
            this.Health     = DPower  = Direction = _Direction = 1;
            this.Scale      = OScale = 1f;

            this.TColor     = Color.White;
            this.Origin     = Vector2.Zero;
        }

        /// <summary>
        /// Damage entity
        /// </summary>
        public virtual void Damage(Entity attacker, int damage)
        {
            this.Health -= damage;

            if (Health <= 0)
                Kill(this);
        }

        /// <summary>
        /// Kill entity
        /// </summary>
        public virtual void Kill(Entity killer) { WCore.Kill(this); }

        /// <summary>
        /// Touch other entity on collision
        /// </summary>
        public virtual void Touch(Entity other) { other.Damage(this, DPower); }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch bather, GameTime gameTime) { }
    }
}
