using Microsoft.Xna.Framework.Content;

using CorridorGravity.Controllers;

namespace CorridorGravity.Services
{
    /// <summary>
    ///  Helps/Provides other ingame entities with services
    /// </summary>
    class ServiceLocator
    {
        public string               RootDirectory { get { return Content.RootDirectory; } }
        public ContentManager       Content     { get; private set; }

        public EntityController     AIHandler   { get; private set; }
        public EntityController     CharHandler { get; private set; }
        public AnimationLoader      ALoader     { get; private set; }
        public PipelineManager      PLManager   { get; private set; }
        public AudioManager         AManager    { get; private set; }

        private static ServiceLocator _Instance;
        public static ServiceLocator Instance
        { get {
                if (_Instance == null)
                    _Instance = new ServiceLocator();
                return _Instance;
            } }

        private ServiceLocator() { }

        public void Initialize(ContentManager content)
        {
            this.Content = content;

            this.AIHandler   = new EnemyController();
            this.CharHandler = new PlayerController();

            this.ALoader     = AnimationLoader.Instance;
            this.ALoader.Initialize();

            this.PLManager   = PipelineManager.Instance;
            this.PLManager.Initialize(content);

            this.AManager    = AudioManager.Instance;
            this.AManager.Initialize();
        }

        public void UnloadContent()
        {
            PLManager.UnloadContent();
        }
    }
}
