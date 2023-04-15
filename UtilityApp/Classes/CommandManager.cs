using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UtilityApp.Classes {
    public class CommandManager {

        private string CommandIsNullError => "No command given";

        private List<Command> _commands;

        public List<Command> Commands {
            get { return _commands; }
            set { _commands = value; }
        }

        public CommandManager() {
            // TODO: file manager
            _commands = new();

            // TODO: yeah same, throw this in files
            Command runApplicationCommand = new(
                    Color.FromRgb(255, 0, 0),
                    new() {
                        "run", "launch"
                    },
                    ExecutionMethod.Application
                );

            Command browserCommand = new(
                    Color.FromRgb(0, 255, 0),
                    new() {
                        "browser", "chrome", "bing", "edge", "duckduckgo"
                    },
                    ExecutionMethod.Browser
                );

            Command cmdCommand = new(
                    Color.FromRgb(0, 255, 0),
                    new() {
                        "cmd"
                    },
                    ExecutionMethod.Browser
                );

            Command minecraftServerCommand = new(
                    Color.FromRgb(0, 255, 0),
                    new() {
                        "minecraft server", "mc server"
                    },
                    ExecutionMethod.Application
                );

            Command youtubeCommand = new(
                    Color.FromRgb(0, 255, 0),
                    new() {
                        "youtube", "yt"
                    },
                    ExecutionMethod.Browser
                );

            Commands.Add(runApplicationCommand);
            Commands.Add(browserCommand);
            Commands.Add(cmdCommand);
            Commands.Add(minecraftServerCommand);
        }

        // TODO: rename text
        public async Task<CommandResponse> RunCommand(string? text) {
            CommandResponse response = new();
            if (text == null) {
                response.CommandErrorMessage = CommandIsNullError;
                return response;
            }

            // Make sure casing wont be a problem
            // This might actually become a problem but skip for now
            text = text.ToLower();

            string keyword = text.Split(' ')[0];
            string query = text[keyword.Length..];

            // TODO: remove this temp code
            if (text.Contains("cmd ")) {
                response.Response = await ProgramLauncher.RunCmd(text.Split("cmd ")[1]);
            }

            // TODO: bad name fix it
            async Task FindThing() {
                // TODO: rename cmd
                foreach (Command cmd in Commands) {
                    foreach (string cmdKeyword in cmd.Keywords) {
                        if (keyword.Equals(cmdKeyword)) {
                            response.Response = await Run(cmd.ExecutionMethod, query);
                            return;
                        }
                    }
                }
            }
            await FindThing();

            response.IsCorrect = false;
            return response;
        }

        // TODO: return the CommandResponse as there is no purpose for only a message
        private async Task<string> Run(ExecutionMethod executionMethod, string extra) {
            switch (executionMethod) {
                case ExecutionMethod.Cmd:
                    return await ProgramLauncher.RunCmd(extra);
                case ExecutionMethod.Application:
                    ProgramLauncher.RunApplication(extra);
                    return "";
                case ExecutionMethod.Browser:
                    ProgramLauncher.RunBrowser(extra);
                    return "";
                default:
                    return "No method for given ExecutionMethod.";
            }
        }
    }
}
