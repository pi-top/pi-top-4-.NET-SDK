using System;
using System.Collections.Generic;
using System.Linq;

namespace PiTop
{
    public class PiTopMessage {
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

        public override string ToString()
        {
            return  string.Join("|", new List<string> { ((int)Id).ToString() }.Concat(Parameters));
        }

        public static PiTopMessage Parse(string message)
        {
            var parts = message.Split(new[] {'|'}, StringSplitOptions.None);
            if (parts.Length < 1)
            {
                throw new InvalidOperationException("Invalid message, must have at least id.");
            }

            if (!int.TryParse(parts[0], out var parsedId))
            {
                throw new InvalidOperationException("Invalid message, id must be a valid integer");
            }
            
            
            var id = (PiTopMessageId)parsedId;

            return new PiTopMessage(id, parts.Skip(1));
        }

        public IEnumerable<string> Parameters { get;  }

        public PiTopMessageId Id { get;  }
    }
}