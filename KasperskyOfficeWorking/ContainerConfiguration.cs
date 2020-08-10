using Autofac;
using KasperskyOfficeWorking.Services;
using KasperskyOfficeWorking.Settings;
using Microsoft.Extensions.Configuration;
using Telegrom;
using Telegrom.Core.Extensions;
using Telegrom.Database.InMemory;
using Telegrom.Database.Sqlite;
using Telegrom.StateMachine;
using Telegrom.TelegramService;

namespace KasperskyOfficeWorking
{
    public class ContainerConfiguration
    {
        internal static void Init(IConfiguration configuration, ContainerBuilder builder)
        {
            var appSettings = new AppSettings
            {
                DatabaseSettings = configuration.GetSection("Database").Get<DatabaseSettings>(),
                TelegramSettings = configuration.GetSection("Telegram").Get<TelegramSettings>()
            };

            builder.RegisterInstance(appSettings).SingleInstance();

            builder.RegisterType<EmailService>()
                .InstancePerUpdate();

            builder.RegisterType<OfficeDayService>()
                .InstancePerUpdate();

            builder.RegisterType<CalendarConditionBuilder>()
                .InstancePerUpdate();

            var stateMachineBuilder = StateMachineBuilderFactory.Create();

            TelegromConfiguration.Configuration
                .AddTelegramOptions(new TelegramOptions
                {
                    TelegramApiToken = appSettings.TelegramSettings.TelegramApiToken
                })
                //.UseSqliteDatabase(appSettings.DatabaseSettings.ConnectionString, optionsBuilder => optionsBuilder.EnableSensitiveDataLogging())
                .UseInMemoryDatabase("23", optionsBuilder => optionsBuilder.EnableSensitiveDataLogging())
                .AddStateMachineBuilder(stateMachineBuilder);
        }
    }
}
