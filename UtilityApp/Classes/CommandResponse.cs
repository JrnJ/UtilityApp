using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityApp.Classes {
    public class CommandResponse {

        public bool IsCorrect { get; set; } = false;

        private string? _commandErrorMessage;

        public string? CommandErrorMessage {
            get { return _commandErrorMessage; }
            set { _commandErrorMessage = value; }
        }

        private string? _response;

        public string? Response {
            get { return _response; }
            set { _response = value; }
        }

        public CommandResponse() {
            
        }
    }
}
