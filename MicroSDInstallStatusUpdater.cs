using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MicroSDInstallStatusUpdater
{
    public class MicroSDInstallStatusUpdater : GenericPlugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        private MicroSDInstallStatusUpdaterSettingsViewModel settings { get; set; }

        public override Guid Id { get; } = Guid.Parse("216a1883-0794-4381-b5e3-8ccf42ad1463");

        public MicroSDInstallStatusUpdater(IPlayniteAPI api) : base(api)
        {
            settings = new MicroSDInstallStatusUpdaterSettingsViewModel(this);
            Properties = new GenericPluginProperties
            {
                HasSettings = false
            };
        }

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            var insertWatcher = new ManagementEventWatcher(new WqlEventQuery(
                      "__InstanceCreationEvent", new TimeSpan(0, 0, 1),
                      "TargetInstance isa 'Win32_DiskDrive' AND TargetInstance.Model LIKE '%SDXC Card%'"));
            insertWatcher.EventArrived += SDCardInserted;
            logger.Info("[SDCardInstallStatusUpdater] SDCard Insert Watcher Registered!");

            var removeWatcher = new ManagementEventWatcher(new WqlEventQuery(
                "__InstanceDeletionEvent", new TimeSpan(0, 0, 1),
                "TargetInstance isa 'Win32_DiskDrive' AND TargetInstance.Model LIKE '%SDXC Card%'"));
            removeWatcher.EventArrived += SDCardRemoved;
            logger.Info("[SDCardInstallStatusUpdater] SDCard Remove Watcher Registered!");
            logger.Info("[SDCardInstallStatusUpdater] SDCard Watc");

            insertWatcher.Start();
            removeWatcher.Start();
        }

        private void SDCardRemoved(object sender, EventArrivedEventArgs e)
        {
            logger.Info("[SDCardInstallStatusUpdater] SDCard Removed! Updating Database!");
            foreach (var game in PlayniteApi.Database.Games)
            {
                var InstallDir = game.InstallDirectory;

                if (!Directory.Exists(InstallDir) || InstallDir == null)
                {
                    logger.Info("[SDCardInstallStatusUpdater] Game '" + game.Name + "' is not found anymore. Updateing Status to Uninstalled");
                    game.IsInstalled = false;
                    PlayniteApi.Database.Games.Update(game);
                }
                if (Directory.Exists(InstallDir))
                {
                    logger.Info("[SDCardInstallStatusUpdater] Game '" + game.Name + "' is now found!. Updateing Status to Installed");
                    game.IsInstalled = true;
                    PlayniteApi.Database.Games.Update(game);
                }
            }
        }

        private void SDCardInserted(object sender, EventArrivedEventArgs e)
        {
            logger.Info("[SDCardInstallStatusUpdater] SDCard Inserted! Sleeping a bit to let Windows handle and mount shit :3 Updating Database!");
            Thread.Sleep(5000);
            logger.Info("[SDCardInstallStatusUpdater] Sleep Done! Updating Database!");
            foreach (var game in PlayniteApi.Database.Games)
            {
                var InstallDir = game.InstallDirectory;

                if(!Directory.Exists(InstallDir) || InstallDir == null)
                {
                    logger.Info("[SDCardInstallStatusUpdater] Game '" + game.Name + "' is not found anymore. Updateing Status to Uninstalled");
                    game.IsInstalled = false;
                    PlayniteApi.Database.Games.Update(game);
                }
                if (Directory.Exists(InstallDir))
                {
                    logger.Info("[SDCardInstallStatusUpdater] Game '" + game.Name + "' is now found!. Updateing Status to Installed");
                    game.IsInstalled = true;
                    PlayniteApi.Database.Games.Update(game);
                }
            }
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return settings;
        }

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            return new MicroSDInstallStatusUpdaterSettingsView();
        }
    }
}