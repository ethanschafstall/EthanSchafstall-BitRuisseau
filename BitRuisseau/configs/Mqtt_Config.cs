namespace BitRuisseau.configs
{
    public static class Mqtt_Config
    {
        public static string Broker = "inf-n510-p301";

        public static int Port = 1883;

        public static string ClientId = Guid.NewGuid().ToString();

        public static string Topic = "test";

        public static string Username = "ict";

        public static string Password = "123";
    }
}
