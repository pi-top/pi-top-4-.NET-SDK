using System;
using System.Collections.Generic;
using System.Linq;

namespace PiTop
{
    internal class PiTopMessage {
        public PiTopMessage(PiTopMessageId messageId, params string[] parameters)
        {
            Id = messageId;
            Parameters = parameters ?? Enumerable.Empty<string>();

        }

        public PiTopMessage(PiTopMessageId messageId, IEnumerable<string> parameters)
        {
            Id = messageId;
            Parameters = parameters ?? Enumerable.Empty<string>();

        }

        public static PiTopMessage Parse(string message)
        {
            var parts = message.Split(new[] {'|'}, StringSplitOptions.None);

            var id = Enum.Parse<PiTopMessageId>(parts[0]);

            return new PiTopMessage(id, parts.Skip(1));
        }

        public IEnumerable<string> Parameters { get;  }

        public PiTopMessageId Id { get;  }
    }
}