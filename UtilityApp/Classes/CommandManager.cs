using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityApp.Classes {
    public static class CommandManager {

        private static string CommandIsNullError => "No command given";

        public static async Task<CommandResponse> RunCommand(string? command) {
            CommandResponse response = new();
            if (command == null) {
                response.CommandErrorMessage = CommandIsNullError;
                return response;
            }

            if (command.Contains("cmd ")) {
                response.Response = await ProgramLauncher.RunCmd(command.Split("cmd ")[1]);
            }

            response.IsCorrect = false;
            return response;
        }
    }
}
