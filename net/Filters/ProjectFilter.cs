using FiltexNet;
using FiltexNet.Models;
using FiltexNet.Options;

namespace net.Filters
{
    public static class Filters
    {
        private static Filtex _instance;

        public static Filtex ProjectFilter()
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = Filtex.New(
                FieldOption.New().String().Name("name").Label("Name").Nullable(),
                FieldOption.New().Number().Name("version").Label("Version"),
                FieldOption.New().String().Array().Name("tags").Label("Tags").Nullable(),
                FieldOption.New().Boolean().Name("status").Label("Status").Lookup("statuses"),
                FieldOption.New().DateTime().Name("createdAt").Label("Created At"),
                LookupOption.New().Key("statuses").Values(new[]
                {
                    new Lookup("Enabled", true),
                    new Lookup("Disabled", false),
                }));

            return _instance;
        }
    }
}