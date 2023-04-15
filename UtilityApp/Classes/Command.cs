using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace UtilityApp.Classes {

    public enum ExecutionMethod {
        Cmd = 0,
        Application = 1,
        Browser = 2
    }

    public class Command {

		public Color Color { get; private set; }

		public List<string> Keywords { get; private set; }

		public ExecutionMethod ExecutionMethod { get; private set; }

		public Command(Color color, List<string> keywords, ExecutionMethod executionMethod) {
			Color = color;
			Keywords = keywords;
			ExecutionMethod = executionMethod;
        }
	}
}
