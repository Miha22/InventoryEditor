using System;
using System.Collections.Generic;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.API;

namespace ItemRestrictorAdvanced
{
    sealed class CommandUIEmergency : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "emergencyuistop";
        public string Help => "Use this command in case if UI craches and everybody has non clearable UI";
        public string Syntax => "/emuistop";
        public List<string> Aliases => new List<string>() { "emuistop" };
        public List<string> Permissions => new List<string>() { "rocket.emergencyuistop", "rocket.emuistop" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            ManageUI.UnLoad();
            //Console.WriteLine(ManageUI.Instances == null);
            Console.WriteLine();
            Logger.Log($"InventoryEditor was immediately stoped by {caller.DisplayName} at {DateTime.Now.TimeOfDay}", ConsoleColor.Yellow);
            Console.WriteLine();
        }
    }
}
