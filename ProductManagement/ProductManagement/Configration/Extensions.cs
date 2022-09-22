namespace ProductManagement.Configration
{
    public static class Extensions
    {
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);

            return model;
        }

        public static string DatabaseConnectionString(this IConfiguration configuration)
        {
            var dbOptions = configuration.GetOptions<DatabaseSettings>(DatabaseSettings.SectionName);
            return dbOptions.ConnectionString;
        }
    }
}
