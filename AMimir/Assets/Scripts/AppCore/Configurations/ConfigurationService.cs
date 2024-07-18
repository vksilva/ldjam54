namespace Busta.AppCore.Configurations
{
    public class ConfigurationService
    {
        public GameConfigurations Configs { get; private set; }

        public void Init(GameConfigurations gameConfigurations)
        {
            Configs = gameConfigurations;
        }
    }
}